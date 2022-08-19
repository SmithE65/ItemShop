using Xunit;
using StoreInventory;

namespace StoreInventoryTests
{
    public class UpdateQualityMethodTests
    {
        private Inventory _inventory;

        public UpdateQualityMethodTests()
        {
            _inventory = new Inventory(); // Reset the Inventory instance
            _inventory.Items.Clear(); // Clear the Items list so we can test one specific item
                        //  /\_____---- Is this cheating?  The rules said not to modify the Items property.
        }

        #region Standard Items

        // This should probably be two tests
        [Fact]
        public void StandardItem()
        {
            Item item = new Item {Name = "item", SellIn = 10, Quality = 20};
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(9, item.SellIn);    // Sell-In decreases by 1
            Assert.Equal(19, item.Quality);  // Quality decreases by 1
        }

        [Fact]
        public void QualityNotNegative()
        {
            Item item = new Item() {Name = "item", SellIn = 10, Quality = 0};
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.True(item.Quality >= 0); // Doesn't drop below zero
        }

        [Fact]
        public void QualityDegradationDoubledPostSellIn()
        {
            Item item = new Item() {Name = "item", SellIn = 0, Quality = 5};
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(3, item.Quality); // Drops by two
        }

        #endregion

        #region Fine Wine

        [Fact]
        public void AgedBrieQualityIncreases()
        {
            Item item = new Item() { Name = "Fine Wine", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(6, item.Quality); // Increases by one
        }

        [Fact]
        public void QualityNeverMoreThan50_Initially50()
        {
            Item item = new Item() { Name = "Fine Wine", SellIn = 5, Quality = 50 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(50, item.Quality); // Remains at 50
        }

        // This fails with the original UpdateQuality()
        [Fact]
        public void QualityNeverMoreThan50_InitiallyOver50()
        {
            Item item = new Item() { Name = "Fine Wine" +
                                            "", SellIn = 5, Quality = 55 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(50, item.Quality); // "The Quality of an item is never more than 50."
        }

        #endregion

        #region Arena Tickets

        // Test initially fails!
        [Fact]
        public void ArenaTickets_QualityIncreases()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = 15, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(6, item.Quality); // Increases by one
        }
        
        // Test initially fails!
        [Fact]
        public void ArenaTickets_QualityIncreasesBy2_10DaysOrLess()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = 10, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();
                                                              // SellIn <= 10
            Assert.Equal(7, item.Quality); // Increases by two
        }

        // Test initially fails!
        [Fact]
        public void ArenaTickets_QualityIncreasesBy3_5DaysOrLess()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();
                                                              // SellIn <= 5
            Assert.Equal(8, item.Quality); // Increases by three
        }

        [Fact]
        public void ArenaTickets_QualityIsZero_AfterConcert()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = -1, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();
                                                              // SellIn < 0
            Assert.Equal(0, item.Quality); // Drops to zero
        }

        #endregion

        #region Conjured Item

        // New item type with special rules

        [Fact]
        public void ConjuredItem_DegradesTwiceAsFast_PreSellIn()
        {
            Item item = new Item() { Name = "Conjured Item", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(3, item.Quality); // Decreases by two
        }

        [Fact]
        public void ConjuredItem_DegradesTwiceAsFast_PostSellIn()
        {
            Item item = new Item() { Name = "Conjured Item", SellIn = -1, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(1, item.Quality); // Decreases by four
        }

        #endregion

        #region Dragon Scale

        // Dragon Scale is a special item that does not change on UpdateQuality()

        [Fact]
        public void DragonScaleQualityNeverDecreases()
        {
            Item item = new Item() { Name = "Dragon Scale", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(5, item.Quality); // Remains unchanged
        }

        [Fact]
        public void DragonScaleSellInNeverDecreases()
        {
            Item item = new Item() { Name = "Dragon Scale", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(5, item.SellIn); // Remains unchanged
        }

        [Fact]
        public void DragonScale_Over50_NoChange()
        {
            Item item = new Item() { Name = "Dragon Scale", SellIn = 0, Quality = 80 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.Equal(0, item.SellIn);    // Remains unchanged
            Assert.Equal(80, item.Quality);  // Remains unchanged
        }

        #endregion
    }
}