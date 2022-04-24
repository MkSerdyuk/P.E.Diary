﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P.E.Diary.Widgets
{
    /// <summary>
    /// Interaction logic for NormativesLeftMenu.xaml
    /// </summary>
    public partial class NormativesLeftMenu : UserControl
    {
        public NormativesList NormativesList;
        public NormativesLeftMenu()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewNormativeDialog newNormativeDialog = new NewNormativeDialog(NormativesList);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            NormativesList.DeleteNode();
        }
         
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (NormativesList.MainList.SelectedItem != null)
            {
                if (((TreeViewItem)NormativesList.MainList.SelectedItem).Parent != null) //если родителю есть, то это категория
                {
                    EditTypeDialog editTypeDialog = new EditTypeDialog(NormativesList, NormativesList.GetSelectedNodeText());
                }
                else //если родителя нет, то это норматив
                {
                    EditNormativeDialog editNormativeDialog = new EditNormativeDialog(NormativesList, NormativesList.GetActiveNormative());
                }
            }
        }
    }
}