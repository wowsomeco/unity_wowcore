using UnityEngine;
using UnityEngine.UI;

namespace Wowsome.Anim {
  [RequireComponent(typeof(Image))]
  public class WAnimatorImage : WAnimatorRT {
    public sealed class ImgInitValueHandler {
      Sprite _sprite;
      Image _img;

      public ImgInitValueHandler(Image img) {
        _img = img;
        _sprite = _img.sprite;
      }

      public void Reset() {
        _img.sprite = _sprite;
      }
    }

    public WAnimSpriteSource spriteSource;

    Image _img;
    ImgInitValueHandler _valueHandler;

    public override void InitAnimator() {
      base.InitAnimator();

      _img = GetComponent<Image>();
      _valueHandler = new ImgInitValueHandler(_img);

      _setters[FrameType.Sprite] = v => {
        int cur = Mathf.FloorToInt(v.x);
        if (cur < spriteSource.sources.Count) {
          var config = spriteSource.sources[cur];
          _img.sprite = config.sprite;
          _img.rectTransform.SetSize(config.size);
        }
      };
    }

    public override void SetInitialValue() {
      base.SetInitialValue();
      _valueHandler.Reset();
    }
  }
}

