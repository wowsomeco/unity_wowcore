using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wowsome.Anim {
  [CreateAssetMenu(fileName = "SpriteSource", menuName = "Wowsome/Anim/Sprite Source")]
  public class WAnimSpriteSource : ScriptableObject {
    [Serializable]
    public class SpriteConfig {
      public Sprite sprite;
      public Vector2 size;
    }

    public List<SpriteConfig> sources = new List<SpriteConfig>();
  }
}