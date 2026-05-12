using System;
using System.Drawing;
using System.Windows.Forms;

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
        private Button showNotificationsButton;
        private Button setThresholdButton;
        private NumericUpDown thresholdNumericUpDown;
        private ListBox notificationsListBox;
        private Label lowStockLabel;
        private Button clearNotificationsButton;

        public InventoryForm()
        {
            this.Text = "Управление инвентарём";
            this.Width = 560;
            this.Height = 600;

            InitializeInventoryManager();
            CreateInputFields();
            CreateButtons();
            CreateNotificationsSection();

            UpdateItemsList();
            UpdateNotificationsDisplay();
        }

        private void InitializeInventoryManager()
        {
            inventoryManager = new InventoryManager(5);
        }

        private void CreateInputFields()
        {
            // Название
            Label nameLabel = new Label
            {
                Location = new Point(10, 10),
                Text = "Название:",
                Width = 70
            };
            nameTextBox = new TextBox
            {
                Location = new Point(85, 7),
                Width = 120,
                PlaceholderText = "Введите название"
            };

            // Количество
            Label quantityLabel = new Label
            {
                Location = new Point(215, 10),
                Text = "Количество:",
                Width = 75
            };
            quantityTextBox = new TextBox
            {
                Location = new Point(295, 7),
                Width = 60,
                PlaceholderText = "шт"
            };

            // Цена
            Label priceLabel = new Label
            {
                Location = new Point(365, 10),
                Text = "Цена:",
                Width = 40
            };
            priceTextBox = new TextBox
            {
                Location = new Point(410, 7),
                Width = 60,
                PlaceholderText = "руб"
            };

            // Категория
            Label categoryLabel = new Label
            {
                Location = new Point(10, 40),
                Text = "Категория:",
                Width = 70
            };
            categoryTextBox = new TextBox
            {
                Location = new Point(85, 37),
                Width = 120,
                PlaceholderText = "Введите категорию"
            };

            // Список товаров
            Label itemsListLabel = new Label
            {
                Location = new Point(10, 75),
                Text = "Список товаров:",
                Width = 150,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            itemsListBox = new ListBox
            {
                Location = new Point(10, 100),
                Width = 520,
                Height = 180
            };

            this.Controls.Add(nameLabel);
            this.Controls.Add(nameTextBox);
            this.Controls.Add(quantityLabel);
            this.Controls.Add(quantityTextBox);
            this.Controls.Add(priceLabel);
            this.Controls.Add(priceTextBox);
            this.Controls.Add(categoryLabel);
            this.Controls.Add(categoryTextBox);
            this.Controls.Add(itemsListLabel);
            this.Controls.Add(itemsListBox);
        }

        private void CreateButtons()
        {
            addItemButton = new Button
            {
                Location = new Point(220, 38),
                Text = "Добавить",
                Width = 90
            };
            addItemButton.Click += AddItemButton_Click;

            removeItemButton = new Button
            {
                Location = new Point(315, 38),
                Text = "Удалить",
                Width = 90
            };
            removeItemButton.Click += RemoveItemButton_Click;

            updateQuantityButton = new Button
            {
                Location = new Point(410, 38),
                Text = "Обновить\nколичество",
                Width = 90,
                Height = 40
            };
            updateQuantityButton.Click += UpdateQuantityButton_Click;

            this.Controls.Add(addItemButton);
            this.Controls.Add(removeItemButton);
            this.Controls.Add(updateQuantityButton);
        }

        private void CreateNotificationsSection()
        {
            // Порог уведомлений
            Label thresholdLabel = new Label
            {
                Location = new Point(10, 295),
                Text = "Порог запаса:",
                Width = 90
            };

            thresholdNumericUpDown = new NumericUpDown
            {
                Location = new Point(105, 292),
                Width = 60,
                Minimum = 0,
                Maximum = 100,
                Value = inventoryManager.LowStockThreshold
            };

            setThresholdButton = new Button
            {
                Location = new Point(175, 290),
                Text = "Установить",
                Width = 90
            };
            setThresholdButton.Click += SetThresholdButton_Click;

            showNotificationsButton = new Button
            {
                Location = new Point(275, 290),
                Text = "Обновить уведомления",
                Width = 130
            };
            showNotificationsButton.Click += ShowNotificationsButton_Click;

            clearNotificationsButton = new Button
            {
                Location = new Point(415, 290),
                Text = "Очистить все",
                Width = 100
            };
            clearNotificationsButton.Click += ClearNotificationsButton_Click;

            // Список уведомлений
            Label notificationsLabel = new Label
            {
                Location = new Point(10, 325),
                Text = "Уведомления о низком запасе:",
                Width = 200,
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = Color.DarkRed
            };

            notificationsListBox = new ListBox
            {
                Location = new Point(10, 350),
                Width = 520,
                Height = 180,
                BackColor = Color.LemonChiffon
            };

            lowStockLabel = new Label
            {
                Location = new Point(220, 325),
                Text = "",
                Width = 250,
                ForeColor = Color.Red,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };

            this.Controls.Add(thresholdLabel);
            this.Controls.Add(thresholdNumericUpDown);
            this.Controls.Add(setThresholdButton);
            this.Controls.Add(showNotificationsButton);
            this.Controls.Add(clearNotificationsButton);
            this.Controls.Add(notificationsLabel);
            this.Controls.Add(notificationsListBox);
            this.Controls.Add(lowStockLabel);
        }

        private void UpdateItemsList()
        {
            itemsListBox.Items.Clear();
            foreach (var item in inventoryManager.Items)
            {
                string stockStatus = item.Quantity < inventoryManager.LowStockThreshold ? " | Низкий запас!" : "";
                itemsListBox.Items.Add($"{item.Name} – {item.Quantity} шт | {item.Price} руб | {item.Category}{stockStatus}");
            }
        }

        private void UpdateNotificationsDisplay()
        {
            notificationsListBox.Items.Clear();
            var notifications = inventoryManager.GetNotifications();

            if (notifications.Count == 0)
            {
                notificationsListBox.Items.Add("Нет активных уведомлений о низком запасе");
                lowStockLabel.Text = "";
                lowStockLabel.ForeColor = Color.Green;
            }
            else
            {
                foreach (var notification in notifications)
                {
                    notificationsListBox.Items.Add($"{notification}");
                }
            }
        }

        private void SetThresholdButton_Click(object sender, EventArgs e)
        {
            try
            {
                int newThreshold = (int)thresholdNumericUpDown.Value;
                inventoryManager.SetLowStockThreshold(newThreshold);
                UpdateItemsList();
                UpdateNotificationsDisplay();
                MessageBox.Show($"Порог уведомлений установлен на {newThreshold} шт.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowNotificationsButton_Click(object sender, EventArgs e)
        {
            UpdateNotificationsDisplay();
        }

        private void ClearNotificationsButton_Click(object sender, EventArgs e)
        {
            inventoryManager.ClearNotifications();
            UpdateNotificationsDisplay();
            MessageBox.Show("Все уведомления очищены.", "Уведомления",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddItemButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) ||
                string.IsNullOrEmpty(quantityTextBox.Text) ||
                string.IsNullOrEmpty(priceTextBox.Text) ||
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

            if (quantity < 0)
            {
                MessageBox.Show("Количество не может быть отрицательным!");
                return;
            }

            if (price < 0)
            {
                MessageBox.Show("Цена не может быть отрицательной!");
                return;
            }

            InventoryItem newItem = new InventoryItem(nameTextBox.Text, quantity, price, categoryTextBox.Text);

            try
            {
                inventoryManager.AddItem(newItem);
                nameTextBox.Clear();
                quantityTextBox.Clear();
                priceTextBox.Clear();
                categoryTextBox.Clear();
                UpdateItemsList();
                UpdateNotificationsDisplay();

                if (quantity < inventoryManager.LowStockThreshold)
                {
                    MessageBox.Show($"Товар добавлен, но у него низкий запас! ({quantity} шт < {inventoryManager.LowStockThreshold} шт)",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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
                MessageBox.Show("Пожалуйста, выберите товар для удаления!",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = itemsListBox.SelectedIndex;
            var itemToRemove = inventoryManager.Items[selectedIndex];

            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить товар \"{itemToRemove.Name}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    inventoryManager.RemoveItem(itemToRemove);
                    UpdateItemsList();
                    UpdateNotificationsDisplay();
                    MessageBox.Show("Товар успешно удален!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateQuantityButton_Click(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Пожалуйста, выберите товар для обновления количества!",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(quantityTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите новое количество товара!",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = itemsListBox.SelectedIndex;
            var itemToUpdate = inventoryManager.Items[selectedIndex];

            int newQuantity;
            if (!int.TryParse(quantityTextBox.Text, out newQuantity))
            {
                MessageBox.Show("Неверный формат количества! Пожалуйста, введите целое число.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newQuantity < 0)
            {
                MessageBox.Show("Количество товара не может быть отрицательным! " +
                    "Введите неотрицательное число.",
                    "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Товар: {itemToUpdate.Name}\n" +
                $"Текущее количество: {itemToUpdate.Quantity} шт.\n" +
                $"Новое количество: {newQuantity} шт.\n\n" +
                $"Вы действительно хотите обновить количество?",
                "Подтверждение обновления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    inventoryManager.UpdateItemQuantity(itemToUpdate, newQuantity);
                    UpdateItemsList();
                    UpdateNotificationsDisplay();
                    quantityTextBox.Clear();

                    string message = $"Количество товара \"{itemToUpdate.Name}\" успешно обновлено!\n" +
                        $"Новое количество: {newQuantity} шт.";

                    if (newQuantity < inventoryManager.LowStockThreshold)
                    {
                        message += $"\n\nВнимание: Товар имеет низкий запас!";
                    }

                    MessageBox.Show(message, "Успех", MessageBoxButtons.OK,
                        newQuantity < inventoryManager.LowStockThreshold ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении количества: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}