using System.Collections.Generic;

namespace Wowsome.Generic {
  public class WRandomizer<T> {
    List<T> _items;

    public WRandomizer(List<T> items) {
      _items = items;
    }

    /// <summary>
    /// Returns the next random item from the list
    /// </summary>    
    public T Next {
      get {
        int idx = new System.Random().Next(0, _items.Count);
        return _items[idx];
      }
    }
  }
}