namespace memo2.Models
{
    /// <summary>
    /// 各プラットフォームごとの通知の変更のためのインターフェイスです。
    /// </summary>
    public interface INotificationOnAndOff
    {
        /// <summary>通知を出します</summary>
        void NotificationOn();
        /// <summary>通知を消します</summary>
        void NotificationOff();
    }
}
