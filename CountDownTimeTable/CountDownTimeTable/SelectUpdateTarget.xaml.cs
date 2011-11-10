using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace CountDownTimeTable
{
    public partial class SelectUpdateTarget : PhoneApplicationPage
    {
        public SelectUpdateTarget()
        {
            InitializeComponent();
            // ## need to be modified ##
            listBoxTimeTable.FontSize = 48;
            listBoxTimeTable.Items.Add("example.txt");
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            string[] filenames = isf.GetFileNames();

            listBoxTimeTable.FontSize = 48;
            listBoxTimeTable.Items.Clear();
            foreach (string file in filenames)
            {
                listBoxTimeTable.Items.Add(file);
            }

            isf.Dispose();
        }

        // -----------------------------------------------
        // Listbox event handler
        // -----------------------------------------------
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        // Todo:SelectionChangedイベント以外にいいのない？ListItemSelected的な
        private void listBoxTimeTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ## need to be modified ##
            String select = (String)listBoxTimeTable.SelectedItem;
            //MessageBox.Show(select, select, MessageBoxButton.OK);
            NavigationService.Navigate(new Uri("/UpdateTimeTable.xaml?msg=" + select, UriKind.Relative));

            // Listboxを非選択状態に設定
            listBoxTimeTable.SelectedIndex = -1;
        }
    }
}