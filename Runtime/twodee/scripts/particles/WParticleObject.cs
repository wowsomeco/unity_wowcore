using UnityEngine;
using Wowsome.Generic;

namespace Wowsome.TwoDee {
  public class WParticleObject : IPoolObject {
    public string ObjId => _id;
    public Vector3 Position {
      get => _particle.transform.position;
      set {
        _particle.transform.position = value;
      }
    }

    ParticleSystem _particle;
    string _id;
    WObjectPool _pool;
    bool _playing = false;

    public WParticleObject(ParticleSystem particle, WParticlePool.PrefabConfig config) {
      _id = config.id;
      _particle = particle;
    }

    public void InitPoolObject(WObjectPool pool) {
      _pool = pool;
      OnReleased();
    }

    public void OnActivated() {
      _particle.SetVisible(true);
      _particle.Play(true);

      _playing = true;
    }

    public void OnReleased() {
      _particle.Stop(true);
      _particle.SetVisible(false);
    }

    public void UpdateObject(float dt) {
      if (_playing && _particle.isStopped) {
        _playing = false;
        _pool.Release(this);
      }
    }
  }
}

