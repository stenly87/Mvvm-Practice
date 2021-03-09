using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WpfApp29
{
    public class RecipeBook
    {
        Dictionary<(string, string), string> recipes = 
            new Dictionary<(string, string), string>();

        internal bool CanJoin(Item firstItem, Item secondItem)
        {
            if (firstItem == secondItem && firstItem.Count < 2)
                return false;
            var key1 = (firstItem.Name, secondItem.Name);
            var key2 = (secondItem.Name, firstItem.Name);
            return recipes.ContainsKey(key1) || recipes.ContainsKey(key2);
        }

        internal Dictionary<(string, string), string> GetRecipes()
        {
            return recipes;
        }

        internal Item Join(Item firstItem, Item secondItem)
        {
            var key1 = (firstItem.Name, secondItem.Name);
            var key2 = (secondItem.Name, firstItem.Name);
            if (recipes.ContainsKey(key1))
                return new Item { Name = recipes[key1] };
            else
                return new Item { Name = recipes[key2] };
        }

        public RecipeBook()
        {
            /*recipes.Add(("Корень имбиря", "Шкура с жопы дракона"), "Спирт");
            recipes.Add(("Спирт", "Спирт"), "Двойной спирт");
            recipes.Add(("Спирт", "Шкура с жопы дракона"), "Гимли");
            recipes.Add(("Спирт", "Гимли"), "Пьяный Гимли");*/
            Load();
        }

        const string db = "recipes.txt";

        void Save()
        {
            using (var fs = File.CreateText(db))
                foreach (var key in recipes.Keys)
                    fs.WriteLine($"{key.Item1}\t{key.Item2}\t=\t{recipes[key]}");
        }

        internal void ChangeRecipes(Dictionary<(string, string), string> newRecipes)
        {
            recipes = newRecipes;
            Save();
        }

        void Load()
        {
            if (!File.Exists(db))
            {
                recipes.Add(("Корень имбиря", "Шкура с жопы дракона"), "Спирт");
                recipes.Add(("Спирт", "Спирт"), "Двойной спирт");
                recipes.Add(("Спирт", "Шкура с жопы дракона"), "Гимли");
                recipes.Add(("Спирт", "Гимли"), "Пьяный Гимли");
                Save();
                return;
            }
            using (var fs = File.OpenText(db))
            {
                var splitter = new char[] { '\t' };
                while (!fs.EndOfStream)
                {
                    var array = fs.ReadLine().Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    recipes.Add((array[0], array[1]), array[3]);
                }
            }
        }
    }
}
