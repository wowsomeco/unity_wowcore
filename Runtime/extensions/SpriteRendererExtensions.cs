﻿using System.Collections.Generic;
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

    public static bool Contains(this SpriteRenderer lhs, Vector2 point) {
      return lhs.bounds.Contains(point.ToVector3().SetZ(lhs.transform.position.z));
    }

    public static SpriteRenderer SetSprite(this SpriteRenderer renderer, Sprite sprite) {
      renderer.sprite = sprite;

      return renderer;
    }

    public static float Alpha(this SpriteRenderer renderer) {
      return renderer.color.a;
    }

    public static SpriteRenderer SetAlpha(this SpriteRenderer renderer, float a) {
      Color curColor = renderer.color;
      curColor.a = a;
      renderer.color = curColor;

      return renderer;
    }

    public static Vector2 Scale(this SpriteRenderer renderer) {
      return renderer.transform.localScale;
    }

    public static SpriteRenderer SetScale(this SpriteRenderer renderer, float scale) {
      renderer.transform.Scale(scale);

      return renderer;
    }

    public static SpriteRenderer AddScale(this SpriteRenderer renderer, float delta) {
      return renderer.AddScale(new Vector2(delta, delta));
    }

    public static SpriteRenderer AddScale(this SpriteRenderer renderer, Vector2 delta) {
      Vector2 newScale = renderer.Scale().Add(delta);
      renderer.transform.Scale(newScale);

      return renderer;
    }

    public static Color Color(this SpriteRenderer renderer) {
      return renderer.color;
    }

    public static SpriteRenderer SetColor(this SpriteRenderer renderer, Color c) {
      renderer.color = c;

      return renderer;
    }

    public static float Rotation(this SpriteRenderer renderer) {
      return renderer.transform.Rotation();
    }

    public static SpriteRenderer SetRotation(this SpriteRenderer renderer, float r) {
      renderer.transform.SetRotation(r);

      return renderer;
    }

    public static SpriteRenderer AddRotation(this SpriteRenderer renderer, float delta) {
      renderer.transform.AddRotation(delta);

      return renderer;
    }

    public static SpriteRenderer SetPos(this SpriteRenderer renderer, Vector2 worldPos) {
      renderer.transform.SetPos(worldPos);

      return renderer;
    }

    public static Vector3 Size(this SpriteRenderer renderer) => renderer.bounds.size;

    public static float Width(this SpriteRenderer renderer) => renderer.Size().x;

    public static float Height(this SpriteRenderer renderer) => renderer.Size().y;
  }
}