using Xamarin.Forms;
using memo2.Droid;
using memo2.Models;
using Android.Content;

[assembly: Dependency(typeof(NotificationOnAndOff_Android))]

namespace memo2.Droid
{
    /// <summary>
    /// Android�p�̒ʒm��ON/OFF���s���N���X: ISaveAndLoad
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
            Context.StopService(_intent); // �T�[�r�X���N�����Ă����ꍇ��x�I��������
            System.Diagnostics.Debug.WriteLine("(�L�E�ցE�M)NotificationOn");
            Context.StartService(_intent);
        }

        [System.Obsolete]
        public void NotificationOff()
        {
            System.Diagnostics.Debug.WriteLine("(�L�E�ցE�M)NotificationOff");
            Context.StopService(_intent);
        }
    }
}