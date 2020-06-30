namespace memo2.Models
{
    /// <summary>
    /// 各プラットフォームごとのJsonデータの保存のためのインターフェイスです。
    /// </summary>
    public interface ISaveAndLoad
    {
        /// <summary>データを保存します</summary>
        /// <param name="filename">保存するJsonファイルの名前です</param>
        /// <param name="text">保存するJsonデータです</param>
        void SaveData(string filename, string text);

        /// <summary>データを読み込みます。</summary>
        /// <param name="filename">読み込むJsonファイルの名前です</param>
        /// <returns>読み込んだJsonデータです</returns>
        string LoadData(string filename);
    }
}
