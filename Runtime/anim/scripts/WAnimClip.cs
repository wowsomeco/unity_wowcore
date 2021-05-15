using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  [CreateAssetMenu(fileName = "Clip", menuName = "Wowsome/Anim/Anim Clip")]
  public class WAnimClip : ScriptableObject {
    public FrameType type;
    public List<AnimFrame> frames;
  }
}