using UnityEngine;
using UnityEngine.EventSystems;
using Wowsome.TwoDee;

namespace Wowsome.UI {
  [DisallowMultipleComponent]
  public class WTappableUI : WTappable {
    public RectTransform Rt => _rt;

    protected RectTransform _rt;

    public override void InitTappable(Camera cam) {
      base.InitTappable(cam);

      _rt = GetComponent<RectTransform>();
    }

    protected override void OnTap(PointerEventData ed) {
      if (_rt.IsPointInRect(ed.position, _camera)) {
        OnEndInside?.Invoke(ed);
      } else {
        OnEndOutside?.Invoke(ed);
      }
    }
  }
}