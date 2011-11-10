using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class CreateTimeTable : PhoneApplicationPage
    {
        public CreateTimeTable()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            string filename = textBoxFilename.Text;
            if (filename != "")
            {
                if (".txt" != System.IO.Path.GetExtension(filename).ToLower())
                {
                    filename += ".txt";
                }

                if (CheckTextFormat(textBoxContent.Text)) {
                    SaveFile(filename, textBoxContent.Text);
                    NavigationService.GoBack();
                }
                else {
                    MessageBox.Show("「時(24時表記):分,イベント名」の形式で入力してください");
                }
            }
            else
            {
                MessageBox.Show("ファイル名を入力してください");
            }
        }

        // Todo:重複してるからまとめたい
        // App.xaml.csにまとめる？
        private void SaveFile(string filename, string content)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write, isf);

            using (StreamWriter writer = new StreamWriter(isfs))
            {
                writer.Write(content);
            }
            isfs.Close();
            isf.Dispose();
        }

        private bool CheckTextFormat(string str)
        {
            bool ret = true;

            // 区切り文字(コロン、カンマ)で分割
            string[] split = str.Split(new Char[] { ',', '，', ':', '：', '\r', '\n' });   // 全角もOKにする

            for (int i = 0; i < split.Length/3; i++) {
                try
                {
                    int.Parse(split[i * 3]);
                    int.Parse(split[i * 3 + 1]);
                }
                catch
                {
                    ret = false;
                }
            }

            return ret;
        }
    }
}