using UnityEngine;
using UnityEngine.EventSystems;

namespace Wowsome.TwoDee {
  [RequireComponent(typeof(BoxCollider2D))]
  public class WTappableCollider2D : WTappable {
    protected BoxCollider2D _collider;

    public override void InitTappable(Camera cam) {
      base.InitTappable(cam);

      _collider = GetComponent<BoxCollider2D>();
    }

    protected override void OnTap(PointerEventData ed) {
      Vector3 worldPos = _camera.ScreenToWorldPoint(ed.position);
      bool isInside = _collider.bounds.Contains(worldPos.SetZ(_collider.WorldPos().z));

      if (isInside) {
        OnEndInside?.Invoke(ed);
      } else {
        OnEndOutside?.Invoke(ed);
      }
    }
  }
}
