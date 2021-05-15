using UnityEngine;
using Wowsome.Core;
using Wowsome.UI;

namespace Wowsome.TwoDee {
  public class WParticlePlayer : MonoBehaviour, ISceneController {
    public WParticlePool ParticlePool { get; private set; }

    [Tooltip("the game camera to render particle, sprite renderer, as well as UI stuff")]
    public Camera renderCamera;
    public WTouchSurface touchSurface;
    [Tooltip("particle id that gets played on swipe screen")]
    public string swipeParticleId;

    public void PlayParticle(string particleId, Vector2 screenPos) {
      if (particleId.IsEmpty()) return;

      Vector3 worldPos = renderCamera.ScreenToWorldPoint(screenPos);
      PlayParticle(particleId, worldPos);
    }

    public void PlayParticle(string particleId, Vector3 worldPos) {
      if (particleId.IsEmpty()) return;

      ParticlePool.Activate(particleId, worldPos);
    }

    public void InitSceneController(ISceneStarter sceneStarter) {
      sceneStarter.OnStartSceneController += starter => {
        // particle pool is required
        ParticlePool = starter.GetController<WParticlePool>();
        Assert.Null<WParticlePool>(ParticlePool);
        // touchSurface is required
        Assert.Null<WTouchSurface>(touchSurface);
        touchSurface.InitTouchSurface();
        // observe swipe
        touchSurface.OnMovingTouch += touch => {
          PlayParticle(swipeParticleId, touch.ScreenPos);
        };
      };
    }

    public void UpdateSceneController(float dt) {

    }
  }
}

