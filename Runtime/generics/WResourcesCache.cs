using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Wowsome {
  /// <summary>
  /// Loads the resource and stores it in the cache
  /// 
  /// It tries to find the object in [[_resources]] first.
  /// If dont exist, it loads the resource from the given path in Load(path) 
  /// and set the path as the key in [[_resources]]
  /// </summary>  
  public class WResourcesCache<TObject> where TObject : Object {
    Dictionary<string, TObject> _resources = new Dictionary<string, TObject>();

    public TObject Load(string path) {
      if (_resources.ContainsKey(path)) {
        return _resources[path];
      }

      TObject loaded = Resources.Load<TObject>(path);
      if (null != loaded) {
        _resources[path] = loaded;
      } else {
        Print.Warn($"cant load resource from {path}");
      }

      return loaded;
    }

    public TObject CombinePathAndLoad(params string[] path) {
      string p = Path.Combine(path);
      return Load(p);
    }
  }
}