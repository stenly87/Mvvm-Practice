using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfApp29
{
    public class CreateItemVM : Mvvm1125.MvvmNotify, IPageVM
    {
        Model model;
        public List<BaseItem> BaseItems { get; set; }
        public ObservableCollection<Item> Items { get; set; }
        public Item FirstItem { get; set; }
        public Item SecondItem { get; set; }
        public Mvvm1125.MvvmCommand CreateItem { get; set; }
        public Mvvm1125.MvvmCommand OpenRecipes { get; set; }

        public void SetModel(Model model)
        {
            this.model = model;            
            Items = model.GetItems();
            CreateItem = new Mvvm1125.MvvmCommand(
                () => {
                    if (model.TryJoin(FirstItem, SecondItem))
                        PageContainer.ChangePageTo(PageType.ListItems);
                },
                () => FirstItem != null && SecondItem != null);
            BaseItems = new List<BaseItem>();
            BaseItems.Add(new BaseItem { 
                Name = "Корень имбиря", 
                CreateBaseItem = new Mvvm1125.MvvmCommand(
                    ()=>{ model.AddItem(new Item { Name = "Корень имбиря" }); },
                    ()=>true)
            });
            BaseItems.Add(new BaseItem
            {
                Name = "Шкура с жопы дракона",
                CreateBaseItem = new Mvvm1125.MvvmCommand(
                    () => { model.AddItem(new Item { Name = "Шкура с жопы дракона" }); },
                    () => true)
            });
            model.ItemsChanged += Model_ItemsChanged;
            OpenRecipes = new Mvvm1125.MvvmCommand(
                ()=> {
                    PageContainer.ChangePageTo(PageType.EditRecipes);
                },
                ()=>true);
        }

        private void Model_ItemsChanged(object sender, EventArgs e)
        {
            Items = model.GetItems();
            NotifyPropertyChanged("Items");
        }

        public class BaseItem
        {
            public string Name { get; set; }
            public Mvvm1125.MvvmCommand CreateBaseItem { get; set; }
        }
    }
}
