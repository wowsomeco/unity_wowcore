using UnityEngine;
using UnityEngine.UI;

namespace Wowsome.Anim {
  public class WAnimatorImageSwap : WAnimatorStaticBase {
    public Image img;
    public Sprite sprite;
    public float maxSize;

    public override void Play() {
      img.sprite = sprite;

      if (maxSize > 0f) {
        img.SetMaxSize(maxSize);
      }
    }
  }
}

