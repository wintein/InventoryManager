using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InventoryManagerApp
{
    public class InventoryManager
    {
        public List<InventoryItem> Items { get; private set; }
        public int LowStockThreshold { get; set; }
        public List<string> Notifications { get; private set; }
        private readonly string filePath;

        // Конструктор по умолчанию (использует "inventory.txt")
        public InventoryManager(int lowStockThreshold = 5)
            : this(lowStockThreshold, "inventory.txt")
        {
        }

        // Конструктор для тестирования (принимает путь к файлу)
        public InventoryManager(int lowStockThreshold, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "Путь к файлу не может быть пустым");
            }

            Items = new List<InventoryItem>();
            Notifications = new List<string>();
            LowStockThreshold = lowStockThreshold;
            this.filePath = filePath;
            LoadItems();
        }

        public void AddItem(InventoryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            Items.Add(item);
            CheckLowStock(item);
            SaveItems();
        }

        public void RemoveItem(InventoryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            Items.Remove(item);
            SaveItems();
        }

        public void UpdateItemQuantity(InventoryItem item, int newQuantity)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (newQuantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Количество не может быть отрицательным");
            }
            item.Quantity = newQuantity;
            CheckLowStock(item);
            SaveItems();
        }

        // Проверка низкого уровня запасов
        public void CheckLowStock(InventoryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.Quantity < LowStockThreshold)
            {
                string notification = $"ВНИМАНИЕ: Товар '{item.Name}' имеет низкий запас! " +
                                    $"Текущее количество: {item.Quantity}, Порог: {LowStockThreshold}";

                if (!Notifications.Contains(notification))
                {
                    Notifications.Add(notification);
                }
            }
        }

        // Установка порога уведомления
        public void SetLowStockThreshold(int threshold)
        {
            if (threshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold),
                    "Порог не может быть отрицательным");
            }
            LowStockThreshold = threshold;
            foreach (var item in Items)
            {
                CheckLowStock(item);
            }
        }

        // Получение всех уведомлений
        public List<string> GetNotifications()
        {
            return new List<string>(Notifications);
        }

        // Очистка уведомлений
        public void ClearNotifications()
        {
            Notifications.Clear();
        }

        // Проверка, есть ли уведомления
        public bool HasNotifications()
        {
            return Notifications.Count > 0;
        }

        private void SaveItems()
        {
            try
            {
                File.WriteAllLines(filePath, Items.Select(i =>
                    $"{i.Name}|{i.Quantity}|{i.Price}|{i.Category}"));
            }
            catch (IOException ex)
            {
                throw new IOException($"Не удалось сохранить данные в файл '{filePath}': {ex.Message}", ex);
            }
        }

        private void LoadItems()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 4)
                        {
                            int quantity;
                            decimal price;
                            if (int.TryParse(parts[1], out quantity) &&
                                decimal.TryParse(parts[2], out price))
                            {
                                Items.Add(new InventoryItem(parts[0], quantity, price, parts[3]));
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    throw new IOException($"Не удалось загрузить данные из файла '{filePath}': {ex.Message}", ex);
                }
            }
        }
    }
}