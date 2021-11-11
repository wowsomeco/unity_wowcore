using UnityEngine;

namespace Wowsome {
  public static class TransformExt {
    public static void SetPos(this Transform t, Vector2 pos) {
      t.position = new Vector3(pos.x, pos.y, t.position.z);
    }

    public static void SetX(this Transform t, float x) {
      t.position = new Vector3(x, t.position.y, t.position.z);
    }

    public static void SetY(this Transform t, float y) {
      t.position = new Vector3(t.position.x, y, t.position.z);
    }
  }
}