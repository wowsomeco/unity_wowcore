using UnityEngine;

namespace Wowsome {
  public static class CameraExtensions {
    public static Bounds OrthoBounds(this Camera camera) {
      float size = camera.orthographicSize * 2;
      float width = size * (float)Screen.width / Screen.height;
      float height = size;

      Bounds bounds = new Bounds(
        camera.transform.position,
        new Vector3(width, height, 0)
      );

      return bounds;
    }

    public static Vector2 OrthoSize(this Camera camera) {
      float height = 2f * camera.orthographicSize;
      float width = height * camera.aspect;

      return new Vector2(width, height);
    }

    public static Vector2 GetAnchoredPos(this Camera cam, Vector2 anchor, Vector2 offset) {
      Vector2 pos = cam.ViewportToWorldPoint(anchor);

      pos = pos.Add(offset);

      return pos;
    }
  }
}