namespace InventoryManagerApp
{
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
            this.Width = 540;
            this.Height = 350;

            Label nameLabel = new Label
            {
                Location = new System.Drawing.Point(10, 10),
                Text = "Название:",
                Width = 70
            };
            nameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(85, 7),
                Width = 120,
                PlaceholderText = "Введите название"
            };

            Label quantityLabel = new Label
            {
                Location = new System.Drawing.Point(215, 10),
                Text = "Количество:",
                Width = 75
            };
            quantityTextBox = new TextBox
            {
                Location = new System.Drawing.Point(295, 7),
                Width = 60,
                PlaceholderText = "шт"
            };

            Label priceLabel = new Label
            {
                Location = new System.Drawing.Point(365, 10),
                Text = "Цена:",
                Width = 40
            };
            priceTextBox = new TextBox
            {
                Location = new System.Drawing.Point(410, 7),
                Width = 60,
                PlaceholderText = "руб"
            };

            Label categoryLabel = new Label
            {
                Location = new System.Drawing.Point(10, 35),
                Text = "Категория:",
                Width = 70
            };
            categoryTextBox = new TextBox
            {
                Location = new System.Drawing.Point(85, 32),
                Width = 120,
                PlaceholderText = "Введите категорию"
            };

            addItemButton = new Button
            {
                Location = new System.Drawing.Point(220, 33),
                Text = "Добавить",
                Width = 90
            };
            addItemButton.Click += AddItemButton_Click;

            removeItemButton = new Button
            {
                Location = new System.Drawing.Point(315, 33),
                Text = "Удалить",
                Width = 90
            };
            removeItemButton.Click += RemoveItemButton_Click;

            updateQuantityButton = new Button
            {
                Location = new System.Drawing.Point(410, 33),
                Text = "Обновить",
                Width = 90
            };
            updateQuantityButton.Click += UpdateQuantityButton_Click;

            Label itemsListLabel = new Label
            {
                Location = new System.Drawing.Point(10, 65),
                Text = "Список товаров:",
                Width = 150,
                Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold)
            };
            itemsListBox = new ListBox
            {
                Location = new System.Drawing.Point(10, 90),
                Width = 500,
                Height = 200
            };

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(quantityLabel);
            this.Controls.Add(quantityTextBox);
            this.Controls.Add(priceLabel);
            this.Controls.Add(priceTextBox);
            this.Controls.Add(categoryLabel);
            this.Controls.Add(categoryTextBox);
            this.Controls.Add(addItemButton);
            this.Controls.Add(removeItemButton);
            this.Controls.Add(updateQuantityButton);
            this.Controls.Add(itemsListLabel);
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
    }
}