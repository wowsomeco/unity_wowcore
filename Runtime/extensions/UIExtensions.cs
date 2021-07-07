using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wowsome.Tween;
using Wowsome.UI;

namespace Wowsome {
  #region Rect Transform
  public static class RectTransformExtensions {
    public static Vector3 ScreenToWorldPos(this RectTransform rectTransform, Vector2 screenPos, Camera cam = null) {
      Vector3 worldPos = Vector3.zero;
      RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPos, cam, out worldPos);
      return worldPos;
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
      rt.SetPos(Vector2.zero);
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
    public static void SetPos(this RectTransform rt, Vector2 pos) {
      rt.anchoredPosition = pos;
    }

    public static void SetPos(this RectTransform rt, float[] pos) {
      rt.SetPos(new Vector2(pos[0], pos[1]));
    }

    public static void SetPos(this RectTransform rt, List<float> pos) {
      rt.SetPos(new Vector2(pos[0], pos[1]));
    }

    public static void AddPosY(this RectTransform rt, float delta) {
      Vector2 addPos = rt.Pos();
      addPos.y += delta;
      rt.SetPos(addPos);
    }

    public static void AddPosX(this RectTransform rt, float delta) {
      Vector2 addPos = rt.Pos();
      addPos.x += delta;
      rt.SetPos(addPos);
    }

    public static void AddPos(this RectTransform rt, Vector2 offset) {
      Vector2 pos = rt.Pos();
      rt.SetPos(pos + offset);
    }

    public static Vector2 Pos(this RectTransform rt) {
      return rt.anchoredPosition;
    }

    public static float X(this RectTransform rt) {
      return rt.Pos().x;
    }

    public static void SetX(this RectTransform rt, float x) {
      Vector2 pos = rt.Pos();
      pos.x = x;
      rt.SetPos(pos);
    }

    public static float Y(this RectTransform rt) {
      return rt.Pos().y;
    }

    public static void SetY(this RectTransform rt, float y) {
      Vector2 pos = rt.Pos();
      pos.y = y;
      rt.SetPos(pos);
    }
    #endregion

    #region Scale
    public static void SetScale(this RectTransform rt, Vector2 scale) {
      rt.localScale = scale;
    }

    public static void SetScale(this RectTransform rt, float x, float y) {
      rt.SetScale(new Vector2(x, y));
    }

    public static void SetScale(this RectTransform rt, float scale) {
      rt.localScale = Vector2.one * scale;
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

    public static void SetScale(this RectTransform rt, float[] scale) {
      rt.SetScale(new Vector2(scale[0], scale[1]));
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

    public static void SetSize(this RectTransform rt, Rect rect, bool ignorePos = false) {
      if (!ignorePos) {
        rt.SetPos(new Vector2(rect.x, rect.y));
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

    public static void Copy(this RectTransform rectTransform, RectTransform other) {
      rectTransform.SetSize(other.Size());
      rectTransform.SetScale(other.Scale());
      rectTransform.SetPos(other.Pos());
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

    public static void SetRotation(this RectTransform rt, Vector3 eulerAngles) {
      rt.eulerAngles = eulerAngles;
    }

    public static void SetRotation(this RectTransform rt, float[] vals) {
      rt.SetRotation(vals[0]);
    }
    #endregion

    #region Canvas Scaler
    public static CanvasScaler GetRootCanvasScaler(this RectTransform rt, bool shouldFindInChildren = false) {
      CanvasScaler canvasScaler = rt.root.GetComponent<CanvasScaler>();
      if (shouldFindInChildren && null == canvasScaler) {
        canvasScaler = rt.root.GetComponentInChildren<CanvasScaler>();
      }
      return canvasScaler;
    }

    public static RectTransform GetRootCanvasScalerRectTransform(this RectTransform rt, bool shouldFindInChildren = false) {
      CanvasScaler canvasScaler = rt.GetRootCanvasScaler(shouldFindInChildren);
      return canvasScaler.GetComponent<RectTransform>();
    }
    #endregion

    #region Intersections
    public static Rect RectWorldPos(this RectTransform rt) {
      return new Rect(rt.position.x, rt.position.y, rt.Width(), rt.Height());
    }

    public static Rect RectAgainstOther(this RectTransform rt, RectTransform other) {
      RectTransform parent = rt.parent.GetComponent<RectTransform>();
      //cache the cur pos
      Vector2 pos = rt.Pos();
      //iterate over until root
      while (parent.gameObject.GetInstanceID() != other.gameObject.GetInstanceID()) {
        //increment the pos with the parent's pos
        pos += parent.Pos();
        parent = parent.parent.GetComponent<RectTransform>();
      }
      //return
      return new Rect(pos.x, pos.y, rt.Width(), rt.Height());
    }

    public static Rect RectAgainstRoot(this RectTransform rt) {
      //cache the root and parent rt
      RectTransform root = rt.root.GetComponent<RectTransform>();
      RectTransform parent = rt.parent.GetComponent<RectTransform>();
      //cache the cur pos
      Vector2 pos = rt.Pos();
      //iterate over until root
      while (parent != root) {
        //increment the pos with the parent's pos
        pos += parent.Pos();
        parent = parent.parent.GetComponent<RectTransform>();
      }
      //return
      return new Rect(pos.x, pos.y, rt.Width(), rt.Height());
    }

    public static Rect RectWithOffset(this RectTransform rt, RectTransform offset) {
      if (rt == offset) {
        return rt.Rect();
      }
      Rect offsetRect = offset.Rect();
      Rect theRect = rt.Rect();
      return new Rect(offsetRect.x + theRect.x, offsetRect.y + theRect.y, theRect.width, theRect.height);
    }

    public static Rect Rect(this RectTransform rt1) {
      return new Rect(rt1.X(), rt1.Y(), rt1.Width(), rt1.Height());
    }

    public static bool Intersects(this RectTransform rt1, RectTransform rt2) {
      return rt1.Rect().Intersects(rt2.Rect());
    }

    public static bool Intersects(this RectTransform rt1, RectTransform rt2, Vector2 offset) {
      Rect r1 = rt1.Rect();
      Rect r2 = rt2.Rect();
      r1.x = offset.x;
      r1.y = offset.y;
      return r1.Intersects(r2);
    }
    #endregion

    #region Manipulation
    public static Vector2 ScreenToAnchoredPos(RectTransform canvasRt, Vector2 screenPos) {
      Vector2 viewportPos = Camera.main.ScreenToViewportPoint(screenPos);
      Vector2 anchoredPos = new Vector2((viewportPos.x * canvasRt.sizeDelta.x) - (canvasRt.sizeDelta.x * 0.5f),
        (viewportPos.y * canvasRt.sizeDelta.y) - (canvasRt.sizeDelta.y * 0.5f));
      return anchoredPos;
    }

    public static void Clamp(this RectTransform rt, Vector2 min, Vector2 max) {
      Vector2 pos = rt.anchoredPosition;
      pos.x = Mathf.Clamp(pos.x, min.x, max.x);
      pos.y = Mathf.Clamp(pos.y, min.y, max.y);
      rt.SetPos(pos);
    }

    public static void SetFromScreenPos(this RectTransform rt, RectTransform canvasRt, Vector2 screenPos) {
      rt.SetPos(ScreenToAnchoredPos(canvasRt, screenPos));
    }

    public static void SetScaledPos(this RectTransform rt, Vector2 screenPos) {
      RectTransform rootRt = rt.root.GetComponent<RectTransform>();
      Debug.Assert(null != rootRt);
      Vector2 scale = rootRt.Scale();
      rt.AddPos(new Vector2(screenPos.x / scale.x, screenPos.y / scale.y));
    }

    public static Vector2 GetScaledPos(this RectTransform rt, Vector2 screenPos) {
      RectTransform rootRt = rt.root.GetComponent<RectTransform>();
      Debug.Assert(null != rootRt);
      Vector2 scale = rootRt.Scale();
      return new Vector2(screenPos.x / scale.x, screenPos.y / scale.y);
    }

    public static Vector2 CenterRootPoint(this RectTransform rt) {
      RectTransform rootRt = rt.root.GetComponent<RectTransform>();
      Debug.Assert(null != rootRt);
      Vector2 pos = rootRt.Pos();
      Vector2 scale = rootRt.Scale();
      return new Vector2(pos.x / scale.x, pos.y / scale.y);
    }

    public static bool IsOutOfHeight(this RectTransform rt, Vector2 offsetPos) {
      //TO DO : refactor and call CenterRootPoint instead
      RectTransform rootRt = rt.root.GetComponent<RectTransform>();
      Debug.Assert(null != rootRt);

      Vector2 rootPos = rootRt.Pos();
      Vector2 rootScale = rootRt.Scale();
      Vector2 rootScaledPos = new Vector2(rootPos.x / rootScale.x, rootPos.y / rootScale.y);

      Vector2 bounds = new Vector2(rootScaledPos.x + rt.Width() / 2f, rootScaledPos.y + rt.Height() / 2f);
      Vector2 rtPos = rt.Pos() + offsetPos;
      //rtPos.x = Mathf.Abs(rtPos.x) + rt.Width();
      rtPos.y = Mathf.Abs(rtPos.y) + rt.Height();
      //check y only for now
      if (rtPos.y > bounds.y) {
        return true;
      }
      //not out of screen
      return false;
    }

    public static Vector2 OffsetToRoot(this RectTransform rt) {
      RectTransform rootRt = rt.root.GetComponent<RectTransform>();
      Debug.Assert(null != rootRt);
      RectTransform rtParent = rt.parent.GetComponent<RectTransform>();
      Vector2 offset = Vector2.zero;
      //iterate over until root
      while (rtParent != rootRt) {
        offset += rtParent.Pos();
        rtParent = rtParent.parent.GetComponent<RectTransform>();
      }
      //return
      return offset;
    }

    public static bool IsOutOfWidth(this RectTransform rt) {
      //TO DO : combine with IsOutOfHeight later
      RectTransform rootRt = rt.root.GetComponent<RectTransform>();
      Debug.Assert(null != rootRt);

      RectTransform rtParent = rt.parent.GetComponent<RectTransform>();
      float offsetX = 0f;
      //iterate over until root
      while (rtParent != rootRt) {
        offsetX += rtParent.Pos().x;
        rtParent = rtParent.parent.GetComponent<RectTransform>();
      }

      Vector2 rootPos = rootRt.Pos();
      Vector2 rootScale = rootRt.Scale();
      Vector2 rootScaledPos = new Vector2(rootPos.x / rootScale.x, rootPos.y / rootScale.y);

      Vector2 bounds = new Vector2(rootScaledPos.x + rt.Width() / 2f, rootScaledPos.y + rt.Height() / 2f);
      Vector2 rtPos = rt.Pos();
      float absX = Mathf.Abs(rtPos.x + offsetX) + rt.Width();
      if (absX > bounds.x) {
        return true;
      }
      //not out of screen's width
      return false;
    }

    public static Vector2 PivotAgainstOther(this RectTransform rt, RectTransform other) {
      Vector2 rtPos = rt.Pos();
      Vector2 otherPos = other.Pos();
      return new Vector2(rtPos.x > otherPos.x ? 1f : -1f, rtPos.y > otherPos.y ? 1f : -1f);
    }

    public static Vector2 AnchoredPosFromRoot(this RectTransform rt, Vector2 anchorMultiplier, Vector2 offset) {
      //anchored multiplier x = -1 and y = -1 means bottom left, x = 1 and y = 1 means top right, etc.
      Vector2 centerPoint = rt.CenterRootPoint();
      float w = -(rt.Width() / 2f * anchorMultiplier.x);
      float h = -(rt.Height() / 2f * anchorMultiplier.y);
      return (new Vector2(centerPoint.x * anchorMultiplier.x + w, centerPoint.y * anchorMultiplier.y + h) + offset);
    }

    public static Vector2 AnchorFromSize(this RectTransform rt, Vector2 anchor) {
      Vector2 size = rt.Size();
      size.x *= anchor.x;
      size.y *= anchor.y;
      return size;
    }
    #endregion

    public static bool IsPointInRect(this RectTransform rt, Vector2 screenPos, Camera cam) {
      return RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, cam);
    }
  }
  #endregion

  #region Image
  public static class ImageExtensions {
    public static void SetMaxSize(this Image img, float maxSize) {
      img.SetNativeSize();
      img.rectTransform.SetMaxSize(maxSize);
    }

    public static void SetColor(this Image img, float[] rgba) {
      Color color = img.color;
      for (int i = 0; i < rgba.Length; ++i) {
        color[i] = rgba[i];
      }
      img.color = color;
    }

    public static void SetColor(this Image img, Color color) {
      img.color = color;
    }

    public static void SetAlpha(this Image img, float alpha) {
      Color color = img.color;
      color.a = alpha;
      img.color = color;
    }

    public static float Alpha(this Image img) {
      return img.color.a;
    }

    public static void SetPos(this Image img, Vector2 pos) {
      img.rectTransform.SetPos(pos);
    }

    public static void SetScale(this Image img, Vector2 scale) {
      img.rectTransform.SetScale(scale);
    }

    public static void SetScale(this Image img, float scale) {
      img.rectTransform.SetScale(new Vector2(scale, scale));
    }

    public static void SetRotation(this Image img, float rot) {
      img.rectTransform.SetRotation(rot);
    }

    public static void SetParent(this Image img, RectTransform parent) {
      img.rectTransform.SetParent(parent);
    }

    public static void SetWidth(this Image img, float w) {
      img.rectTransform.SetWidth(w);
    }

    public static void SetHeight(this Image img, float h) {
      img.rectTransform.SetHeight(h);
    }
  }
  #endregion

  #region Text
  public static class TextExt {
    public static void SetColor(this Text text, float[] rgba) {
      Color color = text.color;
      for (int i = 0; i < rgba.Length; ++i) {
        color[i] = rgba[i];
      }
      text.color = color;
    }

    public static void SetAlpha(this Text txt, float alpha) {
      Color color = txt.color;
      color.a = alpha;
      txt.color = color;
    }

    public static float Alpha(this Text txt) {
      return txt.color.a;
    }
  }
  #endregion

  #region Event Trigger
  public static class EventTriggerExt {
    public static void AddEventTriggerListener(this EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> callback) {
      //check whether entry exists in trigger already
      EventTrigger.Entry entry = trigger.triggers.Find(x => x.eventID == eventType);
      //create new if not exist yet and add to the trigger
      if (null == entry) {
        entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
      }
      //if exist remove the listener first and add it back to avoid duplicate
      else {
        entry.callback.RemoveListener(callback);
        entry.callback.AddListener(callback);
      }
    }
  }
  #endregion

  #region Rect
  public static class RectExt {
    public static bool Intersects(this Rect r1, Rect r2) {
      float rightEdge1 = r1.x + r1.width * 0.5f;
      float leftEdge1 = r1.x - r1.width * 0.5f;
      float topEdge1 = r1.y + r1.height * 0.5f;
      float bottomEdge1 = r1.y - r1.height * 0.5f;

      float rightEdge2 = r2.x + r2.width * 0.5f;
      float leftEdge2 = r2.x - r2.width * 0.5f;
      float topEdge2 = r2.y + r2.height * 0.5f;
      float bottomEdge2 = r2.y - r2.height * 0.5f;

      if (rightEdge1 >= leftEdge2 &&     // r1 right edge past r2 left
        leftEdge1 <= rightEdge2 &&       // r1 left edge past r2 right
        topEdge1 >= bottomEdge2 &&       // r1 top edge past r2 bottom
        bottomEdge1 <= topEdge2) {       // r1 bottom edge past r2 top
        return true;
      }

      return false;
    }
  }
  #endregion

  #region Utils

  public class CommonButton {
    /// <summary>
    /// On Tap Tween
    /// </summary>
    Tweener _tw;

    public CommonButton(GameObject gameObject, System.Action onTap, ITween tween = null) {
      _tw = new Tweener(tween == null ? Tweener.Pulse(gameObject) : tween);
      // init tap handler
      new CTapHandler(gameObject, pos => {
        _tw.Play();
        onTap();
      });
    }

    public void Update(float dt) {
      _tw.Update(dt);
    }
  }

  #endregion
}