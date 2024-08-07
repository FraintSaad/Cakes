using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CakesLibrary.Models;
using Microsoft.VisualBasic;

namespace CakesWpf
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Ingredient> Ingredients { get; set; } = new ObservableCollection<Ingredient>();
        public ObservableCollection<string> Recipes { get; set; } = new ObservableCollection<string>();
        private Storage _storage = new Storage();
        private Kitchen _kitchen;
        private string _selectedCakeName;
        private List<Ingredient> _selectedIngredients;
        
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            _storage = new Storage();
            _kitchen = new Kitchen(_storage);
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            UpdateIngredientsView();
            UpdateRecipesView();
        }
        private void BtnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            var ingredient = new Ingredient();
            ingredient.Name = txtName.Text;
            ingredient.Cost = Convert.ToDecimal(txtCost.Text);
            ingredient.Quantity = Convert.ToInt32(txtQuantity.Text);
            _storage.AddIngredients(ingredient);
            UpdateIngredientsView();

        }

        private async void btnTakeOrder(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedCakeName))
            {
                MessageBox.Show("Вы не выбрали торт");
            }
            else if (!Recipes.Contains(_selectedCakeName))
            {
                MessageBox.Show("Такого нет, пожалуйста выберите из списка");
            }
            else
            {
                MessageBox.Show($"Заказ принят: {_selectedCakeName}. Ждите изготовления.");
                try
                {
                    var cake = await _kitchen.MakeCake(_selectedCakeName);
                    MessageBox.Show($"Торт {_selectedCakeName} изготовлен. Цена: {cake.Price:C}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                UpdateRecipesView();
                _selectedCakeName = "";
            }
        }
        private void UpdateRecipesView()
        {
            Recipes.Clear();
            var avaibleRecipes = _kitchen.GetAvailableRecipes().Keys;

            foreach (var recipe in avaibleRecipes)
            {
                Recipes.Add(recipe);
            }
        }
        private void UpdateIngredientsView()
        {
            Ingredients.Clear();
            var availableIngredients = _storage.GetAllIngredients();
            foreach (var ingredient in availableIngredients)
            {
                Ingredients.Add(ingredient);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            _selectedCakeName = e.AddedItems[0]!.ToString()!;
        }

        private void lstIngredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            _selectedIngredients = new List<Ingredient>();
            foreach (var item in e.AddedItems)
            {
                var ingredient = item as Ingredient;
                _selectedIngredients.Add(ingredient);
            }
        }

        private void selection_changed_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateRecipesView();
            UpdateIngredientsView();
        }


        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (TabChanger.SelectedItem == tabClient)
                {
                    TabChanger.SelectedItem = tabManager;
                }
                else if (TabChanger.SelectedItem == tabManager)
                {
                    TabChanger.SelectedItem = tabClient;
                }
                e.Handled = true;
                UpdateIngredientsView();
                UpdateRecipesView();
            }
        }

    }
}