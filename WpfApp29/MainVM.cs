using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace WpfApp29
{
    public class MainVM : Mvvm1125.MvvmNotify
    {
        Model model;
        private string currentMessage;

        public Page CurrentPage { get; set; }
        public string CurrentMessage
        {
            get => currentMessage;
            private set
            {
                currentMessage = value;
                NotifyPropertyChanged();
            }
        }

        public MainVM()
        {
            model = new Model();
            model.JoinMessage += (o, e) => CurrentMessage = e;
            PageContainer.SetModel(model);
            CurrentPage = PageContainer.GetPageByType(PageType.ListItems);
            PageContainer.CurrentPageChanged += PageContainer_CurrentPageChanged;
        }

        void PageContainer_CurrentPageChanged(object sender, PageType e)
        {
            CurrentPage = PageContainer.GetPageByType(e);
            NotifyPropertyChanged("CurrentPage");
        }

    }
}
