using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;

namespace memo2.Models
{
    /// <summary>
    /// 設定情報をプロパティとして保持するクラスです。
    /// </summary>
    public class SettingModel : BindableBase
    {
        private bool _startup;
        private string _font;

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public SettingModel()
        {
            // 初回起動出ない場合は何もしない
        }
        
        /// <summary>
        /// 初期設定値が必要な際のコンストラクタです。
        /// </summary>
        /// <param name="startup"></param>
        /// <param name="fontsize"></param>
        public SettingModel(bool startup,string fontsize)
        {
            StartUp = startup;
            //Font = fontsize;
        }

        /// <summary>起動状態をBool型で保持</summary>
        public bool StartUp
        {
            get { return _startup; }
            set { SetProperty(ref _startup, value); }
        }

        /// <summary>フォントサイズの値をStringで保持</summary>
        //public string Font
        //{
        //    get { return _font; }
        //    set { SetProperty(ref _font, value); }
        //}
    }
}
