using System.Collections.Generic;

namespace DispatchSharp
{
	public class InMemoryWorkQueue<T> : IWorkQueue<T>
	{
		readonly object _lockObject;
		readonly Queue<T> _queue;

		public InMemoryWorkQueue()
		{
			_queue = new Queue<T>();
			_lockObject = new object();
		}

		public void Enqueue(T work)
		{
			lock (_lockObject)
			{
				_queue.Enqueue(work);
			}
		}

		public IWorkQueueItem<T> TryDequeue()
		{
			lock(_lockObject)
			{
				if (_queue.Count < 1) return NoItem();

				return new WorkQueueItem<T>(_queue.Dequeue());
			}
		}

		IWorkQueueItem<T> NoItem()
		{
			return new WorkQueueItem<T>();
		}
	}

	public class WorkQueueItem<T>:IWorkQueueItem<T>
	{
		public bool HasItem { get; set; }
		public T Item { get; set; }

		public WorkQueueItem()
		{
			HasItem = false;
		}
		public WorkQueueItem(T item)
		{
			HasItem = true;
			Item = item;
		}

		public void Finish()
		{
		}

		public void Cancel()
		{
		}
	}
}