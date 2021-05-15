using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  [CreateAssetMenu(fileName = "Animation", menuName = "Wowsome/Anim/Animation")]
  public class WAnimation : ScriptableObject {
    public List<AnimStep> clips = new List<AnimStep>();
  }
}