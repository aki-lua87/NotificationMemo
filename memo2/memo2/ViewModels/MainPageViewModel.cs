using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using memo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace memo2.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private MainPageModel Model { get; }

        private string _title;
        private bool _startup;
        private int _fontsizeindex;
        private IPageDialogService _pageDialogService;

        /// <summary>ViewのContentPage Titleとバインドされています</summary>
        public new string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        /// <summary>ViewのStartUpCheckとバインドされています</summary>
        public bool StartUp
        {
            get { return _startup; }
            set
            {
                // スイッチ変更時にModelに通知し通知状態を変更
                SetProperty(ref _startup, value);
                Model.StartUp = StartUp;
                Model.StartUpTap();
            }
        }
        /// <summary>ViewのFontSizesのPickerとバインドされています</summary>
        public int FontSizeIndex
        {
            get { return _fontsizeindex; }
            set
            {
                // 疑似的にプロパティが変わったときに発生するイベントの実装
                SetProperty(ref _fontsizeindex, value);
                if (this._fontsizeindex == Model.FontSizeIndex) return;
                Model.FontSizeIndex = this._fontsizeindex;
                Model.FontSizeChange();
            }
        }

        // いったんコメントアウト これが実行されない
        // ^> IDependencyService dependencyServiceすればいけるけど解せない
        // https://prismlibrary.com/docs/xamarin-forms/Dependency-Service.html
        public MainPageViewModel(IPageDialogService pageDialogService,INavigationService navigationService, IDependencyService dependencyService)
            : base(navigationService)
        {
            System.Diagnostics.Debug.WriteLine("(´・ω・｀)ViewModel  start");
            ISaveAndLoad sal = dependencyService.Get<ISaveAndLoad>();
            INotificationOnAndOff ins = dependencyService.Get<INotificationOnAndOff>();
            ICreateService ics = dependencyService.Get<ICreateService>();
            Model = new MainPageModel(sal, ins, ics);

            this.Model.PropertyChanged += Model_PropertyChanged;

            _pageDialogService = pageDialogService;

            Title = "通知メモ";

            // _pageDialogService.DisplayAlertAsync("確認", "利用するには他のアプリの上に重ねて表示する権限を許可してください。", "はい");
            //if (!ics.CheckPermission())
            //{
            //    _pageDialogService.DisplayAlertAsync("確認", "利用するには他のアプリの上に重ねて表示する権限を許可してください。", "はい");
            //}
        }

        // これなら実行される
        //    public MainPageViewModel(INavigationService navigationService)
        //: base(navigationService)
        //    {
        //        System.Diagnostics.Debug.WriteLine("(´・ω・｀)ViewModel  start");
        //        //ISaveAndLoad sal = DependencyService.Get<ISaveAndLoad>();
        //        //INotificationOnAndOff ins = DependencyService.Get<INotificationOnAndOff>();
        //        //ICreateService ics = DependencyService.Get<ICreateService>();
        // dependencyService.Get<IPlatformNameProvider>().GetName();

        //    }

        /// <summary>
        /// Modelのプロパティ変更通知を発火トリガとし実行される
        /// </summary>
        /// <remarks>ModelのプロパティをViewModelに代入</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // モデルの変更通知受け取り

        }

        /// <summary>
        /// ControlPageから遷移する際に実行されるメソッド
        /// </summary>
        /// <param name="parameters"></param>
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        /// <summary>
        /// 遷移する際に実行されるメソッド
        /// </summary>
        /// <param name="parameters"></param>
        public void OnNavigatedTo(NavigationParameters parameters)
        {
            // ツールバー用のタイトル変数受け取り
            if (parameters.ContainsKey("title")) Title = (string)parameters["title"];

            // Index初期値とJsonの値が一致するとVが変更されないため意図的に値を変更
            Model.FontSizeIndex = 1;

            // Jsonをロード
            Model.Load();

            StartUp = Model.StartUp;
            FontSizeIndex = Model.FontSizeIndex;
        }

    }
}
