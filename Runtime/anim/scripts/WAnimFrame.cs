using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  [Serializable]
  public enum WFrameType { Position, Scale, Rotation }

  [Serializable]
  public class WAnimCallback {
    public int percent;
    public List<WAnimFrameNode> frames = new List<WAnimFrameNode>();
  }

  [Serializable]
  public class WAnimFrame {
    public WFrameType type;
    public string from;
    public string to;
    public Timing timing;
    public List<WAnimCallback> callbacks = new List<WAnimCallback>();

    public WAnimFrame(WAnimFrameNode node) {
      type = node.type;
      from = node.from;
      to = node.to;
      timing = node.timing;
    }

    public InterpolationVec GetInterpolationVec() {
      return new InterpolationVec(timing, from.ToVec2(), to.ToVec2());
    }
  }

  [Serializable]
  public class WAnimFrameNode {
    public WFrameType type;
    public string from;
    public string to;
    public Timing timing;

    public InterpolationVec GetInterpolationVec() {
      return new InterpolationVec(timing, from.ToVec2(), to.ToVec2());
    }
  }

  public static class Extensions {
    public static Vector2 ToVec2(this string v) {
      var split = v.Trim().Split(',');
      return new Vector2(float.Parse(split[0]), split.Length > 1 ? float.Parse(split[1]) : 0f);
    }
  }
}