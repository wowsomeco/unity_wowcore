using System;
using System.Collections.Generic;

namespace Wowsome.Generic {
  public interface IPoolObject {
    string ObjId { get; }
    void InitPoolObject(WObjectPool pool);
    void OnActivated();
    void OnReleased();
  }

  public class WObjectPool {
    public Func<string, bool> OnPoolEmpty { get; set; }
    public Action<IPoolObject> OnReleased { get; set; }
    public List<IPoolObject> ActiveObjects { get; private set; } = new List<IPoolObject>();

    Dictionary<string, Queue<IPoolObject>> _poolObjects = new Dictionary<string, Queue<IPoolObject>>();

    public IPoolObject Get(string id) {
      if (!_poolObjects.ContainsKey(id)) return null;

      var pool = _poolObjects[id];

      if (pool.Count == 0) {
        if (!OnPoolEmpty.Invoke(id)) return null;
      }

      IPoolObject obj = pool.Dequeue();
      obj.OnActivated();

      ActiveObjects.Add(obj);

      return obj;
    }

    public void Add(IPoolObject obj) {
      obj.InitPoolObject(this);
      if (!_poolObjects.ContainsKey(obj.ObjId)) {
        _poolObjects[obj.ObjId] = new Queue<IPoolObject>();
      }
      _poolObjects[obj.ObjId].Enqueue(obj);
    }

    public bool TryRelease(string id) {
      if (ActiveObjects.Find(x => x.ObjId.CompareStandard(id)) is IPoolObject obj) {
        Release(obj);

        return true;
      }

      return false;
    }

    public void Release(IPoolObject obj) {
      obj.OnReleased();
      OnReleased?.Invoke(obj);
      // remove from active object and send back to pool
      ActiveObjects.Remove(obj);
      _poolObjects[obj.ObjId].Enqueue(obj);
    }
  }
}

