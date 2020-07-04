using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Button = Android.Widget.Button;
using View = Android.Views.View;
using ListView = Android.Widget.ListView;
using Newtonsoft.Json;
using memo2.Models;
using memo2.Views;
using Android.Provider;

namespace memo2.Droid
{
    /// <summary>
    /// 画面に一覧表示画面をオーバーレイするService
    /// </summary>
    [Service]
    public class ListService : Service
    {
        readonly MemoRepository _db = new MemoRepository();

        private IWindowManager WindowManager;
        private Context context;
        private View view;

        ListView listView;
        EditText editText;

        private CustomMemoAdapter customMemoAdapter;
        private List<CustomViewItem> customViewItem = new List<CustomViewItem>();

        // ListViewのポジションとt_memosのidの値を対応させるリスト
        private ArrayList db_id_list = new ArrayList();
        // dbのidとチェックの状態の対応させるテーブル
        private Dictionary<int, bool> item_check_table = new Dictionary<int, bool>() ;

        private bool _isViewPresenceCheck;

        private WindowManagerLayoutParams param;

        private bool FirstOpenCheck;



        Notification.Builder ntfBuilder; // 通知更新のためグローバルに
        readonly int id = 101; // 通知識別子

        public override void OnCreate()
        {
            System.Diagnostics.Debug.WriteLine("(´・ω・｀)List Servise Create");
            base.OnCreate();
            //context = Forms.Context;
            context = Android.App.Application.Context;



            ViewCreate();
            // -----------------------タスクにキルされないようにここで通知を作成
            NtfUpdate();
            FirstOpenCheck = true;

        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // 初回StartCommand起動時のチェック
            if (FirstOpenCheck)
            {
                FirstOpenCheck = false;
                return StartCommandResult.NotSticky;
            }

            // ViewがすでにあるならRemoveする
            if (_isViewPresenceCheck)
            {
                WindowManager.RemoveView(view);
                _isViewPresenceCheck = false;
            }

            System.Diagnostics.Debug.WriteLine("(´・ω・｀)List Servise  start");

            OverrayViewReSet();

            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            System.Diagnostics.Debug.WriteLine("(´・ω・｀)List Servise Destroy");
            base.OnDestroy();

            if (_isViewPresenceCheck)
            {
                WindowManager.RemoveView(view);
            }
            _isViewPresenceCheck = false;
        }

        public override IBinder OnBind(Intent intent) => null;

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            // 一覧画面がすでにある場合に再描写
            if (_isViewPresenceCheck)
            {
                WindowManager.RemoveView(view);
                OverrayViewReSet();
            }
        }

        private async void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // チェックの状態を変化
            customViewItem[e.Position].check = !customViewItem[e.Position].check;
            customMemoAdapter.NotifyDataSetChanged();

            int p = e.Position;
            var deleteItem = _db.GetItems().Single(item => item.id.Equals(db_id_list[p]));
            item_check_table[deleteItem.id] = customViewItem[e.Position].check;

            if (customViewItem[p].check)
            { 
                await DeleteCheck(deleteItem.id);
            }
            NtfUpdate();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var item = new t_memos { memo = editText.Text };
            _db.SaveItem(item);
            editText.Text = "";

            DepictionMemoListInit();

            NtfUpdate();
        }
        private void ToSettingButtoun_Click(object sender, EventArgs e)
        {
            Context Context = Android.App.Application.Context;
            Intent _intent = new Intent(Context, typeof(MainActivity));
            _intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(_intent);

            WindowManager.RemoveView(view);
            _isViewPresenceCheck = false;

            
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            // item_check_table = null;
            WindowManager.RemoveView(view);
            _isViewPresenceCheck = false;
        }

        /// <summary>
        /// t_memosをid降順にソート
        /// </summary>
        /// <returns>t_memosをid降順にソートしたもの</returns>
        private IEnumerable<t_memos> SortMemosItemDescending()
        {
            return from a in _db.GetItems()
                   orderby a.id descending
                   select a;
        }

        /// <summary>
        /// customViewItemを設定
        /// </summary>
        private void DepictionMemoListInit()
        {
            db_id_list.Clear();
            customViewItem.Clear();
            item_check_table.Clear();

            foreach (var db_item in SortMemosItemDescending())
            {
                db_id_list.Add(db_item.id);
                item_check_table.Add(db_item.id, false); // idがインクリメントされていない？
                customViewItem.Add(new CustomViewItem()
                {
                    check = false,
                    memo = db_item.memo
                });
            }
            customMemoAdapter.NotifyDataSetChanged();
        }
        /// <summary>
        /// チェック情報を利用してcustomViewItemを再設定
        /// </summary>
        private void DepictionMemoList()
        {
            db_id_list.Clear();
            customViewItem.Clear();
            foreach (var db_item in SortMemosItemDescending())
            {
                db_id_list.Add(db_item.id);
                customViewItem.Add(new CustomViewItem()
                {
                    check = item_check_table[db_item.id],
                    memo = db_item.memo
                });
            }
            customMemoAdapter.NotifyDataSetChanged();
        }

        /// <summary>
        /// Jsonからフォントサイズの設定を読み込み
        /// </summary>
        /// <returns>フォントサイズをInt型で返す(大:24 中:18 小:12)</returns>
        private int getFontSize()
        {
            return 18; // フォントサイズ変える意味あんまりない
            //const string SaveFileName = "setting.json";
            //SaveAndLoad SaL = new SaveAndLoad();
            //var data = SaL.LoadData(SaveFileName);
            //var strFontSize = JsonConvert.DeserializeObject<SettingModel>(data).Font;
            //switch (strFontSize)
            //{
            //    case "Large":
            //        return 24;
            //    case "Medium":
            //        return 18;
            //    case "Small":
            //        return 12;
            //    default:
            //        return 36; // デバック用 
            //}
            
        }

        /// <summary>
        /// 2.5秒後にDBからIDと一致する項目を削除し再描写する
        /// </summary>
        /// <param name="id">削除したい項目のID</param>
        private async Task DeleteCheck(int id)
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1500);
            });

            // 別スレッドのためNullチェック
            // Issue ： 1.5秒以内にチェックを外し再びチェックすると最初の2.5秒終了時に消えてしまう
            if (item_check_table?[id] ?? false)
            {
                _db.DeleteItem(_db.GetItems().SingleOrDefault(a => a.id == id));
                item_check_table[id] = false;
                DepictionMemoList();
                customMemoAdapter.NotifyDataSetChanged();
            }
        }
        /// <summary>
        /// OverrayするViewの初期設定
        /// </summary>
        public void ViewCreate()
        {
            WindowManager = context.GetSystemService(WindowService).JavaCast<IWindowManager>();
            // Viewからインフレータを作成する
            var layoutInflater = LayoutInflater.From(context);
            // レイアウトファイルから重ね合わせするViewを作成する
            view = layoutInflater.Inflate(Resource.Layout.listlayout, null);

            // 各要素を変数に格納,イベントのあるものはイベント登録
            var closeButton = view.FindViewById<Button>(Resource.Id.closeButton);
            closeButton.Click += CloseButton_Click;

            var toSettingButtoun = view.FindViewById<Button>(Resource.Id.toSettingButton);
            toSettingButtoun.Click += ToSettingButtoun_Click; 

            var addButton = view.FindViewById<Button>(Resource.Id.addButton);
            addButton.Click += AddButton_Click;

            listView = view.FindViewById<ListView>(Resource.Id.list_view);
            listView.ItemClick += ListView_ItemClick;

            editText = view.FindViewById<EditText>(Resource.Id.editText);

            // カスタムViewにアダプターをセット
            customMemoAdapter = new CustomMemoAdapter(this, customViewItem, getFontSize());
            listView.Adapter = customMemoAdapter;

            // DBの値をアダプターにセットし反映
            DepictionMemoListInit();
        }

        /// <summary>
        /// OverrayするViewの描写範囲設定とフォントサイズを設定し描写
        /// </summary>
        public void OverrayViewReSet()
        {
            // カスタムViewにアダプターをセット(フォントの大きさ変更のため)
            customMemoAdapter = new CustomMemoAdapter(this, customViewItem, getFontSize());
            listView.Adapter = customMemoAdapter;

            // 描写位置決定のため画面サイズを取得
            var psize = new Android.Graphics.Point();
            WindowManager.DefaultDisplay.GetSize(psize);

            // 重ね合わせる画面を指定
            param = new WindowManagerLayoutParams(
                (int)(psize.X * 0.8),
                (int)(psize.Y * 0.9),
                WindowManagerTypes.ApplicationOverlay, // タッチ操作ありのため
                WindowManagerFlags.Fullscreen, //フルスクリーン表示
                Android.Graphics.Format.Translucent //半透明
            )
            { Gravity = GravityFlags.Top };

            // Viewを画面上に重ね合わせする
            WindowManager.AddView(view, param);
            // オーバーレイViewの存在フラグを立てる
            _isViewPresenceCheck = true;
        }

        public void NtfUpdate()
        {
            // 権限確認？
            if (!Settings.CanDrawOverlays(this))
            {
                var intent = new Intent(Settings.ActionManageOverlayPermission, Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName));
                StartActivity(intent);
            }

            // 権限が無い場合は何もしない
            if (!Settings.CanDrawOverlays(this))
            {
                // 権限を許可してくれ～的なメッセージ
                return;
            }

            PendingIntent pendingIntent;

            Intent _intent = new Intent(context, typeof(ListService));
            pendingIntent = PendingIntent.GetService(context, 0, _intent, 0); //タッチして遷移するとき用

            // annmawakarannyatu 
            // https://www.it-swarm.dev/ja/android/android-81%E3%81%B8%E3%81%AE%E3%82%A2%E3%83%83%E3%83%97%E3%82%B0%E3%83%AC%E3%83%BC%E3%83%89%E5%BE%8C%E3%81%ABstartforeground%E3%81%8C%E5%A4%B1%E6%95%97%E3%81%99%E3%82%8B/835322830/
            var channelId = CreateNotificationChannel("my_service", "My Background Service");

            ntfBuilder = new Notification.Builder(context, channelId)
                .SetContentTitle($"未完了のタスクが{SortMemosItemDescending().Count()}件あります")
                .SetSmallIcon(Resource.Mipmap.ic_launcher)
                .SetContentText("タップして一覧を表示")
                .SetOngoing(true) //常駐させる
                .SetContentIntent(pendingIntent);


            StartForeground(id, ntfBuilder.Build());
        }


        // コピペ
        private String CreateNotificationChannel(String channelId, String channelName)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return "";
            }
            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.None);
            var service = GetSystemService(Context.NotificationService) as NotificationManager;
            service.CreateNotificationChannel(channel);
            return channelId;
        }
    }
}