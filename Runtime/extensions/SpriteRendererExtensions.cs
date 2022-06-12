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
      return s.bounds.size;
    }

    public static SpriteRenderer AnchorToCamera(this SpriteRenderer s, Camera c, Vector3 screenAnchor) {
      Vector2 pos = c.ViewportToWorldPoint(screenAnchor);
      return s.SetPos(pos);
    }

    public static Vector2 SignedPos(this SpriteRenderer s) {
      Vector2 pos = s.WorldPos();

      float x = Mathf.Approximately(pos.x, 0f) ? 0f : Mathf.Sign(pos.x);
      float y = Mathf.Approximately(pos.y, 0f) ? 0f : Mathf.Sign(pos.y);

      return new Vector2(x, y);
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

    public static bool Contains(this SpriteRenderer sr, Vector2 point) {
      return sr.bounds.Contains(point.ToVector3().SetZ(sr.transform.position.z));
    }

    public static bool Contains(this IList<SpriteRenderer> renderers, Vector2 point) {
      foreach (SpriteRenderer sr in renderers) {
        if (sr.Contains(point)) return true;
      }

      return false;
    }

    public static SpriteRenderer SetSprite(this SpriteRenderer renderer, Sprite sprite) {
      if (renderer == null || null == sprite) return renderer;

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

    public static SpriteRenderer AddAlpha(this SpriteRenderer renderer, float a) {
      return renderer.SetAlpha(renderer.Alpha() + a);
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

    public static SpriteRenderer AddPos(this SpriteRenderer renderer, Vector2 delta) {
      renderer.transform.AddPos(delta);
      return renderer;
    }

    public static SpriteRenderer SetX(this SpriteRenderer renderer, float x) {
      renderer.transform.SetX(x);

      return renderer;
    }

    public static SpriteRenderer SetY(this SpriteRenderer renderer, float y) {
      renderer.transform.SetY(y);

      return renderer;
    }

    public static SpriteRenderer SetParent(this SpriteRenderer renderer, Transform parent) {
      renderer.transform.SetParent(parent);

      return renderer;
    }

    public static SpriteRenderer SetSortingOrder(this SpriteRenderer renderer, int sortOrder) {
      renderer.sortingOrder = sortOrder;

      return renderer;
    }

    public static Vector3 Size(this SpriteRenderer renderer) => renderer.bounds.size;

    public static float Width(this SpriteRenderer renderer) => renderer.Size().x;

    public static float Height(this SpriteRenderer renderer) => renderer.Size().y;

    public static SpriteRenderer LookAt(this SpriteRenderer renderer, Vector2 worldPos, float angleOffset = 0f) {
      Vector2 dir = worldPos - renderer.WorldPos().ToVector2();

      float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
      angle += angleOffset;

      renderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

      return renderer;
    }

    public static bool IsWithinDistance(this SpriteRenderer renderer, Vector2 worldPos, float maxLimit) {
      float distance = Vector2.Distance(renderer.WorldPos(), worldPos);

      return distance < maxLimit;
    }
  }
}