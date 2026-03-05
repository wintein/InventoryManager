using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagerApp
{
    internal class InventoryItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public InventoryItem(string name, int quantity, decimal price, string category)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = category;
        }
    }
}
