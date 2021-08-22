using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  public class WAnimatorActivator : MonoBehaviour, IAnimComponent {
    [Serializable]
    public class Model {
      public List<string> animIds = new List<string>();
      public List<GameObject> showObjects = new List<GameObject>();
      public List<GameObject> hideObjects = new List<GameObject>();
      [Tooltip("if true, then whenever the animId that is starting is not the same as animId, the visibility of the showObjects & hideObjects will get toggled")]
      public bool toggleIfOtherwise;
    }

    public List<Model> models = new List<Model>();

    public void InitAnimator(WAnimController controller) {
      controller.OnAnimStart += animId => {
        foreach (Model model in models) {
          if (model.animIds.Contains(animId)) {
            model.showObjects.ForEach(o => o.SetActive(true));
            model.hideObjects.ForEach(o => o.SetActive(false));
          } else if (model.toggleIfOtherwise) {
            model.showObjects.ForEach(o => o.SetActive(false));
            model.hideObjects.ForEach(o => o.SetActive(true));
          }
        }
      };
    }
  }
}

