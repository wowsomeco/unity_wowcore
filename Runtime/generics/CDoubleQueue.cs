using System.Collections.Generic;

namespace Wowsome {
  public class CDoubleQueue<T> {
    Queue<T> m_first;
    Queue<T> m_second = new Queue<T>();

    public CDoubleQueue() {
      m_first = new Queue<T>();
    }

    public CDoubleQueue(IEnumerable<T> enumerables) {
      m_first = new Queue<T>(enumerables);
    }

    public void Enqueue(T item) {
      m_first.Enqueue(item);
    }

    public T DequeueFirst() {
      return Dequeue(m_first, m_second);
    }

    public T DequeueSecond() {
      return Dequeue(m_second, m_first);
    }

    public int Count() {
      return m_first.Count + m_second.Count;
    }

    T Dequeue(Queue<T> dequeued, Queue<T> enqueued) {
      if (dequeued.Count <= 0) {
        return default(T);
      }
      T dequeuedItem = dequeued.Dequeue();
      enqueued.Enqueue(dequeuedItem);
      return dequeuedItem;
    }
  }
}
