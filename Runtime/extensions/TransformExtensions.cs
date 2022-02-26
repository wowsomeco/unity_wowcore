using UnityEngine;

namespace Wowsome {
  public static class TransformExt {
    public static Transform SetPos(this Transform t, Vector2 pos) {
      t.position = new Vector3(pos.x, pos.y, t.position.z);

      return t;
    }

    public static Transform SetPos(this Transform t, Vector3 pos) {
      t.position = pos;

      return t;
    }

    public static Transform SetPos(this Transform t, float x, float y) {
      return t.SetPos(new Vector2(x, y));
    }

    public static Transform SetPos(this Transform t, Transform other) {
      return t.SetPos(other.position);
    }

    public static Transform AddPos(this Transform t, Vector2 pos) {
      Vector2 curPos = t.WorldPos();
      return t.SetPos(curPos.x + pos.x, curPos.y + pos.y);
    }

    public static Transform SetLocalPos(this Transform t, Vector2 pos) {
      t.localPosition = new Vector3(pos.x, pos.y, t.localPosition.z);

      return t;
    }

    public static Transform SetLocalPos(this Transform t, float x, float y) {
      return t.SetLocalPos(new Vector2(x, y));
    }

    public static Transform SetX(this Transform t, float x) {
      t.position = new Vector3(x, t.position.y, t.position.z);

      return t;
    }

    public static float X(this Transform t) => t.position.x;

    public static Transform AddX(this Transform t, float x) {
      Vector3 curPos = t.position;
      t.position = new Vector3(curPos.x + x, curPos.y, curPos.z);

      return t;
    }

    public static Transform SetY(this Transform t, float y) {
      t.position = new Vector3(t.position.x, y, t.position.z);

      return t;
    }

    public static float Y(this Transform t) => t.position.y;

    public static Transform AddY(this Transform t, float y) {
      Vector3 curPos = t.position;
      t.position = new Vector3(curPos.x, curPos.y + y, curPos.z);

      return t;
    }

    public static Transform Scale(this Transform t, Vector2 scale) {
      t.localScale = new Vector3(scale.x, scale.y, t.localScale.z);

      return t;
    }

    public static Transform Scale(this Transform t, float scale) {
      return t.Scale(new Vector2(scale, scale));
    }

    public static Transform AddScale(this Transform t, float delta) {
      return t.Scale(t.localScale + new Vector3(delta, delta, 0f));
    }

    public static float Rotation(this Transform t) {
      return t.localEulerAngles.z.WrapAngle();
    }

    public static Transform SetRotation(this Transform t, float r) {
      t.localRotation = Quaternion.Euler(0f, 0f, r);

      return t;
    }

    public static Transform AddRotation(this Transform t, float delta) {
      float curRotation = t.Rotation() + delta;
      return t.SetRotation(curRotation);
    }
  }
}