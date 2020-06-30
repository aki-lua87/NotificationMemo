using Xamarin.Forms;
using memo2.Droid;
using memo2.Models;
using Android.Content;

[assembly: Dependency(typeof(NotificationOnAndOff_Android))]

namespace memo2.Droid
{
    /// <summary>
    /// Android用の通知のON/OFFを行うクラス: ISaveAndLoad
    /// </summary>
    public sealed class NotificationOnAndOff_Android : INotificationOnAndOff
    {
        [System.Obsolete]
        private static readonly Context Context = Forms.Context;
        [System.Obsolete]
        private readonly Intent _intent = new Intent(Context, typeof(ListService));

        [System.Obsolete]
        public void NotificationOn()
        {
            Context.StopService(_intent); // サービスが起動していた場合一度終了させる
            System.Diagnostics.Debug.WriteLine("(´・ω・｀)NotificationOn");
            Context.StartService(_intent);
        }

        [System.Obsolete]
        public void NotificationOff()
        {
            System.Diagnostics.Debug.WriteLine("(´・ω・｀)NotificationOff");
            Context.StopService(_intent);
        }
    }
}