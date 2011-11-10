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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CountDownTimeTable
{
    public partial class App : Application
    {
        /// <summary>
        /// Phone アプリケーションのルート フレームへの容易なアクセスを提供します。
        /// </summary>
        /// <returns>Phone アプリケーションのルート フレームです。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Application オブジェクトのコンストラクターです。
        /// </summary>
        public App()
        {
            // キャッチできない例外のグローバル ハンドラーです。 
            UnhandledException += Application_UnhandledException;

            // Silverlight の標準初期化
            InitializeComponent();

            // Phone 固有の初期化
            InitializePhoneApplication();

            // デバッグ中にグラフィックスのプロファイル情報を表示します。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 現在のフレーム レート カウンターを表示します。
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 各フレームで再描画されているアプリケーションの領域を表示します。
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // 試験的な分析視覚化モードを有効にします。 
                // これにより、色付きのオーバーレイを使用して、GPU に渡されるページの領域が表示されます。
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // アプリケーションの PhoneApplicationService オブジェクトの UserIdleDetectionMode プロパティを Disabled に設定して、
                // アプリケーションのアイドル状態の検出を無効にします。
                // 注意: これはデバッグ モードのみで使用してください。ユーザーが電話を使用していないときに、ユーザーのアイドル状態の検出を無効にする
                // アプリケーションが引き続き実行され、バッテリ電源が消耗します。
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // (たとえば、[スタート] メニューから) アプリケーションが起動するときに実行されるコード
        // このコードは、アプリケーションが再アクティブ化済みの場合には実行されません
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            // ".txt"ファイルの数を取得する
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            string[] filenames = isf.GetFileNames();
            int numText = 0;
            foreach (string file in filenames)
            {
                if (".txt" == System.IO.Path.GetExtension(file).ToLower())
                {
                    numText++;
                }
            }
            isf.Dispose();

            // ".txt"ファイルの数が0ならデフォルトファイルを作る
            if (numText == 0)
            {
                // Todo:デフォルトの文字列の内容
                //string content = "8:00,東京\n9:00,神奈川\n21:44,大阪\n23:30,New York\n";
                string content = "6:15,普通 東京\n6:26,快速 高崎\n7:05,普通 籠原\n8:05,普通 東京\n9:02,普通 東京\n10:08,快速 東京\n11:03,普通 東京\n12:01,普通 東京\n13:04,普通 東京\n14:03,普通 東京\n15:01,普通 東京\n16:05,特急 東京\n17:07,普通 東京\n18:04,快速 東京\n19:06,普通 東京\n20:04,快速 籠原\n21:04,普通 東京\n22:04,普通 東京\n23:00,快速 籠原\n";
                SaveFile("example.txt", content);
            }

            // ListBoxの表示をクリアする
            // Todo:各ページのコントロールにはどうやってアクセスする？
            // MainPageクラスがpublicだから、どこかに宣言を書けば使えると思うけど。。
//          listBoxTimeTable.Items.Clear();
        }

        // Todo:重複してるからまとめたい
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

        // アプリケーションがアクティブになった (前面に表示された) ときに実行されるコード
        // このコードは、アプリケーションの初回起動時には実行されません
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // アプリケーションが非アクティブになった (バックグラウンドに送信された) ときに実行されるコード
        // このコードは、アプリケーションの終了時には実行されません
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // (たとえば、ユーザーが戻るボタンを押して) アプリケーションが終了するときに実行されるコード
        // このコードは、アプリケーションが非アクティブになっているときには実行されません
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // ナビゲーションに失敗した場合に実行されるコード
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // ナビゲーションに失敗しました。デバッガーで中断します。
                System.Diagnostics.Debugger.Break();
            }
        }

        // ハンドルされない例外の発生時に実行されるコード
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // ハンドルされない例外が発生しました。デバッガーで中断します。
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone アプリケーションの初期化

        // 初期化の重複を回避します
        private bool phoneApplicationInitialized = false;

        // このメソッドに新たなコードを追加しないでください
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // フレームを作成しますが、まだ RootVisual に設定しないでください。これによって、アプリケーションがレンダリングできる状態になるまで、
            // スプラッシュ スクリーンをアクティブなままにすることができます。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // ナビゲーション エラーを処理します
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 再初期化しないようにします
            phoneApplicationInitialized = true;
        }

        // このメソッドに新たなコードを追加しないでください
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // ルート visual を設定してアプリケーションをレンダリングできるようにします
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // このハンドラーは必要なくなったため、削除します
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}