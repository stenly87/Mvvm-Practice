﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp29
{
    /// <summary>
    /// Логика взаимодействия для EditRecipes.xaml
    /// </summary>
    public partial class EditRecipes : Page, IPage
    {
        public EditRecipes()
        {
            InitializeComponent();
        }

        public void SetVM(IPageVM vm)
        {
            DataContext = vm;
        }
    }
}
