using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace WpfApp29
{
    public static class PageContainer
    {
        static Model model;

        static Dictionary<PageType, Page> pages = 
            new Dictionary<PageType, Page>();

        static Dictionary<PageType, (Type, Type)> pageTypes =
            new Dictionary<PageType, (Type, Type)>();

        public static event EventHandler<PageType> CurrentPageChanged;

        public static void RegisterPageType(PageType type, 
                Type pageType, Type vmType)
        {
            if (pageTypes.ContainsKey(type))
                throw new Exception("Заданный тип страницы уже зарегистрирован");
            pageTypes.Add(type, (pageType, vmType));
        }

        public static Page GetPageByType(PageType type)
        {
            if (pages.ContainsKey(type))
                return pages[type];
            var page = (Page)Activator.CreateInstance(pageTypes[type].Item1);
            pages.Add(type, page);
            var vm = (IPageVM)Activator.CreateInstance(pageTypes[type].Item2);
            vm.SetModel(model);
            ((IPage)page).SetVM(vm);
            return page;
        }

        public static void SetModel(Model model)
        {
            PageContainer.model = model;
        }

        static PageContainer()
        {
            RegisterPageType(PageType.ListItems,
                typeof(ListItems), typeof(ListItemsVM));
            RegisterPageType(PageType.CreateItem,
                typeof(CreateItem), typeof(CreateItemVM));
            RegisterPageType(PageType.EditRecipes,
               typeof(EditRecipes), typeof(EditRecipesVM));
        }

        public static void ChangePageTo(PageType type)
        {
            CurrentPageChanged?.Invoke(null, type);
        }

        
    }
}
