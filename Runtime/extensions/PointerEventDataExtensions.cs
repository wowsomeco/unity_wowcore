using UnityEngine;
using UnityEngine.EventSystems;

namespace Wowsome {
  public static class PointerEventDataExtensions {
    public static Vector2 ToWorldPoint(this PointerEventData ed, Camera camera) {
      return camera.ScreenToWorldPoint(ed.position);
    }
  }
}