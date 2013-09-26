namespace AnimateListViewDelete
{
    using System.Collections.Generic;
    using System.Linq;

    using Android.Content;
    using Android.Views;
    using Android.Widget;

    public class MonkeyListAdapter : BaseAdapter<string>
    {
        private readonly Context _context;
        private readonly Dictionary<long, string> _monkeyList;

        public MonkeyListAdapter(Context context, IEnumerable<string> listOfMonkeyNames)
        {
            _context = context;
            _monkeyList = new Dictionary<long, string>();
            InitializeMonkeyList(listOfMonkeyNames);
        }

        public override int Count { get { return _monkeyList.Count; } }

        // We need to have stable Ids for this to work.
        public override bool HasStableIds { get { return true; } }

        public override string this[int index] { get { return _monkeyList.Values.ElementAt(index); } }

        public override long GetItemId(int position)
        {
            var id = _monkeyList.Keys.ElementAt(position);
            return id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView as TextView ?? new TextView(_context);
            view.SetTextAppearance(_context, Android.Resource.Style.TextAppearanceDeviceDefaultLarge);
            view.Text = this[position];
            return view;
        }

        public void Remove(long id)
        {
            if (_monkeyList.ContainsKey(id))
            {
                _monkeyList.Remove(id);
                NotifyDataSetChanged();
            }
        }

        private void InitializeMonkeyList(IEnumerable<string> listOfMonkeyNames)
        {
            var sortedList = from name in listOfMonkeyNames
                orderby name
                select name;
            var monkeyId = 0;
            foreach (var monkey in sortedList)
            {
                _monkeyList.Add(monkeyId++, monkey);
            }
        }
    }
}
