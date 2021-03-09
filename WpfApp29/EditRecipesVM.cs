using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

namespace WpfApp29
{
    public class EditRecipesVM : Mvvm1125.MvvmNotify, IPageVM
    {
        public Recipe FirstItem
        {
            get => firstItem;
            set { firstItem = value; NotifyPropertyChanged(); }
        }
        public Recipe SecondItem
        {
            get => secondItem;
            set { secondItem = value; NotifyPropertyChanged(); }
        }
        public Recipe SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
                FirstItem = Recipes.FirstOrDefault(s => s.ResultItem == selectedRecipe.FirstItem);
                SecondItem = Recipes.FirstOrDefault(s => s.ResultItem == selectedRecipe.SecondItem);
                ResultItem = selectedRecipe.ResultItem;
            }
        }

        public string ResultItem
        {
            get => resultItem;
            set
            {
                resultItem = value;
                NotifyPropertyChanged();
            }
        }

        public Mvvm1125.MvvmCommand SaveRecipe { get; set; }
        public Mvvm1125.MvvmCommand DeleteRecipe { get; set; }
        public Mvvm1125.MvvmCommand NewRecipe { get; set; }

        public ObservableCollection<Recipe> Recipes { get; set; }

        Model model;
        private string resultItem;
        private Recipe selectedRecipe;
        private Recipe firstItem;
        private Recipe secondItem;

        public void SetModel(Model model)
        {
            this.model = model;
            Recipes = CreateWrapper(model.GetRecipes());
            SaveRecipe = new Mvvm1125.MvvmCommand(
                () => {                    
                    SelectedRecipe.FirstItem = FirstItem?.ResultItem;
                    SelectedRecipe.SecondItem = SecondItem?.ResultItem;
                    SelectedRecipe.ResultItem = ResultItem;
                    SaveRecipes();
                },
                () => SecondItem != null && !string.IsNullOrEmpty(ResultItem)
                );
            DeleteRecipe = new Mvvm1125.MvvmCommand(
               () => { 
                   Recipes.Remove(SelectedRecipe);
                   SaveRecipes();
               },
               () => SelectedRecipe != null
               );
            NewRecipe = new Mvvm1125.MvvmCommand(
                () => {
                    var recipe = new Recipe();
                    recipe.FirstItem = FirstItem?.ResultItem;
                    recipe.SecondItem = SecondItem?.ResultItem;
                    recipe.ResultItem = ResultItem;
                    Recipes.Add(recipe);
                    SaveRecipes();
                    SelectedRecipe = recipe;
                },
                () => FirstItem != null && SecondItem != null && !string.IsNullOrEmpty(ResultItem)); ; ;
        }

        private void SaveRecipes()
        {
            Dictionary<(string, string), string> newRecipes = new Dictionary<(string, string), string>();
            foreach (var recipe in Recipes)
                newRecipes.Add((recipe.FirstItem, recipe.SecondItem), recipe.ResultItem );
            model.SaveRecipes(newRecipes);
        }

        private ObservableCollection<Recipe> CreateWrapper(Dictionary<(string, string), string> recipes)
        {
            ObservableCollection<Recipe> result = new ObservableCollection<Recipe>();
            foreach (var key in recipes.Keys)
                result.Add(new Recipe { FirstItem = key.Item1, SecondItem = key.Item2, ResultItem = recipes[key] });
            return result;
        }

        public class Recipe : Mvvm1125.MvvmNotify
        {
            private string firstItem;
            private string secondItem;
            private string resultItem;

            public string FirstItem
            {
                get => firstItem;
                set { firstItem = value; NotifyPropertyChanged(); }
            }
            public string SecondItem
            {
                get => secondItem;
                set { secondItem = value; NotifyPropertyChanged(); }
            }
            public string ResultItem 
            { 
                get => resultItem; 
                set { resultItem = value; NotifyPropertyChanged(); }
            }
        }
    }
}
