using UnityEngine;

namespace Wowsome {
  public static class CameraExtensions {
    public static Bounds OrthoBounds(this Camera camera) {
      float screenAspect = (float)Screen.width / Screen.height;
      float cameraHeight = camera.orthographicSize * 2;

      Bounds bounds = new Bounds(
        camera.transform.position,
        new Vector3(cameraHeight * screenAspect, cameraHeight, 0)
      );

      return bounds;
    }

    public static Vector2 OrthoSize(this Camera camera) {
      float height = 2f * camera.orthographicSize;
      float width = height * camera.aspect;

      return new Vector2(width, height);
    }
  }
}