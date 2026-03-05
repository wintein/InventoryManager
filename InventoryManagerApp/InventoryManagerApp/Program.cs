namespace InventoryManagerApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public class InventoryForm : Form
        {
            private InventoryManager inventoryManager;
            private TextBox nameTextBox;
            private TextBox quantityTextBox;
            private TextBox priceTextBox;
            private TextBox categoryTextBox;
            private Button addItemButton;
            private Button removeItemButton;
            private Button updateQuantityButton;
            private ListBox itemsListBox;
            public InventoryForm()
            {
                this.Text = "Управление инвентарём";
                this.Width = 500;
                this.Height = 400;
                nameTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(10, 10),
                    Width = 150,
                    PlaceholderText = "Название"
                };
                quantityTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(170, 10),
                    Width = 80,
                    PlaceholderText = "Количество"
                };
                priceTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(260, 10),
                    Width = 100,
                    PlaceholderText = "Цена"
                };
                categoryTextBox = new TextBox
                {
                    Location = new System.Drawing.Point(370, 10),
                    Width = 100,
                    PlaceholderText = "Категория"
                };
                addItemButton = new Button
                {
                    Location = new System.Drawing.Point(10, 40),
                    Text = "Добавить",
                    Width = 100
                };
                addItemButton.Click += AddItemButton_Click;
                removeItemButton = new Button
                {
                    Location = new System.Drawing.Point(120, 40),
                    Text = "Удалить",
                    Width = 100
                };
                removeItemButton.Click += RemoveItemButton_Click;
                updateQuantityButton = new Button
                {
                    Location = new System.Drawing.Point(220, 40),
                    Text = "Обновить",
                    Width = 100
                };
                updateQuantityButton.Click += UpdateQuantityButton_Click;
                itemsListBox = new ListBox
                {
                    Location = new System.Drawing.Point(10, 70),
                    Width = 460,
                    Height = 200
                };
                this.Controls.Add(nameTextBox);
                this.Controls.Add(quantityTextBox);
                this.Controls.Add(priceTextBox);
                this.Controls.Add(categoryTextBox);
                this.Controls.Add(addItemButton);
                this.Controls.Add(removeItemButton);
                this.Controls.Add(updateQuantityButton);
                this.Controls.Add(itemsListBox);
                inventoryManager = new InventoryManager();
                UpdateItemsList();
            }
            private void UpdateItemsList()
            {
                itemsListBox.Items.Clear();
                foreach (var item in inventoryManager.Items)
                {
                    itemsListBox.Items.Add($"{item.Name} – Количество: {item.Quantity} | Цена: {item.Price} руб. | Категория: {item.Category}");
                }
            }
            private void AddItemButton_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrEmpty(nameTextBox.Text) ||
                string.IsNullOrEmpty(quantityTextBox.Text) || string.IsNullOrEmpty(priceTextBox.Text) ||
                string.IsNullOrEmpty(categoryTextBox.Text))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }
                int quantity;
                decimal price;
                if (!int.TryParse(quantityTextBox.Text, out quantity) ||
                !decimal.TryParse(priceTextBox.Text, out price))
                {
                    MessageBox.Show("Неверный формат количества или цены!");
                    return;
                }
                InventoryItem newItem = new InventoryItem(nameTextBox.Text, quantity, price,
                categoryTextBox.Text);
                try
                {
                    inventoryManager.AddItem(newItem);
                    nameTextBox.Clear();
                    quantityTextBox.Clear();
                    priceTextBox.Clear();
                    categoryTextBox.Clear();
                    UpdateItemsList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            private void RemoveItemButton_Click(object sender, EventArgs e)
            {
                if (itemsListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите товар для удаления!");
                    return;
                }
                string selectedItem = itemsListBox.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
                if (parts.Length >= 2)
                {
                    string name = parts[0].Trim();
                    var itemToRemove = inventoryManager.Items.Find(i => i.Name == name);
                    if (itemToRemove != null)
                    {
                        try
                        {
                            inventoryManager.RemoveItem(itemToRemove);
                            UpdateItemsList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            private void UpdateQuantityButton_Click(object sender, EventArgs e)
            {
                if (itemsListBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите товар для обновления!");
                    return;
                }
                string selectedItem = itemsListBox.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
                if (parts.Length >= 2)
                {
                    string name = parts[0].Trim();
                    var itemToUpdate = inventoryManager.Items.Find(i => i.Name == name);
                    if (itemToUpdate != null)
                    {
                        if (string.IsNullOrEmpty(quantityTextBox.Text))
                        {
                            MessageBox.Show("Введите новое количество!");
                            return;
                        }
                        int newQuantity;
                        if (!int.TryParse(quantityTextBox.Text, out newQuantity))
                        {
                            MessageBox.Show("Неверный формат количества!");
                            return;
                        }
                        try
                        {
                            inventoryManager.UpdateItemQuantity(itemToUpdate, newQuantity);
                            UpdateItemsList();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new InventoryForm());
            }
        }

        //[STAThread]
        //static void Main()
        //{
        //    // To customize application configuration such as set high DPI settings or default font,
        //    // see https://aka.ms/applicationconfiguration.
        //    ApplicationConfiguration.Initialize();
        //    Application.Run(new Form1());
        //}
    }
}