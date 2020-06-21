using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public class TweenUnitTest1 : MonoBehaviour {
      ITween m_tweenRotation;
      ITween m_tweenMove;
      ITween m_tweenFade;
      CTweenChainer m_chainer;

      void Start() {
        //instantiate tween rotation
        m_tweenRotation = new CTweenRotation(TargetType.RectTransform, gameObject, new RotationData(50f, 0.5f))
            .SetCompleteCallback(OnCompleteRotation)
            .SetLoop(3, Loop.Yoyo);
        //instantiate tween move
        m_tweenMove = new CTweenMove(
            TargetType.RectTransform
            , gameObject
            , new MoveData(
                Vector2.zero
                , 1f
                , TweenType.To
                , Easing.Linear
                , 1f
                )).SetCompleteCallback(OnCompleteMove);
        //instantiate tween fade
        m_tweenFade = new CTweenFade(TargetType.Image, gameObject, new FadeData(0f, 1f));
        //play one at a time
        m_chainer = new CTweenChainer(TweenerType.StepByStep);
        m_chainer.Add(m_tweenRotation).Add(m_tweenMove).Add(m_tweenFade).PlayExistingAll(() => {
          print("all has done for unit test 1");
        });
      }

      void Update() {
        float dt = Time.deltaTime;
        m_chainer.Update(dt);
      }

      void OnCompleteRotation() {
        print("on complete rotation unit test 1");
      }

      void OnCompleteMove() {
        print("on complete move unit test 1");
      }
    }
  }
}

