using System.Collections.Generic;

namespace Wowsome.Generic {
  public class WPickableList<T> {
    /// <summary>
    /// Returns the next random item from the list
    /// </summary>    
    public T Random => _items.PickRandom();
    public T Next {
      get {
        T nextItem = _items[_counter];

        ++_counter;
        if (_counter >= _items.Count) {
          _counter = 0;
        }

        return nextItem;
      }
    }

    List<T> _items;
    int _counter = 0;

    public WPickableList(IList<T> items) {
      _items = new List<T>(items);
    }
  }
}