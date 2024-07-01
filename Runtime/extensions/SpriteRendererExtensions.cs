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

    public static SpriteRenderer AnchorToCamera(this SpriteRenderer s, Camera c, Vector3 screenAnchor) {
      Vector2 pos = c.ViewportToWorldPoint(screenAnchor);
      return s.SetWorldPos(pos);
    }

    public static Vector2 MidLeftCameraPos(this SpriteRenderer s, Camera c) {
      Vector2 pos = c.ViewportToWorldPoint(new Vector2(0f, .5f));

      return pos.AddX(-s.Width().Half());
    }

    public static Vector2 TopCenterCameraPos(this SpriteRenderer s, Camera c) {
      Vector2 pos = c.ViewportToWorldPoint(new Vector2(0.5f, 1f));

      return pos.AddY(s.Height().Half());
    }

    public static Vector2 BottomCenterCameraPos(this SpriteRenderer s, Camera c) {
      Vector2 pos = c.ViewportToWorldPoint(new Vector2(0.5f, 0f));

      return pos.AddY(-s.Height().Half());
    }

    public static Vector2 MidRightCameraPos(this SpriteRenderer s, Camera c) {
      Vector2 pos = c.ViewportToWorldPoint(new Vector2(1f, .5f));

      return pos.AddX(s.Width().Half());
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

    public static bool Intersects(this IList<SpriteRenderer> renderers, SpriteRenderer other) {
      foreach (SpriteRenderer sr in renderers) {
        if (sr.bounds.Intersects(other.bounds)) {
          return true;
        }
      }

      return false;
    }

    public static bool Contains(this SpriteRenderer sr, Vector2 point) {
      return sr.bounds.Contains(point.ToVector3().SetZ(sr.transform.position.z));
    }

    public static bool Contains(this IList<SpriteRenderer> renderers, Vector2 point, bool shouldActive = false) {
      foreach (SpriteRenderer sr in renderers) {
        if (shouldActive && !sr.IsVisible()) continue;

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
      curColor.a = a.Clamp(0f, 1f);
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
      renderer.transform.SetScale(scale);

      return renderer;
    }

    public static SpriteRenderer SetScale(this SpriteRenderer renderer, Vector2 scale) {
      renderer.transform.SetScale(scale);

      return renderer;
    }

    public static SpriteRenderer AddScale(this SpriteRenderer renderer, float delta) {
      return renderer.AddScale(new Vector2(delta, delta));
    }

    public static SpriteRenderer AddScale(this SpriteRenderer renderer, Vector2 delta) {
      Vector2 newScale = renderer.Scale().Add(delta);
      renderer.transform.SetScale(newScale);

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

    public static SpriteRenderer SetWorldPos(this SpriteRenderer renderer, Vector2 worldPos) {
      renderer.transform.SetWorldPos(worldPos);

      return renderer;
    }

    public static SpriteRenderer SetLocalPos(this SpriteRenderer renderer, Vector2 localPos) {
      renderer.transform.SetLocalPos(localPos);

      return renderer;
    }

    public static SpriteRenderer AddWorldPos(this SpriteRenderer renderer, Vector2 delta) {
      renderer.transform.AddWorldPos(delta);
      return renderer;
    }

    public static SpriteRenderer SetWorldX(this SpriteRenderer renderer, float x) {
      renderer.transform.SetWorldX(x);

      return renderer;
    }

    public static float WorldX(this SpriteRenderer renderer) {
      return renderer.transform.WorldX();
    }

    public static SpriteRenderer SetLocalX(this SpriteRenderer renderer, float x) {
      renderer.transform.SetLocalX(x);

      return renderer;
    }

    public static float LocalX(this SpriteRenderer renderer) => renderer.transform.LocalX();

    public static SpriteRenderer SetWorldY(this SpriteRenderer renderer, float y) {
      renderer.transform.SetWorldY(y);

      return renderer;
    }

    public static float WorldY(this SpriteRenderer renderer) {
      return renderer.transform.WorldY();
    }

    public static SpriteRenderer SetLocalY(this SpriteRenderer renderer, float y) {
      renderer.transform.SetLocalY(y);

      return renderer;
    }

    public static float LocalY(this SpriteRenderer renderer) => renderer.transform.LocalY();

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