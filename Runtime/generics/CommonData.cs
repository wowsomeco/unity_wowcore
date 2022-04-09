using System;
using UnityEngine;

namespace Wowsome {
  public delegate T Delegate<T, U>(U t);

  public class CapacityData {
    public int Max { get; private set; }
    public int Cur { get; private set; }
    public float DecimalPercentage => (float)Cur / Max;
    public float Percentage => DecimalPercentage * 100f;
    public bool IsAny => Cur > 0;
    public bool IsFull => Cur == Max;

    int _initCur;

    public CapacityData(int max) {
      Max = max;
      Cur = 0;
      _initCur = Cur;
    }

    public CapacityData(int max, int cur) {
      Max = max;
      Cur = cur;
      _initCur = Cur;
    }

    public CapacityData(CapacityData other) {
      Max = other.Max;
      Cur = other.Cur;
      _initCur = Cur;
    }

    public bool Add() {
      if (IsFull) {
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
      Cur = _initCur;
    }
  }

  [Serializable]
  public struct RangeData {
    public float min;
    public float max;

    bool _hasClamped;

    public RangeData(float def) {
      min = max = def;
      _hasClamped = false;
    }

    public RangeData(float min, float max) {
      this.min = min;
      this.max = max;
      _hasClamped = false;
    }

    public float GetRand() {
      Clamp();

      if (Mathf.Approximately(min, max)) {
        return max;
      }

      float rand = (((float)MathExtensions.GetRandom().NextDouble()) * (max - min) + min);
      return rand;
    }

    void Clamp() {
      if (_hasClamped) {
        return;
      }

      const float min = 0f;
      const float max = 1000f;

      this.min = this.min.Clamp(min, max);
      this.max = this.max.Clamp(this.min, max);

      _hasClamped = true;
    }
  }

  [Serializable]
  public class TimeData {
    public RangeData duration;
    public RangeData delay;

    public TimeData(float duration) {
      this.duration = new RangeData(duration);
      delay = new RangeData(0f);
    }

    public TimeData(float duration, float delay) {
      this.duration = new RangeData(duration);
      this.delay = new RangeData(delay);
    }

    public TimeData(RangeData duration) {
      this.duration = duration;
      delay = new RangeData(0f);
    }

    public TimeData(RangeData duration, RangeData delay) {
      this.duration = duration;
      this.delay = delay;
    }
  }

  public struct Vec2Int {
    public static Vec2Int Create(Vector3 v) {
      return new Vec2Int(v);
    }

    Vector2 _vector;

    public int X { get { return Mathf.FloorToInt(_vector.x); } }
    public int XHalf { get { return X / 2; } }
    public int Y { get { return Mathf.FloorToInt(_vector.y); } }
    public int YHalf { get { return Y / 2; } }
    public int Xy { get { return X * Y; } }
    public int Sum { get { return X + Y; } }
    public Vec2Int Half { get { return new Vec2Int(XHalf, YHalf); } }

    public Vec2Int(Vector2 v) {
      _vector = v;
    }

    public Vec2Int(int x, int y) {
      _vector = new Vector2(x, y);
    }

    public override string ToString() {
      return X + "," + Y;
    }

    public override int GetHashCode() {
      return (X << 16) | Y;
    }

    public override bool Equals(object obj) {
      if (!(obj is Vec2Int))
        return false;

      Vec2Int other = (Vec2Int)obj;
      return X == other.X && Y == other.Y;
    }

    public Vector2 ToVec2() {
      return new Vector2(X, Y);
    }
  }
}