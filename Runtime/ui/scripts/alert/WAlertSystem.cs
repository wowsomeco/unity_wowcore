using System;
using UnityEngine;
using Wowsome.Core;

namespace Wowsome.UI {
  public class WAlertSystem : MonoBehaviour, ISystem {
    public class ActionOptions {
      public string Txt { get; set; }
      public Action OnTap { get; set; }
    }

    public class ShowOptions {
      public string Title { get; set; }
      public string Content { get; set; }
      public ActionOptions YesOption { get; set; } = null;
      public ActionOptions NoOption { get; set; } = null;
    }

    public Action<ShowOptions> OnShow { get; set; }
    public Action OnHide { get; set; }

    protected ShowOptions _curShowOptions = null;

    public void Show(ShowOptions options) {
      _curShowOptions = options;
      OnShow?.Invoke(_curShowOptions);
    }

    public virtual void InitSystem() { }

    public virtual void StartSystem(CavEngine gameEngine) { }

    public virtual void UpdateSystem(float dt) { }
  }
}

