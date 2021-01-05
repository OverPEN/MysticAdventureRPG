using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class RecipeFactory
    {
        private static readonly List<Recipe> _recipes = new List<Recipe>();

        static RecipeFactory()
        {
            Recipe pane = new Recipe(1, "Pane");
            pane.AddIngredient(5, 1);
            pane.AddIngredient(6, 1);
            pane.AddIngredient(7, 1);

            pane.AddOutputItem(2001, 1);

            _recipes.Add(pane);
        }

        public static Recipe RecipeByID(int id)
        {
            return _recipes.FirstOrDefault(x => x.ID == id);
        }
    }
}
