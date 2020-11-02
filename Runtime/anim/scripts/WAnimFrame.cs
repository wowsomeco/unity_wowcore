using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  [Serializable]
  public enum WFrameType { Position, Scale, Rotation, Pivot }

  [Serializable]
  public class WAnimCallback {
    public int percent;
    public List<WAnimFrameNode> frames = new List<WAnimFrameNode>();
  }

  [Serializable]
  public class WAnimFrame {
    public WFrameType type;
    public List<float> to;
    public Timing timing;
    public List<WAnimCallback> progressCallbacks = new List<WAnimCallback>();

    public WAnimFrame(WAnimFrameNode node) {
      type = node.type;
      to = node.to;
      timing = node.timing;
    }
  }

  [Serializable]
  public class WAnimFrameNode {
    public WFrameType type;
    public List<float> to;
    public Timing timing;
  }

  public static class Extensions {
    public static Vector2 ToVec2(this List<float> v) {
      return new Vector2(v[0], v.Count > 1 ? v[1] : 0f);
    }
  }
}