using System.Collections.Generic;

namespace Wowsome.Generic {
  public interface IObserver { }

  public interface IObserver<T> : IObserver {
    void ReceiveMessage(T ev);
  }

  public class CMessenger {
    HashSet<IObserver> _observers = new HashSet<IObserver>();

    public void AddObserver(IObserver observer) {
      if (!_observers.Contains(observer)) {
        _observers.Add(observer);
      }
    }

    public bool RemoveObserver(IObserver observer) {
      return _observers.Remove(observer);
    }

    public void Clear() {
      _observers.Clear();
    }

    public void BroadcastMessage<Ev>(Ev msg) {
      foreach (IObserver observer in _observers) {
        IObserver<Ev> genericObserver = observer as IObserver<Ev>;
        if (null != genericObserver) {
          genericObserver.ReceiveMessage(msg);
        }
      }
    }
  }
}

