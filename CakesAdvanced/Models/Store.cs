using ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CakesAdvanced.Models
{
    internal class Store
    {
        public string Name { get; }
        private Kitchen _kitchen;
        private Storage _storage;
        public Store(string name)
        {
            Name = name;
            _storage = new Storage();
            _kitchen = new Kitchen(_storage);
            _kitchen.CakeReady += OnCakeReady;
            
        }

        public void OnCakeReady(Cake newCake)
        {
            Console.WriteLine($"Ваш торт {newCake.Name} готов! Вот его цена: {newCake.Price}");
        }

        public void AddIngredients()
        {
             Console.WriteLine("Введите ингредиенты в формате 'Название:Цена:Количество");
             string? input = Console.ReadLine();
             if (string.IsNullOrEmpty(input))
             {
                Console.WriteLine("Вы ничего не ввели");
                Console.ReadKey();
                Console.Clear();
                Open();
                return;
            }
             string[] ingredientsSeparately = input.Split(',');
             
             foreach (string x in ingredientsSeparately)
             {
                 string[] ingredientsDetails = x.Split(':');
                 if (ingredientsDetails.Length != 3)
                 {
                     Console.WriteLine("Введите в соответствии с примером");
                     Console.ReadKey();
                     Console.Clear();
                     AddIngredients();
                     continue;
                 }
                 Ingredient ingredients = new Ingredient()
                 {
                     Name = ingredientsDetails[0],
                     Cost = Convert.ToDecimal(ingredientsDetails[1]),
                     Quantity = Convert.ToInt32(ingredientsDetails[2])
                 };
                 _storage.AddIngredients(ingredients);
             }
        }

        public void ShowManageOptions()
        {
            Dictionary<char, string> options = new Dictionary<char, string>
            {
                { '1', "Добавить ингредиенты" },
                { '2', "Показать ингредиенты" },
                
            };
            char selectedOption = InputService.GetOption("Выберите дествие", options);

            switch(selectedOption)
            {
                case '1':
                    AddIngredients();
                    break;
                case '2':
                    ShowIngredients();
                    break;
                default:
                    Open();
                    return;
            }
            
                
        } 
        public void ShowClientOption()
        {
            Dictionary<char, string> options = new Dictionary<char, string>{
                { '1', "Показать список возможных тортов" },
                { '2', "Заказать торт" }
            };
            char selectedOption = InputService.GetOption("Выберите дествие", options);
            Console.Clear();

            switch (selectedOption)
            {
                case '1':
                    ShowAvailableCakeOptions();
                    break;
                case '2':
                    TakeOrder();
                    break;
                default:
                    Open();
                    return;
            }
        }

        public Dictionary<string, Dictionary<string, int>> ShowAvailableCakeOptions()
        {
            var recipes = _kitchen.GetAvailableRecipes();
            if (recipes.Keys.Count() == 0)
            {
                Console.WriteLine("Доступных рецептов нет");
                Console.ReadKey();
                Console.Clear();
                Open();
            }
            else 
            {
                Console.WriteLine(string.Join(",", recipes.Keys));
            }
                return recipes;
            
        }

        public async void TakeOrder()
        {
            Console.WriteLine("Пожалуйста, напишите название нужного торта");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                throw new Exception("Вы не ввели название");
            }
            Console.WriteLine("Заказ принят, ожидайте");
            _kitchen.MakeCake(cakeName: input);
            
           
        }

        public void Open()
        {
            try
            {
                Console.WriteLine("Здравствуйте!");
                Dictionary<char, string> options = new Dictionary<char, string>{
                { '1', "Менеджер" },
                { '2', "Клиент" }
            };
                char selectedOption = InputService.GetOption("Выберите дествие", options);
                switch (selectedOption)
                {
                    case '1':
                        ShowManageOptions();
                        break;
                    case '2':
                        ShowClientOption();
                        break;
                    default:
                        Open();
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
                Console.Clear();
                Open();
            }
            
            
        }
        public void ShowIngredients()
        {
            var allIngredients=_storage.GetAllIngredients();
            Console.WriteLine("Ингридиенты на складе:");
            Console.WriteLine("Название |Цена     |Количество");
            foreach (var ingredient in allIngredients)
            {
                Console.Write(ingredient.Name.PadRight(9));
                Console.Write("|");
                Console.Write($"{ ingredient.Cost}".PadRight(9));
                Console.Write("|");
                Console.Write(ingredient.Quantity);
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.Clear();
            Open();
        }
       
    }
}
