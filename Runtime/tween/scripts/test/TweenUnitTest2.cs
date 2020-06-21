using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public class TweenUnitTest2 : MonoBehaviour {
      CTweenChainer m_chainer;

      void Start() {
        //instantiate the chainer
        m_chainer = new CTweenChainer(TweenerType.Simultaneous);
        //add tween rotation
        m_chainer.Add(
            new CTweenRotation(
                TargetType.Image
                , gameObject
                , new RotationData(90f, 1f, Easing.OutCirc, 0.5f)
  ).SetLoop(1, Loop.Restart)
        ).PlayExistingAll(() => { print("on complete tween rotation unit test 2"); });
        //add tween scale , then play
        m_chainer.Add(
            new CTweenScale(
                TargetType.Image
                , gameObject
    , new ScaleData(new Vector2(1.1f, 1.1f), 1f, Easing.InQuad, 0.5f)
  ).SetLoop(1, Loop.Yoyo)
        ).PlayExistingAll(() => { print("on complete tween scale unit test 2"); });
      }

      void Update() {
        float dt = Time.deltaTime;
        m_chainer.Update(dt);
      }
    }
  }
}
