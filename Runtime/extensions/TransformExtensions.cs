using UnityEngine;

namespace Wowsome {
  public static class TransformExt {
    public static void SetPos(this Transform t, Vector2 pos) {
      t.position = new Vector3(pos.x, pos.y, t.position.z);
    }
  }
}