using UnityEngine;
using Wowsome.Tween;
using Wowsome.UI;

namespace Wowsome {
  namespace Pretend {
    public class UrlButton : MonoBehaviour {
      #region More Games URL
      public string LinkAndroid = "https://play.google.com/store/apps/details?id=com.pointclickgames.mysteryofhauntedhollow&hl=en";
      public string LinkIOS = "https://itunes.apple.com/us/app/mystery-of-haunted-hollow-point-click-escape-game/id942723145?mt=8";
      #endregion

      CGestureHandler m_tapHandler;
      CTweenChainer m_tweener;

      void Start() {
        //init tap handler
        m_tapHandler = new CGestureHandler(gameObject);
        m_tapHandler.SetTappable();
        m_tapHandler.OnTapListeners += OnTap;
        //init tweener
        m_tweener = new CTweenChainer(TweenerType.Simultaneous);
        m_tweener.Add(
            new CTweenScale(
                gameObject,
                new Vector2(1.1f, 1.1f), 1f, 0f, Easing.OutQuad).SetLoop(
                    new LoopData(-1, Loop.Yoyo)
                )
        );
        m_tweener.PlayExistingAll();
      }

      void Update() {
        m_tweener.Update(Time.deltaTime);
      }

      void OnTap(Vector2 pos) {
#if UNITY_IOS || UNITY_STANDALONE_OSX
        Application.OpenURL(LinkIOS);
#endif
#if UNITY_ANDROID
                Application.OpenURL(LinkAndroid);
#endif
      }
    }
  }
}