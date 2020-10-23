using System;
using System.Collections.Generic;

namespace Wowsome.Generic {
  public class WObservable<T> {
    public T Value { get; private set; }

    List<Action<T>> _observers = new List<Action<T>>();

    public WObservable(T v) {
      Value = v;
    }

    public void Subscribe(Action<T> observer) {
      _observers.Add(observer);
    }

    public void Unsubscribe(Action<T> observer) {
      _observers.Remove(observer);
    }

    public void Next(T v) {
      Value = v;
      Broadcast();
    }

    public void Broadcast() {
      _observers.ForEach(obs => {
        if (null != obs) obs(Value);
      });
    }
  }
}
