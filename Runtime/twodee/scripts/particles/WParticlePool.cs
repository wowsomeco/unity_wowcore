using System;
using System.Collections.Generic;
using UnityEngine;
using Wowsome.Core;
using Wowsome.Generic;

namespace Wowsome.TwoDee {
  public class WParticlePool : MonoBehaviour, ISceneController {
    [Serializable]
    public class PrefabConfig {
      public ParticleSystem prefab;
      public string id;
      [Tooltip("the number of Gameobject to init")]
      public int initCount;
      [Tooltip("when it's true and there is no more object in the pool, it will instantiate a new gameobject")]
      public bool cloneWhenEmpty;
    }

    public List<PrefabConfig> prefabs = new List<PrefabConfig>();

    WObjectPool _pool = new WObjectPool();
    HashSet<WParticleObject> _objects = new HashSet<WParticleObject>();
    Dictionary<Vec2Int, WParticleObject> _activeObjects = new Dictionary<Vec2Int, WParticleObject>();

    public void Activate(string id, Vector3 pos) {
      Vec2Int intPos = Vec2Int.Create(pos);
      // optimization, dont render more than once on the same pos
      if (_activeObjects.ContainsKey(intPos)) {
        return;
      }

      var obj = _pool.Get(id);
      if (null != obj) {
        var po = obj as WParticleObject;
        Vector3 curPos = new Vector3(pos.x, pos.y, po.Position.z);
        po.Position = curPos;
        // add to cache active obj
        _activeObjects[intPos] = po;
      }
    }

    #region ISceneController

    public void InitSceneController(ISceneStarter sceneStarter) {
      for (int i = 0; i < prefabs.Count; ++i) {
        Clone(prefabs[i]);
      }

      _pool.OnPoolEmpty += id => {
        PrefabConfig config = prefabs.Find(x => x.id == id);
        if (null != config && config.cloneWhenEmpty) {
          Clone(config);
          return true;
        }

        return false;
      };

      _pool.OnReleased += obj => {
        // remove the obj from the cache of active objects
        WParticleObject pObj = obj as WParticleObject;
        _activeObjects.Remove(Vec2Int.Create(pObj.Position));
      };
    }

    public void UpdateSceneController(float dt) {
      foreach (WParticleObject obj in _objects) {
        obj.UpdateObject(dt);
      }
    }

    #endregion

    void Clone(PrefabConfig config) {
      for (int i = 0; i < config.initCount; ++i) {
        ParticleSystem p = config.prefab.Clone<ParticleSystem>(transform);
        WParticleObject pObj = new WParticleObject(p, config);
        _pool.Add(pObj);
        _objects.Add(pObj);
      }
    }
  }
}

