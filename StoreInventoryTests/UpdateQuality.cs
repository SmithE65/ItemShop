using NUnit.Framework;

using StoreInventory;

namespace StoreInventoryTests
{
    public class UpdateQualityMethodTests
    {
        private Inventory _inventory;

        [SetUp]
        public void Setup()
        {
            _inventory = new Inventory(); // Reset the Inventory instance
            _inventory.Items.Clear(); // Clear the Items list so we can test one specific item
                        //  /\_____---- Is this cheating?  The rules said not to modify the Items property.
        }

        #region Standard Items

        // This should probably be two tests
        [Test]
        public void StandardItem()
        {
            Item item = new Item {Name = "item", SellIn = 10, Quality = 20};
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.SellIn, Is.EqualTo(9));    // Sell-In decreases by 1
            Assert.That(item.Quality, Is.EqualTo(19));  // Quality decreases by 1
        }

        [Test]
        public void QualityNotNegative()
        {
            Item item = new Item() {Name = "item", SellIn = 10, Quality = 0};
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.GreaterThanOrEqualTo(0)); // Doesn't drop below zero
        }

        [Test]
        public void QualityDegradationDoubledPostSellIn()
        {
            Item item = new Item() {Name = "item", SellIn = 0, Quality = 5};
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(3)); // Drops by two
        }

        #endregion

        #region Fine Wine

        [Test]
        public void AgedBrieQualityIncreases()
        {
            Item item = new Item() { Name = "Fine Wine", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(6)); // Increases by one
        }

        [Test]
        public void QualityNeverMoreThan50_Initially50()
        {
            Item item = new Item() { Name = "Fine Wine", SellIn = 5, Quality = 50 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(50)); // Remains at 50
        }

        // This fails with the original UpdateQuality()
        [Test]
        public void QualityNeverMoreThan50_InitiallyOver50()
        {
            Item item = new Item() { Name = "Fine Wine" +
                                            "", SellIn = 5, Quality = 55 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(50)); // "The Quality of an item is never more than 50."
        }

        #endregion

        #region Arena Tickets

        // Test initially fails!
        [Test]
        public void ArenaTickets_QualityIncreases()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = 15, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(6)); // Increases by one
        }
        
        // Test initially fails!
        [Test]
        public void ArenaTickets_QualityIncreasesBy2_10DaysOrLess()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = 10, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();
                                                              // SellIn <= 10
            Assert.That(item.Quality, Is.EqualTo(7)); // Increases by two
        }

        // Test initially fails!
        [Test]
        public void ArenaTickets_QualityIncreasesBy3_5DaysOrLess()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();
                                                              // SellIn <= 5
            Assert.That(item.Quality, Is.EqualTo(8)); // Increases by three
        }

        [Test]
        public void ArenaTickets_QualityIsZero_AfterConcert()
        {
            Item item = new Item() { Name = "Arena Tickets", SellIn = -1, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();
                                                              // SellIn < 0
            Assert.That(item.Quality, Is.EqualTo(0)); // Drops to zero
        }

        #endregion

        #region Conjured Item

        // New item type with special rules

        [Test]
        public void ConjuredItem_DegradesTwiceAsFast_PreSellIn()
        {
            Item item = new Item() { Name = "Conjured Item", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(3)); // Decreases by two
        }

        [Test]
        public void ConjuredItem_DegradesTwiceAsFast_PostSellIn()
        {
            Item item = new Item() { Name = "Conjured Item", SellIn = -1, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(1)); // Decreases by four
        }

        #endregion

        #region Dragon Scale

        // Dragon Scale is a special item that does not change on UpdateQuality()

        [Test]
        public void DragonScaleQualityNeverDecreases()
        {
            Item item = new Item() { Name = "Dragon Scale", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.Quality, Is.EqualTo(5)); // Remains unchanged
        }

        [Test]
        public void DragonScaleSellInNeverDecreases()
        {
            Item item = new Item() { Name = "Dragon Scale", SellIn = 5, Quality = 5 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.SellIn, Is.EqualTo(5)); // Remains unchanged
        }

        [Test]
        public void DragonScale_Over50_NoChange()
        {
            Item item = new Item() { Name = "Dragon Scale", SellIn = 0, Quality = 80 };
            _inventory.Items.Add(item);

            _inventory.UpdateQuality();

            Assert.That(item.SellIn, Is.EqualTo(0));    // Remains unchanged
            Assert.That(item.Quality, Is.EqualTo(80));  // Remains unchanged
        }

        #endregion
    }
}