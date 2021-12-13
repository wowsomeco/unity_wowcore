using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  public static class SpriteRendererExtensions {
    public static Bounds MaxBounds(this IList<SpriteRenderer> renderers) {
      Bounds b = new Bounds();
      foreach (SpriteRenderer renderer in renderers) {
        b.Encapsulate(renderer.bounds);
      }

      return b;
    }

    public static Vector2 WorldSize(this SpriteRenderer s, bool roundDown = true) {
      Vector2 spriteSize = s.sprite.rect.size;
      float ppu = roundDown ? Mathf.Ceil(s.sprite.pixelsPerUnit) : Mathf.Floor(s.sprite.pixelsPerUnit);

      return spriteSize / ppu;
    }

    /// <summary>
    /// Checks whether the sprite is still visible within the camera viewport
    /// </summary>
    public static bool IsVisibleFrom(this SpriteRenderer renderer, Camera camera) {
      return renderer.bounds.IsVisibleFrom(camera);
    }

    public static bool Intersects(this SpriteRenderer lhs, SpriteRenderer rhs) {
      return lhs.bounds.Intersects(rhs.bounds);
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
      renderer.transform.SetRotation(r);

      return renderer;
    }

    public static Vector3 Size(this SpriteRenderer renderer) => renderer.bounds.size;

    public static float Width(this SpriteRenderer renderer) => renderer.Size().x;

    public static float Height(this SpriteRenderer renderer) => renderer.Size().y;
  }
}