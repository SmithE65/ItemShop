using System;
using System.Collections.Generic;

namespace StoreInventory
{
    /// <summary>
    /// Extension methods on the Item class, since I can't touch it
    /// </summary>
    public static class ItemExtensions
    {
        private static readonly Dictionary<string, Func<Item, Item>> ItemRules =
            new Dictionary<string, Func<Item, Item>>()
            {
                {"Fine Wine", (item) => item.DecrementSellIn().DegradeQuality(-1)},
                {"Arena Tickets", (item) =>
                    {
                        item.DecrementSellIn();
                        if (item.SellIn < 0)
                            item.Quality = 0; // Worth zero after the show
                        else if (item.SellIn <= 5)
                            item.DegradeQuality(-3); // Increases by 3 each day w/in 5 days of show
                        else if (item.SellIn <= 10)
                            item.DegradeQuality(-2); // Increases by 2 each day w/in 10 days of show
                        else
                            item.DegradeQuality(-1);
                        return item;
                    }
                },
                {"Conjured Item", (item) => item.DecrementSellIn().DegradeQuality(2)},
                {"Dragon Scale", (item) => item }
            };

        /// <summary>
        /// Registers a custom UpdateQuality rule for an item
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="rule"></param>
        public static void RegisterRule(string itemName, Func<Item, Item> rule)
        {
            ItemRules.Add(itemName, rule);
        }

        /// <summary>
        /// Unregisters a custom UpdateQuality rule for an item
        /// </summary>
        /// <param name="itemName"></param>
        public static void UnregisterRule(string itemName)
        {
            if (ItemRules.ContainsKey(itemName))
                ItemRules.Remove(itemName);
        }

        /// <summary>
        /// Degrades the Quality of Item by the specified amount
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount">Amount item degrades; amount is doubled passed SellIn date</param>
        /// <returns></returns>
        public static Item DegradeQuality(this Item item, int amount)
        {
            if (item.SellIn < 0 && amount > 0) // Whether 'degradation' doubles after sell date on items that increase in Quality is undefined; should ask.
                amount *= 2;                   // Degradation doubles after SellIn date

            item.Quality -= amount; // Degrade

            if (item.Quality < 0)
                item.Quality = 0; // Minimum is zero
            else if (item.Quality > 50)
                item.Quality = 50; // Maximum is 50

            return item; // Return item for chaining
        }

        /// <summary>
        /// Decrements SellIn by 1
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Item DecrementSellIn(this Item item)
        {
            item.SellIn--; // Decrement SellIn date

            return item; // Return item for method chaining
        }

        /// <summary>
        /// Runs special update rules for items in the ItemRules dictionary, otherwise standard update rules
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Item UpdateQuality(this Item item)
        {
            if (ItemRules.ContainsKey(item.Name)) // If the item has rules registered in the ItemRules dictionary
                ItemRules[item.Name](item);       // Run them.
            else
            {
                item.DecrementSellIn().DegradeQuality(1); // Otherwise run standard item rules
            }

            return item; // Return item for method chaining
        }
    }
}
