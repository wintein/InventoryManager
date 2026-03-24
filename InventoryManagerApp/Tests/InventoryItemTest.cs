using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InventoryManagerApp.Tests
{
    [TestClass]
    public class InventoryItemTests
    {
        // проверяет, что конструктор корректно устанавливает все свойства
        [TestMethod]
        public void ValidParameters()
        {
            // Arrange
            string expectedName = "Ноутбук";
            int expectedQuantity = 10;
            decimal expectedPrice = 999.99m;
            string expectedCategory = "Электроника";

            // Act
            var item = new InventoryItem(expectedName, expectedQuantity, expectedPrice, expectedCategory);

            // Assert
            Assert.AreEqual(expectedName, item.Name);
            Assert.AreEqual(expectedQuantity, item.Quantity);
            Assert.AreEqual(expectedPrice, item.Price);
            Assert.AreEqual(expectedCategory, item.Category);
        }

        // проверяет, что конструктор корректно обрабатывает пустые строки и null значения
        [TestMethod]
        [DataRow("", 5, 100.50, "Тест", "")]
        [DataRow(null, 5, 100.50, "Тест", null)]
        public void EmptyOrNullStrings(
            string name,
            int quantity,
            double price,
            string category,
            string expectedName)
        {
            // Arrange
            decimal expectedPrice = (decimal)price;
            string expectedCategory = category;

            // Act
            var item = new InventoryItem(name, quantity, expectedPrice, category);

            // Assert
            Assert.AreEqual(expectedName, item.Name);
            Assert.AreEqual(quantity, item.Quantity);
            Assert.AreEqual(expectedPrice, item.Price);
            Assert.AreEqual(expectedCategory, item.Category);
        }

        // проверяет, что конструктор корректно устанавливает количество при различных значениях
        [TestMethod]
        [DataRow("Товар", 0, 50.00, "Категория", 0)]
        [DataRow("Товар", -5, 50.00, "Категория", -5)]
        [DataRow("Товар", 100, 50.00, "Категория", 100)]
        public void VariousQuantities(
            string name,
            int quantity,
            double price,
            string category,
            int expectedQuantity)
        {
            // Arrange
            decimal expectedPrice = (decimal)price;

            // Act
            var item = new InventoryItem(name, quantity, expectedPrice, category);

            // Assert
            Assert.AreEqual(expectedQuantity, item.Quantity);
        }

        // проверяет, что конструктор корректно устанавливает цену при различных значениях
        [TestMethod]
        [DataRow("Товар", 10, 0, "Категория", 0)]
        [DataRow("Товар", 10, -50.75, "Категория", -50.75)]
        [DataRow("Товар", 10, 999.99, "Категория", 999.99)]
        public void VariousPrices(
            string name,
            int quantity,
            double price,
            string category,
            double expectedPrice)
        {
            // Arrange
            decimal priceDecimal = (decimal)price;
            decimal expectedPriceDecimal = (decimal)expectedPrice;

            // Act
            var item = new InventoryItem(name, quantity, priceDecimal, category);

            // Assert
            Assert.AreEqual(expectedPriceDecimal, item.Price);
        }

        // проверяет, что свойство Name можно изменить после создания объекта
        [TestMethod]
        [DataRow("Старое имя", 5, 10.99, "Категория", "Новое имя")]
        [DataRow("Товар", 10, 99.99, "Электроника", "Смартфон")]
        [DataRow("Книга", 3, 450.00, "Литература", "Учебник")]
        public void ChangingName(
            string initialName,
            int quantity,
            double price,
            string category,
            string newName)
        {
            // Arrange
            decimal priceDecimal = (decimal)price;
            var item = new InventoryItem(initialName, quantity, priceDecimal, category);

            // Act
            item.Name = newName;

            // Assert
            Assert.AreEqual(newName, item.Name);
        }

        // проверяет, что свойство Quantity можно изменить после создания объекта
        [TestMethod]
        [DataRow("Товар", 5, 10.99, "Категория", 15)]
        [DataRow("Товар", 0, 10.99, "Категория", 25)]
        [DataRow("Товар", 100, 10.99, "Категория", 0)]
        public void ChangingQuantity(
            string name,
            int initialQuantity,
            double price,
            string category,
            int newQuantity)
        {
            // Arrange
            decimal priceDecimal = (decimal)price;
            var item = new InventoryItem(name, initialQuantity, priceDecimal, category);

            // Act
            item.Quantity = newQuantity;

            // Assert
            Assert.AreEqual(newQuantity, item.Quantity);
        }

        // проверяет, что свойство Price можно изменить после создания объекта
        [TestMethod]
        [DataRow("Товар", 5, 10.99, "Категория", 25.50)]
        [DataRow("Товар", 5, 0, "Категория", 99.99)]
        [DataRow("Товар", 5, 100.00, "Категория", 0)]
        public void ChangingPrice(
            string name,
            int quantity,
            double initialPrice,
            string category,
            double newPrice)
        {
            // Arrange
            decimal initialPriceDecimal = (decimal)initialPrice;
            decimal newPriceDecimal = (decimal)newPrice;
            var item = new InventoryItem(name, quantity, initialPriceDecimal, category);

            // Act
            item.Price = newPriceDecimal;

            // Assert
            Assert.AreEqual(newPriceDecimal, item.Price);
        }

        // проверяет, что свойство Category можно изменить после создания объекта
        [TestMethod]
        [DataRow("Товар", 5, 10.99, "Старая категория", "Новая категория")]
        [DataRow("Товар", 5, 10.99, "", "Электроника")]
        [DataRow("Товар", 5, 10.99, null, "Мебель")]
        public void ChangingCategory(
            string name,
            int quantity,
            double price,
            string initialCategory,
            string newCategory)
        {
            // Arrange
            decimal priceDecimal = (decimal)price;
            var item = new InventoryItem(name, quantity, priceDecimal, initialCategory);

            // Act
            item.Category = newCategory;

            // Assert
            Assert.AreEqual(newCategory, item.Category);
        }

        // проверяет, что конструктор корректно обрабатывает максимальные/большие значения свойств
        [TestMethod]
        public void BigValues()
        {
            // Arrange
            string expectedName = new string('A', 1000);
            int expectedQuantity = int.MaxValue;
            decimal expectedPrice = decimal.MaxValue;
            string expectedCategory = new string('B', 1000);

            // Act
            var item = new InventoryItem(expectedName, expectedQuantity, expectedPrice, expectedCategory);

            // Assert
            Assert.AreEqual(expectedName, item.Name);
            Assert.AreEqual(expectedQuantity, item.Quantity);
            Assert.AreEqual(expectedPrice, item.Price);
            Assert.AreEqual(expectedCategory, item.Category);
        }

        // проверяет, что конструктор корректно обрабатывает минимальные значения свойств
        [TestMethod]
        public void SmallValues()
        {
            // Arrange
            string expectedName = null;
            int expectedQuantity = int.MinValue;
            decimal expectedPrice = decimal.MinValue;
            string expectedCategory = null;

            // Act
            var item = new InventoryItem(expectedName, expectedQuantity, expectedPrice, expectedCategory);

            // Assert
            Assert.AreEqual(expectedName, item.Name);
            Assert.AreEqual(expectedQuantity, item.Quantity);
            Assert.AreEqual(expectedPrice, item.Price);
            Assert.AreEqual(expectedCategory, item.Category);
        }
    }
}