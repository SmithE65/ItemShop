using System.Collections.Generic;

namespace StoreInventory
{
    public class Inventory
    {
        // Can't change this property  (I did make it a property)
        public IList<Item> Items { get; } = new List<Item>
        {
            new Item { Name = "+5 Vest of Dexterity", SellIn = 10, Quality = 20 },
            new Item { Name = "Fine Wine", SellIn = 2, Quality = 0 },
            new Item { Name = "Elixir of Light", SellIn = 5, Quality = 7 },
            new Item { Name = "Dragon Scale", SellIn = 0, Quality = 80 },
            new Item { Name = "Arena Tickets", SellIn = 15, Quality = 20 },
            new Item { Name = "Conjured Item", SellIn = 3, Quality = 6 }
        };

        /// <summary>
        /// Updates Quality for each Item in Items list applying custom rules for certain objects
        /// </summary>
        public void UpdateQuality()
        {
            foreach (Item item in Items)
            {
                item.UpdateQuality();
                //switch (item.Name)
                //{
                //    case "Fine Wine":
                //        item.DecrementSellIn().DegradeQuality(-1); // Brie increases in quality
                //        break;
                //    case "Arena Tickets":
                //        item.DecrementSellIn();
                //        if (item.SellIn < 0)
                //            item.Quality = 0; // Worth zero after the show
                //        else if (item.SellIn <= 5)
                //            item.DegradeQuality(-3); // Increases by 3 each day w/in 5 days of show
                //        else if (item.SellIn <= 10)
                //            item.DegradeQuality(-2); // Increases by 2 each day w/in 10 days of show
                //        else
                //            item.DegradeQuality(-1); // Increases in quality otherwise
                //        break;
                //    case "Conjured Item":
                //        item.DecrementSellIn().DegradeQuality(2); // Conjured Item items degrade twice as fast
                //        break;
                //    case "Dragon Scale": // We don't touch Dragon Scale
                //        break;
                //    default:
                //        item.DecrementSellIn().DegradeQuality(1); // Standard items degrade by 1
                //        break;
                //}
            }
        }
    }
}
