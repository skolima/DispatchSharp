using System;

namespace DispatchSharp.QueueTypes
{
	/// <summary>
	/// A default work queue item wrapper
	/// </summary>
	/// <typeparam name="T">Type of the underlying queue item</typeparam>
	public class WorkQueueItem<T>:IWorkQueueItem<T>
	{
		readonly Action<T> _finish;
		readonly Action<T> _cancel;

		/// <summary>
		/// Has an item been dequeued?
		/// If false, Item will be default value (i.e. null)
		/// </summary>
		public bool HasItem { get; set; }

		/// <summary>
		/// Queue item if one was available
		/// </summary>
		public T Item { get; set; }

		/// <summary>
		/// Create an empty item (represents an unsucessful dequeue)
		/// </summary>
		public WorkQueueItem()
		{
			HasItem = false;
		}

		/// <summary>
		/// Create a populated item with optional finish and cancel items
		/// </summary>
		/// <param name="item">Item dequeued</param>
		/// <param name="finish">Finish action (may be null)</param>
		/// <param name="cancel">Cancel action (may be null)</param>
		public WorkQueueItem(T item, Action<T> finish, Action<T> cancel)
		{
			_finish = finish ?? (t => { });
			_cancel = cancel ?? (t => { });
			HasItem = true;
			Item = item;
		}

		/// <summary>
		/// Call this to permanently remove an item from the queue
		/// </summary>
		public void Finish()
		{
			_finish(Item);
		}

		/// <summary>
		/// Call this to cancel the dequeue and return item to work queue.
		/// There is no guarantee where the item will be returned (head, end or somewhere in the middle)
		/// </summary>
		public void Cancel()
		{
			_cancel(Item);
		}
	}
}