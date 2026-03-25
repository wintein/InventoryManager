using Microsoft.VisualStudio.TestTools.UnitTesting;
using InventoryManagerApp;
using System;
using System.IO;
using System.Threading;

namespace InventoryManagerApp.Tests
{
    [TestClass]
    [DoNotParallelize]
    public class InventoryManagerTests
    {
        private const string TestFileName = "inventory.txt";

        [TestInitialize]
        public void TestInitialize()
        {
            DeleteFileWithRetry(TestFileName);
            Thread.Sleep(100);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteFileWithRetry(TestFileName);
            Thread.Sleep(100);
        }

        private void DeleteFileWithRetry(string path)
        {
            if (!File.Exists(path)) return;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    File.Delete(path);
                    return;
                }
                catch (IOException)
                {
                    Thread.Sleep(500);
                }
                catch (UnauthorizedAccessException)
                {
                    Thread.Sleep(500);
                }
            }
        }

        // проверяет, что конструктор корректно инициализирует список товаров
        [TestMethod]
        public void InitializeItemsList()
        {
            // Arrange and Act
            var manager = new InventoryManager();

            // Assert
            Assert.IsNotNull(manager.Items);
            Assert.IsEmpty(manager.Items);
        }

        // проверяет метод для добавления товаров в список
        [TestMethod]
        public void AddItemToList()
        {
            // Arrange
            var manager = new InventoryManager();
            var item = new InventoryItem("Ноутбук", 5, 1000m, "Электроника");

            // Act
            manager.AddItem(item);

            // Assert
            Assert.HasCount(1, manager.Items);
            Assert.Contains(item, manager.Items);
        }

        // проверяет метод для удаления товаров из списка
        [TestMethod]
        public void RemoveItemFromList()
        {
            // Arrange
            var manager = new InventoryManager();
            var item = new InventoryItem("Мышь", 10, 20m, "Электроника");
            manager.AddItem(item);

            // Act
            manager.RemoveItem(item);

            // Assert
            Assert.IsEmpty(manager.Items);
        }

        // проверяет метод для изменения количества товара в списке
        [TestMethod]
        public void UpdateItemQuantity()
        {
            // Arrange
            var manager = new InventoryManager();
            var item = new InventoryItem("Клавиатура", 5, 50m, "Электроника");
            manager.AddItem(item);

            // Act
            manager.UpdateItemQuantity(item, 100);

            // Assert
            Assert.AreEqual(100, item.Quantity);
        }

        // проверяет возможность получения списка для чтения через геттер
        [TestMethod]
        public void ItemsIsReadOnly()
        {
            // Arrange
            var manager = new InventoryManager();

            // Act and Assert
            var items = manager.Items;
            Assert.IsNotNull(items);
        }
    }
}