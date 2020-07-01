namespace Wowsome {
  namespace Chrono {
    public class TimerData {
      public float Max { get; set; }
      public float Cur { get; set; }
      public float Multiplier { get; set; }

      public TimerData(float max) {
        Max = max;
        Cur = 0f;
        Multiplier = 1f;
      }

      public TimerData(float max, float multiplier) {
        Max = max;
        Multiplier = multiplier;
      }

      public TimerData(TimerData other) {
        Max = other.Max;
        Cur = other.Cur;
        Multiplier = other.Multiplier;
      }
    }

    /// <summary>
    /// Timer
    /// </summary>
    /// <description>
    /// used mainly to perform delays in the game
    /// e.g. Timer timer = new Timer(1f);
    /// call UpdateTimer() in your Update loop until it returns false, means that the timer has
    /// completed the countdown call Reset() to re use the timer once done you might want to set it
    /// to null i.e. timer = null.
    /// </description>
    public class Timer {
      TimerData m_data;

      public Timer(TimerData timerData) {
        m_data = new TimerData(timerData);
      }

      public Timer(int max) {
        m_data = new TimerData((float)max);
      }

      public Timer(float max) {
        m_data = new TimerData(max);
      }

      public void Reset() {
        m_data.Cur = 0f;
      }

      public void AddTime(float time) {
        //subtract the cur when we add more time to it
        m_data.Cur -= time;
        //clamp so it doesnt go below 0 or exceed the max time 
        m_data.Cur = m_data.Cur.Clamp(0, m_data.Max);
      }

      public float GetCurrentTime() {
        return m_data.Cur;
      }

      public float GetDeltaTime() {
        return m_data.Max - m_data.Cur;
      }

      public float GetPercentage() {
        return m_data.Cur / m_data.Max;
      }

      public bool UpdateTimer(float dT) {
        m_data.Cur += dT * m_data.Multiplier;
        if (m_data.Cur > m_data.Max) {
          return false;
        }
        return true;
      }
    }
  }
}
