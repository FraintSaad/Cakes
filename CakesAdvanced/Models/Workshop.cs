using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakesAdvanced.Models
{
    internal class Workshop
    {
        Dictionary<string, Dictionary<string, int>> _recipes = new Dictionary<string, Dictionary<string, int>>();

        public Dictionary<string, Dictionary<string, int>> GetAllRecipes() 
        { 
            return _recipes;
        }

        public Workshop()
        {
            Dictionary<string, int> Medovik = new Dictionary<string, int>()
            {
                {"Мед", 300},
                {"Мука", 500},
                {"Сахар", 300},
                {"Масло", 200}
            };
            _recipes.Add("Медовик", Medovik);

            Dictionary<string, int> Napoleon = new Dictionary<string, int>()
            {
                {"Мука", 750},
                {"Масло", 400},
                {"Яйцо", 2},
                {"Сахар", 300}
            };
            _recipes.Add("Наполеон", Napoleon);
        }

        
    }
}
