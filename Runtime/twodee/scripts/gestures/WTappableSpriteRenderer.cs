using UnityEngine;
using UnityEngine.EventSystems;

namespace Wowsome.TwoDee {
  [RequireComponent(typeof(SpriteRenderer))]
  public class WTappableSpriteRenderer : WTappable {
    protected SpriteRenderer _renderer;

    public override void InitTappable(Camera cam) {
      base.InitTappable(cam);

      _renderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnTap(PointerEventData ed) {
      Vector2 worldPos = _camera.ScreenToWorldPoint(ed.position);
      bool isInRect = _renderer.Contains(worldPos);

      if (isInRect) {
        OnEndInside?.Invoke(ed);
      } else {
        OnEndOutside?.Invoke(ed);
      }
    }
  }
}