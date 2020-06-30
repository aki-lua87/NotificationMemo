using System.Collections.Generic;
using SQLite;

namespace memo2.Droid
{
    class MemoRepository
    {
        private static readonly object Locker = new object();
        private readonly SQLiteConnection _db;

        public MemoRepository()
        {
            var SQLite = new SQLite_Android();
            _db = SQLite.GetConnection();
            _db.CreateTable<t_memos>();
        }

        public IEnumerable<t_memos> GetItems()
        {
            lock (Locker)
            {
                return _db.Table<t_memos>();
            }
        }

        public int SaveItem(t_memos item)
        {
            lock (Locker)
            {
                if (item.id != 0)
                {
                    _db.Update(item);
                    return item.id;
                }
                return _db.Insert(item);
            }
        }

        public int DeleteItem(t_memos item)
        {
            lock (Locker)
            {
                _db.Delete(item);
                return item.id;
            }
        }
    }
}