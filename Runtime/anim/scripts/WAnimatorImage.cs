using UnityEngine;
using UnityEngine.UI;

namespace Wowsome.Anim {
  public sealed class WAnimatorImage : WAnimatorRT {
    public sealed class ImgInitValueHandler {
      Image _img;
      Sprite _sprite;
      float _alpha;

      public ImgInitValueHandler(Image img) {
        _img = img;
        _sprite = _img.sprite;
        _alpha = _img.Alpha();
      }

      public void Reset() {
        _img.sprite = _sprite;
        _img.SetAlpha(_alpha);
      }
    }

    public WAnimSpriteSource spriteSource;

    Image _img;
    ImgInitValueHandler _valueHandler;

    public override void InitAnimator() {
      base.InitAnimator();

      _img = otherTarget?.GetComponent<Image>() ?? GetComponent<Image>();
      Assert.Null<Image, WAnimatorImage>(_img, gameObject);

      _valueHandler = new ImgInitValueHandler(_img);

      _setters[FrameType.Sprite] = v => {
        int cur = Mathf.FloorToInt(v.x);
        if (cur < spriteSource.sources.Count) {
          var config = spriteSource.sources[cur];
          _img.sprite = config.sprite;
          _img.rectTransform.SetSize(config.size);
        }
      };
      _setters[FrameType.Alpha] = v => {
        _img.SetAlpha(v[0]);
      };

      _getters[FrameType.Alpha] = () => _img.Alpha().ToVector2();
    }

    public override void SetInitialValue() {
      base.SetInitialValue();
      _valueHandler.Reset();
    }
  }
}

