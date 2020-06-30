using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Newtonsoft.Json;
using System.Diagnostics;

namespace memo2.Models
{
    /// <summary>
    /// ControlPageのModelとなるクラスです
    /// </summary>
    class MainPageModel : BindableBase
    {
        private ISaveAndLoad SaveAndLoad { get; }
        private ICreateService CreateService { get; }
        private INotificationOnAndOff NotificationService { get; }
        private SettingModel _settingModel = new SettingModel();

        private bool _startup;
        private int _fontsizeindex;

        /// <summary>
        /// StartUpの値をBool値で保持
        /// </summary>
        public bool StartUp
        {
            get { return _startup; }
            set { SetProperty(ref _startup, value); }
        }

        /// <summary>
        /// FontSizeをPickerのIndexの値でint型で保持
        /// </summary>
        public int FontSizeIndex
        {
            get { return _fontsizeindex; }
            set { SetProperty(ref _fontsizeindex, value); }
        }

        private const string SaveFileName = "setting.json";

        /// <summary>
        /// コンストラクタです。VMからInterfaceを受け取りModelで使用可能にします。
        /// </summary>
        /// <param name="sal">ISaveAndLoad　インターフェイス型</param>
        /// <param name="ins">INotificationOnAndOff　インターフェイス型</param>
        /// <param name="ics">ICreateService インターフェイス型</param>
        public MainPageModel(ISaveAndLoad sal,INotificationOnAndOff ins,ICreateService ics)
        {
            SaveAndLoad = sal;
            NotificationService = ins;
            CreateService = ics;
        }

        /// <summary>
        /// <see cref="_settingModel"/>の設定情報をJson変換し保存します。
        /// </summary>
        public void Save()
        {
            var data = JsonConvert.SerializeObject(_settingModel);
            Debug.WriteLine("OutputJsonData = "+ data);
            SaveAndLoad.SaveData(SaveFileName, data);
        }
        /// <summary>
        /// Json設定情報を<see cref="_settingModel"/>に読み込みます。その後通知状態を変化させます。
        /// </summary>
        public void Load()
        {
            Debug.WriteLine("Jsonロード(´・ω・｀)");
            try
            {
                var data = SaveAndLoad.LoadData(SaveFileName);
                Debug.WriteLine("InputJsonData = " + data);
                _settingModel = JsonConvert.DeserializeObject<SettingModel>(data);
                this.StartUp = _settingModel.StartUp;
                //this.FontSizeIndex = _intToSize.First(x => string.Equals(x.Value, _settingModel.Font, StringComparison.CurrentCultureIgnoreCase)).Key;
            }
            catch 
            {
                Debug.WriteLine("Json作るよー(´・ω・｀)");
                var data = JsonConvert.SerializeObject(new SettingModel(false, "Medium"));
                SaveAndLoad.SaveData(SaveFileName, data);
                Load();
            }

            // StartUpの値によって通知を変化
            if (StartUp)
            {
                SettingOn();
            }
            else
            {
                SettingOff();
            }
        }

        /// <summary>
        /// StartUpの設定値を変更し<see cref="_settingModel"/>に反映しJson形式で保存します。
        /// </summary>
        public void StartUpTap()
        {
            if (StartUp)
            {
                SettingOn();
            }
            else
            {
                SettingOff();
            }
            Debug.WriteLine("Model.StartUp => " + this.StartUp);
            _settingModel.StartUp = this.StartUp;
            Save();
        }

        /// <summary>
        /// 変更されたFontSizeの設定値を<see cref="_settingModel"/>に適用しJson形式で保存します。
        /// </summary>
        /// <remarks>
        /// PickerがCommandとの関連漬けが出来ないため呼び出し元で<see cref="FontSizeIndex"/>への代入後このメソッドを呼び出してください。
        /// </remarks>
        public void FontSizeChange()
        {
            //_settingModel.Font = _intToSize[this.FontSizeIndex];
            Save();
        }

        public void SettingOn()
        {
            NotificationService.NotificationOn();
        }
        public void SettingOff()
        {
            NotificationService.NotificationOff();
            CreateService.ServiceOff();
        }

        /// <summary>
        /// フォントサイズの一覧Dictionary
        /// </summary>
        //private readonly Dictionary<int, string> _intToSize = new Dictionary<int, string>
        //{
        //    { 0, "Small" }, { 1, "Medium" },{ 2,"Large" }
        //};

    }
}
