using System;
using UnityEngine;

namespace Wowsome {
  public class CapacityData {
    public int Max { get; private set; }
    public int Cur { get; private set; }

    int m_initCur;

    public CapacityData(int max) {
      Max = max;
      Cur = 0;
      m_initCur = Cur;
    }

    public CapacityData(int max, int cur) {
      Max = max;
      Cur = cur;
      m_initCur = Cur;
    }

    public CapacityData(CapacityData other) {
      Max = other.Max;
      Cur = other.Cur;
      m_initCur = Cur;
    }

    public bool IsAny() {
      return Cur > 0;
    }

    public bool IsFull() {
      return Cur == Max;
    }

    public bool Add() {
      if (IsFull()) {
        return false;
      }
      ++Cur;
      return true;
    }

    public bool Remove() {
      --Cur;
      if (Cur < 0) {
        Cur = 0;
        return false;
      }
      return true;
    }

    public void Reset() {
      Cur = m_initCur;
    }
  }

  [Serializable]
  public struct ImageData {
    public string m_spriteName;
    public Vector2 m_pos;
    public Vector2 m_size;
    public float m_rotation;

    public ImageData(
      string spriteName
      , Vector2 pos
      , Vector2 size
      , float rot
    ) {
      m_spriteName = spriteName;
      m_pos = pos;
      m_size = size;
      m_rotation = rot;
    }

    public ImageData(string spriteName, ImageData other) {
      m_spriteName = spriteName;
      m_pos = other.m_pos;
      m_size = other.m_size;
      m_rotation = other.m_rotation;
    }
  }

  [Serializable]
  public class SpriteData {
    public string m_id;
    public Sprite m_sprite;

    public SpriteData(string id, Sprite sprite) {
      m_id = id;
      m_sprite = sprite;
    }
  }

  [Serializable]
  public struct RangeData {
    public float m_min;
    public float m_max;

    bool m_hasClamped;

    public RangeData(float def) {
      m_min = m_max = def;
      m_hasClamped = false;
    }

    public RangeData(float min, float max) {
      m_min = min;
      m_max = max;
      m_hasClamped = false;
    }

    public float GetRand() {
      Clamp();

      if (Mathf.Approximately(m_min, m_max)) {
        return m_max;
      }

      float rand = (((float)MathExtensions.GetRandom().NextDouble()) * (m_max - m_min) + m_min);
      return rand;
    }

    void Clamp() {
      if (m_hasClamped) {
        return;
      }

      const float min = 0f;
      const float max = 1000f;

      m_min = m_min.Clamp(min, max);
      m_max = m_max.Clamp(m_min, max);

      m_hasClamped = true;
    }
  }

  [Serializable]
  public class TimeData {
    public RangeData m_duration;
    public RangeData m_delay;

    public TimeData(float duration) {
      m_duration = new RangeData(duration);
      m_delay = new RangeData(0f);
    }

    public TimeData(float duration, float delay) {
      m_duration = new RangeData(duration);
      m_delay = new RangeData(delay);
    }

    public TimeData(RangeData duration) {
      m_duration = duration;
      m_delay = new RangeData(0f);
    }

    public TimeData(RangeData duration, RangeData delay) {
      m_duration = duration;
      m_delay = delay;
    }
  }

  [Serializable]
  public struct GridData {
    public int m_row;
    public int m_col;

    public GridData(int r, int c) {
      m_row = r;
      m_col = c;
    }

    public bool IsEqual(GridData other) {
      return m_row == other.m_row && m_col == other.m_col;
    }

    public int this[int idx] {
      get { return idx == 0 ? m_row : m_col; }
      set {
        if (idx == 0) {
          m_row = value;
        } else {
          m_col = value;
        }
      }
    }
  }
}