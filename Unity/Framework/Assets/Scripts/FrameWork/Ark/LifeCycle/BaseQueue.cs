using System.Collections.Generic;

namespace Ark.LifeCycle
{
	public class BaseQueue<TData>
	{
		protected List<TData> _bookList = null;

		protected BaseQueue()
		{
		}

		public void ListInit()
		{
			_bookList = new List<TData>();
		}

		public void ListClear()
		{
			if (_bookList.Count > 0)
			{
				_bookList.Clear();
			}
		}

		public void Book(TData obj)
		{
			_bookList.Add(obj);
		}

		public TData Get(int i)
		{
			return _bookList[i];
		}

		public int GetBookLength()
		{
			return _bookList.Count;
		}

		public void ListRelease()
		{
			_bookList.Clear();
			_bookList = null;
		}
	}
}