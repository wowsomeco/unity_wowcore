using System.Collections.Generic;

namespace Wowsome {
  namespace Generic {
    public interface IObserver { }

    public interface IObserver<T> : IObserver {
      void ReceiveMessage(T ev);
    }

    public class CMessenger {
      HashSet<IObserver> m_observers = new HashSet<IObserver>();

      public void AddObserver(IObserver observer) {
        if (!m_observers.Contains(observer)) {
          m_observers.Add(observer);
        }
      }

      public bool RemoveObserver(IObserver observer) {
        return m_observers.Remove(observer);
      }

      public void Clear() {
        m_observers.Clear();
      }

      public void BroadcastMessage<Ev>(Ev msg) {
        foreach (IObserver observer in m_observers) {
          IObserver<Ev> genericObserver = observer as IObserver<Ev>;
          if (null != genericObserver) {
            genericObserver.ReceiveMessage(msg);
          }
        }
      }
    }
  }
}

