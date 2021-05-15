using UnityEngine;
using Wowsome.Chrono;

namespace Wowsome {
  namespace Tween {
    public class TweenLerpData {
      float[] m_start;
      float[] m_end;
      TweenData m_tweenData;
      Timer m_lerpTimer = null;
      Timer m_delayTimer = null;

      public float[] Values { get; private set; }

      #region Constructors
      public TweenLerpData(float[] start, float[] end, TweenData tweenData) {
        Init(start, end, tweenData);
      }

      public TweenLerpData(float[] start, float[] end, float duration, float delay = 0f) {
        Init(
            start
            , end
            , new TweenData(new TimeData(duration, delay))
            );
      }

      public TweenLerpData(Vector2 start, Vector2 end, float duration, float delay = 0f) {
        Init(
            start.ToFloats()
            , end.ToFloats()
            , new TweenData(new TimeData(duration, delay))
            );
      }

      public TweenLerpData(float start, float end, TweenData tweenData) {
        Init(new float[] { start }, new float[] { end }, tweenData);
      }

      public TweenLerpData(float start, float end, TimeData timeData) {
        Init(new float[] { start }, new float[] { end }, new TweenData(timeData));
      }

      public TweenLerpData(float start, float end, float duration, float delay = 0f) {
        Init(
            new float[] { start }
            , new float[] { end }
            , new TweenData(new TimeData(duration, delay))
            );
      }
      #endregion

      public bool Lerping(float dt) {
        bool isLerping = false;
        //count down the delay
        if (null != m_delayTimer) {
          if (!m_delayTimer.UpdateTimer(dt)) {
            m_lerpTimer = new Timer(m_tweenData.m_timeData.duration.GetRand());
            m_delayTimer = null;
          }
          isLerping = true;
        }
        //lerp once done with the delay
        else if (null != m_lerpTimer) {
          m_lerpTimer.UpdateTimer(dt);
          float percentage = m_lerpTimer.GetPercentage();
          if (percentage > 1f) {
            m_lerpTimer = null;
          } else {
            Lerp(percentage);
            isLerping = true;
          }
        }
        //return
        return isLerping;
      }

      public void Swap() {
        //iterate over the m_start and m_end, swap them
        for (int i = 0; i < m_start.Length; ++i) {
          float temp = m_start[i];
          m_start[i] = m_end[i];
          m_end[i] = temp;
        }
      }

      public void Done() {
        m_delayTimer = null;
        m_lerpTimer = null;
        SetValues(m_end);
      }

      public void Replay() {
        //set initial values as the start
        SetValues(m_start);
        m_delayTimer = new Timer(m_tweenData.m_timeData.delay.GetRand());
        m_lerpTimer = null;
      }

      void Lerp(float percentage) {
        for (int i = 0; i < Values.Length; ++i) {
          float t = CEasings.GetEasing(percentage, m_tweenData.m_easeType);
          Values[i] = Mathf.Lerp(m_start[i], m_end[i], t);
        }
      }

      void Init(float[] start, float[] end, TweenData tweenData) {
        Debug.Assert(start.Length == end.Length, "start length has to be equal end length!");
        m_start = start;
        m_end = end;
        m_tweenData = tweenData;
        //reset on init
        Replay();
      }

      void SetValues(float[] newValues) {
        Values = new float[newValues.Length];
        for (int i = 0; i < Values.Length; ++i) {
          Values[i] = newValues[i];
        }
      }
    }

    public class TweenLerper {
      ITweenTargetData m_target;
      TweenLerpData[] m_nodes;
      LoopData m_loopData;
      int m_curLoop = 0;
      int m_delta;

      public TweenLerper(ITweenTargetData target, TweenLerpData[] nodes, LoopData loopData) {
        m_target = target;
        m_nodes = nodes;
        //set loop data
        m_loopData = loopData;
        //play
        Replay();
      }

      public bool Lerping(float dt) {
        bool isLerping = true;
        if (!m_nodes[m_target.CurIdx].Lerping(dt)) {
          SetDone();
          isLerping = ShouldLoop();
        } else {
          //update the target data on lerping
          m_target.TargetData = m_nodes[m_target.CurIdx].Values;
        }
        return isLerping;
      }

      public void SetDone() {
        //set done the cur node first
        m_nodes[m_target.CurIdx].Done();
        //update the target data afterwards
        m_target.TargetData = m_nodes[m_target.CurIdx].Values;
      }

      public void FastForward() {
        //set the cur index of the target to the last node idx
        m_target.CurIdx = m_nodes.Length - 1;
        //update the values
        SetDone();
      }

      public void Replay() {
        m_delta = 1;
        m_curLoop = m_loopData.m_loopCount;
        m_target.CurIdx = 0;
        Reset();
      }

      void Reset() {
        //do reset the nodes
        for (int i = 0; i < m_nodes.Length; ++i) {
          m_nodes[i].Replay();
        }
      }

      void Swap() {
        //do swap the nodes
        for (int i = 0; i < m_nodes.Length; ++i) {
          m_nodes[i].Swap();
        }
      }

      bool ShouldLoop() {
        m_target.CurIdx += m_delta;
        //if the index exceeds the nodes length, or less than 0, do check the looping
        if (m_target.CurIdx >= m_nodes.Length || m_target.CurIdx < 0) {
          //if dont need to loop, set done
          if (m_curLoop == 0) {
            //set cur idx back to the last default
            m_target.CurIdx = m_target.CurIdx.Clamp(0, m_nodes.Length - 1);
            //we are done
            return false;
          }
          //else check whether it's yoyo or restart
          else {
            //if yoyo, reset the delta
            if (m_loopData.m_loopType == Loop.Yoyo) {
              m_delta *= -1;
              m_target.CurIdx += m_delta;
              //swap the nodes if yoyo
              Swap();
            }
            //if it's restart loop type
            else {
              //reset the index
              m_target.CurIdx = 0;
            }
            //decrement the number of loop
            if (m_curLoop > 0) {
              --m_curLoop;
            }
            //reset the nodes if looping
            Reset();
          }
        }
        //finally
        return true;
      }
    }
  }
}
