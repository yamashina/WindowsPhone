using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Reflection;
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
    public partial class MainPage : PhoneApplicationPage
    {
        // コンストラクター
        public MainPage()
        {
            InitializeComponent();
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
        // Application Bar event handler
        // -----------------------------------------------
        private void buttonCreate_Click(object sender, System.EventArgs e)
        {
        	// TODO: ここにイベント ハンドラーのコードを追加します。
            NavigationService.Navigate(new Uri("/CreateTimeTable.xaml", UriKind.Relative));
        }

        private void buttonUpdate_Click(object sender, System.EventArgs e)
        {
        	// TODO: ここにイベント ハンドラーのコードを追加します。
            NavigationService.Navigate(new Uri("/SelectUpdateTarget.xaml", UriKind.Relative));
        }

        private void buttonDelete_Click(object sender, System.EventArgs e)
        {
        	// TODO: ここにイベント ハンドラーのコードを追加します。
            NavigationService.Navigate(new Uri("/SelectDeleteTarget.xaml", UriKind.Relative));
        }

        private void menuItemHowToUse_Click(object sender, System.EventArgs e)
        {
        	// TODO: ここにイベント ハンドラーのコードを追加します。
            NavigationService.Navigate(new Uri("/HowToUse.xaml", UriKind.Relative));
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
        	// TODO: ここにイベント ハンドラーのコードを追加します。
            AssemblyName an = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            string ApplicationName = an.Name;

            string s = ApplicationName + Environment.NewLine;
            s += "Version " + an.Version.ToString() + Environment.NewLine;

            //object[] company = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            //s += ((AssemblyCompanyAttribute)company[0]).Company + Environment.NewLine;

            //object[] copyright = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            //s += ((AssemblyCopyrightAttribute)copyright[0]).Copyright.ToString();

            s += "http://d.hatena.ne.jp/yamashina/" + Environment.NewLine;

            MessageBox.Show(s);
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
            NavigationService.Navigate(new Uri("/CountDown.xaml?msg=" + select, UriKind.Relative));

            // Listboxを非選択状態に設定
            listBoxTimeTable.SelectedIndex = -1;
        }
    }
}