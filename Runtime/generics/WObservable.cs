using System;

namespace Wowsome.Generic {
  public class WObservable<T> {
    public T Value { get; private set; }

    Action<T> _observers = null;

    public WObservable(T v) {
      Value = v;
    }

    public void Subscribe(Action<T> observer) {
      _observers += observer;
    }

    public void Unsubscribe(Action<T> observer) {
      _observers -= observer;
    }

    public void Next(T v) {
      Value = v;
      Broadcast();
    }

    public void Broadcast() {
      if (null != _observers) _observers.Invoke(Value);
    }
  }
}
