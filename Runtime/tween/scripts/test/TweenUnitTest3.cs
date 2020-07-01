using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  namespace Tween {
    public class TweenUnitTest3 : MonoBehaviour {
      ITween m_tweenRotation;
      CTweenMove m_tweenMove;
      CTweenColor m_tweenColor;
      ITween m_tweenFade;
      List<ITween> m_tweens = new List<ITween>();

      void Start() {
        //instantiate the tween rotation and start immediately
        m_tweenRotation = new CTweenRotation(
            gameObject
            , 30f
            , 1f
            , Easing.OutBounce
            ).SetLoop(3, Loop.Yoyo).Start().SetCompleteCallback(OnCompleteRotation);

        //instantiate the tween move but dont play it yet
        m_tweenMove = new CTweenMove(
            gameObject
            , new MoveData[] {
                        new MoveData(new Vector2(0f, 300f), 0.5f, TweenType.By, Easing.InQuad)
                        , new MoveData(new Vector2(500f, 0f), 1f, TweenType.By, Easing.InOutCubic, 1f)
                        , new MoveData(new Vector2(0f, -100f), 0.5f, TweenType.By, Easing.OutBounce)
            });

        //instatiate the tween color but dont play it yet
        m_tweenColor = new CTweenColor(
            gameObject
            , Color.magenta
            , 1f
            , 0.5f);

        //instantiate the tween fade but dont play it yet
        m_tweenFade = new CTweenFade(
            gameObject
            , 0.5f
            , 1f
            , 1f
            , Easing.OutExpo
            );

        Play();
      }

      void Update() {
        float dt = Time.deltaTime;
        //update them all
        for (int i = 0; i < m_tweens.Count; ++i) {
          m_tweens[i].UpdateTween(dt);
        }
      }

      void OnCompleteRotation() {
        //execute on complete with lambda to play the tween color once moved
        m_tweenMove.Start().SetCompleteCallback(() => {
          m_tweenFade.Play();
          m_tweenColor.Start().SetCompleteCallback(() => {
            Vector2 pos = GetComponent<RectTransform>().Pos();
            print(string.Format("expect move to = (500,200) , has moved to current location x = {0} and y = {1}", pos.x, pos.y));
          });
        });
      }

      void Play() {
        //add them to the list on play
        m_tweens.Add(m_tweenRotation);
        m_tweens.Add(m_tweenMove);
        m_tweens.Add(m_tweenColor);
        m_tweens.Add(m_tweenFade);
      }

      public void Stop() {
        for (int i = 0; i < m_tweens.Count; ++i) {
          m_tweens[i].Stop();
        }
        m_tweens.Clear();
      }
    }
  }
}
