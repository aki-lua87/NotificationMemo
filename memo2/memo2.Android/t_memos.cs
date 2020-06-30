using SQLite;

namespace memo2.Droid
{
    /// <summary>
    /// 登録したメモのデータを保持するテーブルデータの定義
    /// </summary>
    public class t_memos
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string memo { get; set; }
    }
}
