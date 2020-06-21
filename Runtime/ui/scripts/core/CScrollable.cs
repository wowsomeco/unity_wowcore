using UnityEngine;

namespace Wowsome {
  namespace UI {
    public class CScrollable {
      RectTransform m_target;
      int m_axisInt;
      Vector2 m_limit;
      Vector2 m_velocity;
      Vector2 m_prevPos;
      float m_decelerationRate = 0.0001f;
      bool m_scrolling;

      public CScrollable(RectTransform target, SwipeAxis axis, Vector2 limit) {
        //get the target
        m_target = target;
        //set axis and limit
        m_axisInt = (int)axis;
        m_limit = limit;
      }

      public void UpdateScroll(float dt) {
        //inertia
        if (m_scrolling) {
          Vector2 newVelo = (m_target.Pos() - m_prevPos) / dt;
          m_velocity = Vector2.Lerp(m_velocity, newVelo, dt * 2f);
        } else {
          Vector2 pos = m_target.Pos();
          m_velocity[m_axisInt] *= Mathf.Pow(m_decelerationRate, dt);
          if (Mathf.Abs(m_velocity[m_axisInt]) < 1) {
            m_velocity[m_axisInt] = 0;
          }
          pos[m_axisInt] += m_velocity[m_axisInt] * dt;
          pos[m_axisInt] = pos[m_axisInt].Clamp(m_limit.x, m_limit.y);
          m_target.SetPos(pos);
        }

        m_prevPos = m_target.Pos();
      }

      public void OnBeginScroll() {
        m_scrolling = true;
      }

      public void OnScroll(Vector2 delta) {
        Vector2 pos = m_target.Pos();
        pos[m_axisInt] += m_target.GetScaledPos(delta)[m_axisInt];
        pos[m_axisInt] = pos[m_axisInt].Clamp(m_limit.x, m_limit.y);
        m_target.SetPos(pos);
      }

      public void OnEndScroll() {
        m_scrolling = false;
      }
    }
  }
}