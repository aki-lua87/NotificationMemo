using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace memo2.Droid
{
    /// <summary>
    /// CustomViewItem用のカスタムアダプター
    /// </summary>
    internal class CustomMemoAdapter : BaseAdapter<CustomViewItem>
    {
        public int _fontSize;
        public List<CustomViewItem> items = new List<CustomViewItem>();
        private Context _context;
        private CheckBox _viewItem;

        public CustomMemoAdapter(Context context, List<CustomViewItem> items,int size)
        {
            this._context = context;
            this.items = items;
            this._fontSize = size;
        }
        public override int Count => items.Count;

        public override CustomViewItem this[int position] => items[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            var view = convertView ?? View.Inflate(_context,Resource.Layout.CustomListView, null);

            // BaseAdapter<T>の対応するプロパティを割り当て
            _viewItem = view.FindViewById<CheckBox>(Resource.Id.checkBoxList);

            _viewItem.Checked = item.check;
            _viewItem.Text = item.memo;
            _viewItem.TextSize = _fontSize;

            return view;
            // https://teratail.com/questions/234330
        }
    }
}