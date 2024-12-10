using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWIS.MicroState.Model
{
    public class CircularBuffer<T>
    {
        private readonly Queue<T> _queue;
        private readonly int _capacity;

        public CircularBuffer(int capacity)
        {
            _capacity = capacity;
            _queue = new Queue<T>(capacity);
        }

        public void Add(T item)
        {
            if (_queue.Count == _capacity)
            {
                _queue.Dequeue(); // Remove the oldest item
            }
            _queue.Enqueue(item); // Add the new item
        }

        public T Peek()
        {
            return _queue.Peek();
        }

        public T GetLast() 
        { 
            return _queue.Last();
        }

        public IEnumerable<T> GetItems()
        {
            return _queue.ToArray();
        }

        public int Count => _queue.Count;
        public int Capacity => _capacity;
    }
}
