using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Threading;
using Microsoft.Phone.Controls;

namespace CountDownTimeTable
{
    public partial class CountDown : PhoneApplicationPage
    {
        public class TimeLine
        {
            private DateTime Time;
            private string Name;

            public TimeLine(string line) { 
                // 区切り文字(カンマ)で分割
                string[] split_line = line.Split(new Char[] { ',', '，' });   // 全角カンマも入れとく

                // 前半の文字列をDateTimeに変換
                string[] split_time = split_line[0].Split(new Char[] { ':', '：' });   // 全角コロンも入れとく
                DateTime tmp = DateTime.Now;
                Time = new DateTime(tmp.Year, tmp.Month, tmp.Day, int.Parse(split_time[0]), int.Parse(split_time[1]), 0);

                // 後半の文字列はそのままセット
                Name = split_line[1];
            }

            public override string ToString()
            {
                //return Time.ToString("T") + " " + Name; // ★Todo:「秒」部分は不要だから消す
                return string.Format("{0,2:D2}:{1,2:D2} {2}", Time.Hour, Time.Minute, Name);
            }
        }

        // Constructor
        public CountDown()
        {
            InitializeComponent();

            // Timer ★場所ここでいい？
            DispatcherTimer tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromSeconds(1);
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Start();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // TimeTableのtxtを読み込んでListBoxに設定
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
            {
                LoadFile(msg);
            }

            listBoxTimeLine.FontSize = 48;

            // 現在時刻とTimeTableから、カウントダウンのTextBlockに数値をセットする
            int SelectedIndex = 0;   // listBoxTimeLineで選択中の行番号
            foreach (object line in listBoxTimeLine.Items)
            {
                DateTime time = GetDateTime(line.ToString());
                DateTime now = DateTime.Now;

                if (time.CompareTo(now) >= 0) {
                    TimeSpan interval = time.Subtract(now);
                    textBlockRemainTime.Text = string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", interval.Hours, interval.Minutes, interval.Seconds);
                    break;
                }

                SelectedIndex++;
            }

            if (SelectedIndex < listBoxTimeLine.Items.Count) {
                // listBoxTimeLineで、対象となる行を選択状態にする
                listBoxTimeLine.SelectedIndex = SelectedIndex;
            }
            else {
                // 先頭行を選択状態にする
                listBoxTimeLine.SelectedIndex = 0;
            }

            // 選択行をScrollViewの先頭にする
            // ★機能してないみたいなので修正する
            listBoxTimeLine.ScrollIntoView(listBoxTimeLine.SelectedItem);
        }

        // TimeLine.ToString()の文字列からDateTimeオブジェクトを取得する
        // 本当はlistBoxTimeLine.ItemsからTimeLineオブジェクトを取得したかったけど出来なかった。。
        private DateTime GetDateTime(string line)
        {
            DateTime tmp = DateTime.Now;
            string[] split_line = line.Split(new Char[] { ':', ' ' });
            DateTime ret = new DateTime(tmp.Year, tmp.Month, tmp.Day, int.Parse(split_line[0]), int.Parse(split_line[1]), 0);

            return ret;
        }

        private void LoadFile(string filename)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            if (isf.FileExists(filename))
            {
                IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, isf);
                using (StreamReader reader = new StreamReader(isfs))
                {
                    string s = "";
                    while ((s = reader.ReadLine()) != null) // ★Todo:改行のみの場合も弾く
                    {
                        //listBoxTimeLine.Items.Add(s);
                        listBoxTimeLine.Items.Add(new TimeLine(s));
                    }
                }
                isfs.Close();
            }
            else
            {
                MessageBox.Show("Cannot find " + filename);
            }

            isf.Dispose();

            return;
        }

        // Timeout Handler
        void tmr_Tick(object sender, EventArgs e)
        {
            // textBlockRemainTime.Text = DateTime.Now.ToLongTimeString();

            // 現在時刻とlistBoxTimeLineの選択行を比較
            DateTime selected = GetDateTime(listBoxTimeLine.SelectedItem.ToString());
            DateTime now = DateTime.Now;
            if (selected.CompareTo(now) >= 0)
            {
                // 残り時間の更新
                TimeSpan interval = selected.Subtract(now);
                textBlockRemainTime.Text = string.Format("{0,2:D2}:{1,2:D2}:{2,2:D2}", interval.Hours, interval.Minutes, interval.Seconds);
            }
            else
            {
                // Todo:下記コードだと、選択行がタイムアウトの度に切り替わる。あとで修正する
                //if (listBoxTimeLine.SelectedIndex + 1 < listBoxTimeLine.Items.Count)
                //{
                //    // 次の行を選択状態にする
                //    listBoxTimeLine.SelectedIndex = listBoxTimeLine.SelectedIndex + 1;
                //}
                //else
                //{
                //    // 先頭行を選択状態にする
                //    listBoxTimeLine.SelectedIndex = 0;
                //}
            }
        }
    }
}