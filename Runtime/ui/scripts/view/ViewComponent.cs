using UnityEngine;
using Wowsome.Core;

namespace Wowsome {
  namespace UI {
    public abstract class ViewComponent : MonoBehaviour, IViewComponent {
      public abstract void Setup(ISceneStarter sceneStarter, IViewManager viewManager);
    }
  }
}
