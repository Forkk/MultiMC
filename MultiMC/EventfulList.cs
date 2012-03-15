using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC
{
	public class EventfulList<T> : List<T>, IList<T>
	{
		protected virtual void OnItemAdded(T item, int index)
		{
			if (Added != null)
				Added(this, new ItemAddRemoveEventArgs<T>(item, index));
		}

		protected virtual void OnItemRemoved(T item, int index)
		{
			if (Removed != null)
				Removed(this, new ItemAddRemoveEventArgs<T>(item, index));
		}

		protected virtual void OnItemChanged(T newValue, T oldValue, int index)
		{
			if (Changed != null)
				Changed(this, new ItemChangedEventArgs<T>(newValue, oldValue, index));
		}


		public event EventHandler<ItemChangedEventArgs<T>> Changed;


		public event EventHandler<ItemAddRemoveEventArgs<T>> Added;


		public event EventHandler<ItemAddRemoveEventArgs<T>> Removed;

		public new void Insert(int index, T item)
		{
			base.Insert(index, item);
			OnItemAdded(item, index);
		}

		public new void RemoveAt(int index)
		{
			T item = this[index];
			base.RemoveAt(index);
			OnItemRemoved(item, index);
		}

		public new T this[int index]
		{
			get { return base[index]; }
			set
			{
				T oldValue = base[index];
				base[index] = value;
				OnItemChanged(value, oldValue, index);
			}
		}

		public new void Add(T item)
		{
			base.Add(item);
			OnItemAdded(item, base.Count - 1);
		}

		public new void Clear()
		{
			foreach (T item in this)
			{
				Remove(item);
			}
		}

		public new bool Remove(T item)
		{
			int index = IndexOf(item);
			bool val = base.Remove(item);
			OnItemRemoved(item, index);
			return val;
		}
	}

	#region Event args

	public class ItemAddRemoveEventArgs<T> : EventArgs
	{
		public ItemAddRemoveEventArgs(T item, int index)
		{
			this.Item = item;
			this.Index = index;
		}

		public T Item
		{
			get;
			protected set;
		}

		public int Index
		{
			get;
			protected set;
		}
	}

	public class ItemChangedEventArgs<T> : EventArgs
	{
		public ItemChangedEventArgs(T newValue, T oldValue, int index)
		{
			this.NewValue = newValue;
			this.OldValue = oldValue;
			this.Index = index;
		}

		public T NewValue
		{
			get;
			protected set;
		}

		public T OldValue
		{
			get;
			protected set;
		}

		public int Index
		{
			get;
			protected set;
		}
	}

	#endregion
}
