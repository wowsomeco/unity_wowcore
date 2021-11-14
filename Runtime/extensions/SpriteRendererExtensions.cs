using UnityEngine;

namespace Wowsome {
  public static class SpriteRendererExtensions {
    public static Vector2 WorldSize(this SpriteRenderer s, bool roundDown = true) {
      Vector2 spriteSize = s.sprite.rect.size;
      float ppu = roundDown ? Mathf.Ceil(s.sprite.pixelsPerUnit) : Mathf.Floor(s.sprite.pixelsPerUnit);

      return spriteSize / ppu;
    }

    /// <summary>
    /// Checks whether the sprite is still visible within the camera viewport
    /// </summary>
    public static bool IsVisibleFrom(this SpriteRenderer renderer, Camera camera) {
      Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
      return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static SpriteRenderer SetAlpha(this SpriteRenderer renderer, float a) {
      Color curColor = renderer.color;
      curColor.a = a;
      renderer.color = curColor;

      return renderer;
    }

    public static Color Color(this SpriteRenderer renderer) {
      return renderer.color;
    }

    public static SpriteRenderer SetColor(this SpriteRenderer renderer, Color c) {
      renderer.color = c;

      return renderer;
    }

    public static SpriteRenderer SetRotation(this SpriteRenderer renderer, float r) {
      renderer.transform.localRotation = Quaternion.Euler(0f, 0f, r);

      return renderer;
    }
  }
}