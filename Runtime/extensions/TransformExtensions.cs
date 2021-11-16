using UnityEngine;

namespace Wowsome {
  public static class TransformExt {
    public static Transform SetPos(this Transform t, Vector2 pos) {
      t.position = new Vector3(pos.x, pos.y, t.position.z);

      return t;
    }

    public static Transform SetPos(this Transform t, float x, float y) {
      return t.SetPos(new Vector2(x, y));
    }

    public static Transform SetX(this Transform t, float x) {
      t.position = new Vector3(x, t.position.y, t.position.z);

      return t;
    }

    public static Transform AddX(this Transform t, float x) {
      Vector3 curPos = t.position;
      t.position = new Vector3(curPos.x + x, curPos.y, curPos.z);

      return t;
    }

    public static Transform SetY(this Transform t, float y) {
      t.position = new Vector3(t.position.x, y, t.position.z);

      return t;
    }

    public static Transform Scale(this Transform t, Vector2 scale) {
      t.localScale = new Vector3(scale.x, scale.y, t.localScale.z);

      return t;
    }

    public static Transform Scale(this Transform t, float scale) {
      return t.Scale(new Vector2(scale, scale));
    }
  }
}