using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace CakesLibrary.Models
{
    public class Storage
    {
        const string PATH = "ingredients.json";
        private List<Ingredient> _allIngredients = new List<Ingredient>();

        public void SaveIngredients()
        {
            var serializedIngredients = JsonConvert.SerializeObject(_allIngredients);
            File.WriteAllText(PATH, serializedIngredients);
        }
        public void LoadIngredients()
        {
            if(File.Exists(PATH))
            {
                var serializedIngredients = File.ReadAllText(PATH);
                _allIngredients = JsonConvert.DeserializeObject<List<Ingredient>>(serializedIngredients);
                return;
            } 
        }
        public Storage()
        {
            LoadIngredients();
        }
        public Ingredient? FindIngredientByName(string nameOfIngredient)
        {
            return _allIngredients.Find(i => i.Name.ToLower() == nameOfIngredient.ToLower());
        }
        public Ingredient GetIngredientByName(string nameOfIngredient)
        {
            return _allIngredients.First(someWord =>someWord.Name.ToLower() == nameOfIngredient.ToLower());
        }
        public void AddIngredients(Ingredient ingredient)
        {
            var existingIngredient = FindIngredientByName(ingredient.Name);
            if(existingIngredient != null)
            {
                existingIngredient.Quantity += ingredient.Quantity;
            }
            else
            {
                _allIngredients.Add(ingredient);
            }
            SaveIngredients();
        }
        public void AddIngredient(List<Ingredient> ingredients)
        {
            foreach(var ingredient in ingredients)
            {
                AddIngredients(ingredient);
            }
        }
        public void VerifyIngredientsAvailability(Dictionary<string, int> neededIngredients)
        {
            
            foreach(var i in neededIngredients)
            {
                string ingredientName= i.Key;
                int neededQuantity= i.Value;
                Ingredient existingIngredient = FindIngredientByName(ingredientName);

                if(existingIngredient == null)
                {
                    throw new Exception("Ингредиент отсутствует на складе");
                }
                if(existingIngredient.Quantity<neededQuantity)
                {
                    throw new Exception("недостаточное количество ингредиента на складе");
                }
            }
        }
        public List<Ingredient> TakeIngredients(Dictionary<string, int> neededIngredients)
        {
           VerifyIngredientsAvailability(neededIngredients);
            List<Ingredient> ingredientsToReturn = new List<Ingredient>();
            foreach (var i in neededIngredients)
            {
                string ingredientName= i.Key;
                int neededQuantity= i.Value;
                Ingredient existingIngredient = GetIngredientByName(ingredientName);

                existingIngredient.Quantity -= neededQuantity;

                Ingredient takenIngredient = new Ingredient
                {
                    Name = existingIngredient.Name,
                    Cost = existingIngredient.Cost,
                    Quantity = neededQuantity,
                };
                ingredientsToReturn.Add(takenIngredient);
            }
            SaveIngredients();
            return ingredientsToReturn;
        }
        public List<Ingredient> GetAllIngredients()
        {
            LoadIngredients();
            return _allIngredients;
        }
    }
}
