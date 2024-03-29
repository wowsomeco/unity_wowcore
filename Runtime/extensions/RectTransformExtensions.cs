﻿using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  public static class RectTransformExtensions {
    public static Vector3 ScreenToWorldPos(this RectTransform rectTransform, Vector2 screenPos, Camera cam = null) {
      Vector3 worldPos = Vector3.zero;
      RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPos, cam, out worldPos);
      return worldPos;
    }

    public static Vector3 ScreenToLocalPos(this RectTransform rectTransform, Vector2 screenPos, Camera cam = null) {
      RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, cam, out Vector2 localPos);
      return localPos;
    }

    public static RectTransform SetLeft(this RectTransform rt, float left) {
      rt.offsetMin = new Vector2(left, rt.offsetMin.y);
      return rt;
    }

    public static RectTransform SetRight(this RectTransform rt, float right) {
      rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
      return rt;
    }

    public static RectTransform SetTop(this RectTransform rt, float top) {
      rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
      return rt;
    }

    public static RectTransform SetBottom(this RectTransform rt, float bottom) {
      rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
      return rt;
    }

    public static RectTransform Normalize(this RectTransform rt) {
      rt.SetLocalPos(Vector2.zero);
      rt.SetScale(Vector2.one);
      rt.SetRotation(0f);
      return rt;
    }

    public static RectTransform Stretch(this RectTransform rt) {
      rt.anchorMin = Vector2.zero;
      rt.anchorMax = Vector2.one;
      rt.pivot = new Vector2(0.5f, 0.5f);
      rt.SetLeft(0f).SetRight(0f).SetTop(0f).SetBottom(0f);

      return rt;
    }

    #region Position

    public static RectTransform SetLocalPos(this RectTransform rt, Vector2 pos) {
      rt.anchoredPosition = pos;
      return rt;
    }

    public static RectTransform SetLocalPos(this RectTransform rt, float[] pos) {
      rt.SetLocalPos(new Vector2(pos[0], pos[1]));
      return rt;
    }

    public static RectTransform SetLocalPos(this RectTransform rt, List<float> pos) {
      rt.SetLocalPos(new Vector2(pos[0], pos[1]));
      return rt;
    }

    public static RectTransform SetWorldPos(this RectTransform rt, Vector2 pos) {
      rt.transform.SetWorldPos(pos);
      return rt;
    }

    public static RectTransform AddLocalPosY(this RectTransform rt, float delta) {
      Vector2 addPos = rt.LocalPos();
      addPos.y += delta;
      rt.SetLocalPos(addPos);

      return rt;
    }

    public static RectTransform AddLocalPosX(this RectTransform rt, float delta) {
      Vector2 addPos = rt.LocalPos();
      addPos.x += delta;
      rt.SetLocalPos(addPos);

      return rt;
    }

    public static RectTransform AddLocalPos(this RectTransform rt, Vector2 offset) {
      Vector2 pos = rt.LocalPos();
      rt.SetLocalPos(pos + offset);

      return rt;
    }

    public static Vector2 LocalPos(this RectTransform rt) {
      return rt.anchoredPosition;
    }

    public static float LocalX(this RectTransform rt) {
      return rt.LocalPos().x;
    }

    public static RectTransform SetLocalX(this RectTransform rt, float x) {
      Vector2 pos = rt.LocalPos();
      pos.x = x;
      rt.SetLocalPos(pos);

      return rt;
    }

    public static float LocalY(this RectTransform rt) {
      return rt.LocalPos().y;
    }

    public static RectTransform SetLocalY(this RectTransform rt, float y) {
      Vector2 pos = rt.LocalPos();
      pos.y = y;
      rt.SetLocalPos(pos);

      return rt;
    }

    #endregion

    #region Scale

    public static RectTransform SetScale(this RectTransform rt, Vector2 scale) {
      rt.localScale = scale;

      return rt;
    }

    public static RectTransform SetScale(this RectTransform rt, float x, float y) {
      return rt.SetScale(new Vector2(x, y));
    }

    public static RectTransform SetScale(this RectTransform rt, float scale) {
      rt.localScale = Vector2.one * scale;

      return rt;
    }

    public static void SetScaleX(this RectTransform rt, float x) {
      rt.localScale = new Vector2(x, rt.localScale.y);
    }

    public static void SetScaleY(this RectTransform rt, float y) {
      rt.localScale = new Vector2(rt.localScale.x, y);
    }

    public static void SetDefaultScale(this RectTransform trans) {
      trans.SetScale(Vector2.one);
    }

    public static RectTransform SetScale(this RectTransform rt, float[] scale) {
      return rt.SetScale(new Vector2(scale[0], scale[1]));
    }

    public static Vector2 Scale(this RectTransform rt) {
      return rt.localScale;
    }

    #endregion

    #region Size

    public static Vector2 Size(this RectTransform trans, bool shouldRound = false) {
      return shouldRound ? new Vector2(trans.rect.size.x.Round(), trans.rect.size.y.Round()) : trans.rect.size;
    }

    public static float Width(this RectTransform trans) {
      return trans.rect.width;
    }

    public static float Height(this RectTransform trans) {
      return trans.rect.height;
    }

    public static RectTransform SetPivot(this RectTransform rt, Vector2 pivot) {
      rt.pivot = pivot;
      return rt;
    }

    public static RectTransform SetSize(this RectTransform trans, Vector2 newSize) {
      Vector2 oldSize = trans.rect.size;
      Vector2 deltaSize = newSize - oldSize;
      trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
      trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));

      return trans;
    }

    public static RectTransform SetSize(this RectTransform trans, RectTransform other) {
      SetSize(trans, other.Size());

      return trans;
    }

    public static void SetSize(this RectTransform rt, Rect rect, bool ignorePos = false) {
      if (!ignorePos) {
        rt.SetLocalPos(new Vector2(rect.x, rect.y));
      }
      rt.SetSize(new Vector2(rect.width, rect.height));
    }

    public static void DivideSize(this RectTransform trans, float divider) {
      Vector2 newSize = trans.Size() / divider;
      SetSize(trans, newSize);
    }

    public static void MultiplySize(this RectTransform trans, float multiplier) {
      Vector2 newSize = trans.Size() * multiplier;
      SetSize(trans, newSize);
    }

    public static void SetWidth(this RectTransform trans, float newSize) {
      SetSize(trans, new Vector2(newSize, trans.Size().y));
    }

    public static void AddWidth(this RectTransform rt, float w) {
      SetSize(rt, new Vector2(rt.Size().x + w, rt.Size().y));
    }

    public static void SetHeight(this RectTransform trans, float newSize) {
      SetSize(trans, new Vector2(trans.Size().x, newSize));
    }

    public static void SetMaxSize(this RectTransform trans, float maxSize) {
      Vector2 newSize = trans.Size();
      float biggest = Mathf.Max(newSize.x, newSize.y);
      if (biggest > maxSize) {
        float ratio = maxSize / biggest;
        SetSize(trans, newSize *= ratio);
      }
    }

    public static void SetMinSize(this RectTransform trans, float minSize) {
      Vector2 newSize = trans.Size();
      float biggest = Mathf.Max(newSize.x, newSize.y);
      if (biggest < minSize) {
        float ratio = minSize / biggest;
        SetSize(trans, newSize *= ratio);
      }
    }

    public static void Copy(this RectTransform rectTransform, RectTransform other) {
      rectTransform.SetSize(other.Size());
      rectTransform.SetScale(other.Scale());
      rectTransform.SetLocalPos(other.LocalPos());
    }

    public static void SetOneOfSize(this RectTransform rt, int idx, float value) {
      Assert.If(idx < 0 || idx > 1, "idx needs to be either 0 or 1");

      Vector2 size = rt.Size();
      size[idx] = value;

      rt.SetSize(size);
    }

    #endregion

    #region Rotation

    public static float Rotation(this RectTransform rt) {
      return rt.localEulerAngles.z.WrapAngle();
    }

    public static RectTransform SetRotation(this RectTransform rt, float angle) {
      rt.localRotation = Quaternion.Euler(0f, 0f, angle);
      return rt;
    }

    public static RectTransform AddRotation(this RectTransform rt, float delta) {
      float curRotation = rt.Rotation() + delta;
      rt.SetRotation(curRotation);
      return rt;
    }

    public static void SetRotation(this RectTransform rt, Vector3 eulerAngles) {
      rt.eulerAngles = eulerAngles;
    }

    public static void SetRotation(this RectTransform rt, float[] vals) {
      rt.SetRotation(vals[0]);
    }

    public static void RandomizeRotation(this RectTransform rt, float min, float max) {
      float r = UnityEngine.Random.Range(min, max);
      rt.SetRotation(r);
    }

    #endregion

    #region Intersections

    public static Rect RectWorldPos(this RectTransform rt) {
      Vector3[] corners = new Vector3[4];
      rt.GetWorldCorners(corners);
      // Get the bottom left corner.
      Vector3 position = corners[0];

      Vector2 size = new Vector2(
        rt.lossyScale.x * rt.rect.size.x,
        rt.lossyScale.y * rt.rect.size.y
      );

      return new Rect(position, size);
    }

    public static bool Intersects(this RectTransform rt1, RectTransform rt2) {
      return rt1.RectWorldPos().Intersects(rt2.RectWorldPos());
    }

    #endregion

    public static bool IsPointInRect(this RectTransform rt, Vector2 screenPos, Camera cam) {
      return RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, cam);
    }

    public static float Distance(this RectTransform rt, RectTransform other) {
      return Vector2.Distance(rt.transform.position, other.transform.position);
    }

    /// <summary>
    /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
    /// </summary>
    /// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
    private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera) {
      Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
      Vector3[] objectCorners = new Vector3[4];
      rectTransform.GetWorldCorners(objectCorners);

      int visibleCorners = 0;
      Vector3 tempScreenSpaceCorner; // Cached
      // For each corner in rectTransform
      for (int i = 0; i < objectCorners.Length; ++i) {
        tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
        // If the corner is inside the screen
        if (screenBounds.Contains(tempScreenSpaceCorner)) {
          ++visibleCorners;
        }
      }

      return visibleCorners;
    }

    /// <summary>
    /// Determines if this RectTransform is fully visible from the specified camera.
    /// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
    /// </summary>
    /// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
    public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera) {
      return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
    }

    /// <summary>
    /// Determines if this RectTransform is at least partially visible from the specified camera.
    /// Works by checking if any bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
    /// </summary>
    /// <returns><c>true</c> if is at least partially visible from the specified camera; otherwise, <c>false</c>.</returns>
    public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera) {
      return CountCornersVisibleFrom(rectTransform, camera) > 0; // True if any corners are visible
    }
  }
}