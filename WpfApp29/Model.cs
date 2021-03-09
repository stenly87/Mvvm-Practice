using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp29
{
    public class Model
    {
        Inventory inventory = new Inventory();
        RecipeBook recipeBook = new RecipeBook();

        public event EventHandler ItemsChanged; 
        public event EventHandler<string> JoinMessage;

        internal ObservableCollection<Item> GetItems()
        {
            return new ObservableCollection<Item>(inventory.Items);
        }

        internal Dictionary<(string, string), string> GetRecipes()
        {
            return recipeBook.GetRecipes();
        }

        internal bool TryJoin(Item firstItem, Item secondItem)
        {
            if (recipeBook.CanJoin(firstItem, secondItem))
            {
                inventory.DecrementItem(firstItem);
                inventory.DecrementItem(secondItem);
                Item newItem = recipeBook.Join(firstItem, secondItem);
                AddItem(newItem);
                JoinMessage?.Invoke(this, $"Успешно создан предмет {newItem.Name}");
                return true;
            }
            JoinMessage?.Invoke(this, $"Невозможно объединить предметы: {firstItem.Name} и {secondItem.Name}");
            return false;
        }

        internal void AddItem(Item item)
        {
            inventory.IncrementItem(item);
            ItemsChanged?.Invoke(this, null);
        }

        internal void SaveRecipes(Dictionary<(string, string), string> newRecipes)
        {
            recipeBook.ChangeRecipes(newRecipes);
        }
    }
}