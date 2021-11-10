using UnityEngine;

namespace Wowsome.Tween {
  public class CTween<T> where T : ITweenTargetData, new() {
    ITweenTarget<T> m_target;
    T m_values = new T();
    TweenLerper m_lerper = null;
    bool m_isPaused = false;

    public string Id { get; private set; }

    public float[] Values {
      get {
        return m_target.GetTargetData(m_values).TargetData;
      }
      set {
        m_values.TargetData = value;
        m_target.SetTargetData(m_values);
      }
    }

    public bool IsPlaying {
      get { return null != m_lerper; }
    }

    #region Constructors			
    public CTween(TargetType type, GameObject go, string id) {
      //instantiate the tween target factory
      TweenTargetFactory<T> factory = new TweenTargetFactory<T>();
      //create target based on the type
      m_target = factory.Create(type, go);
      Id = id;
    }

    public CTween(ITweenTarget<T> target, string id) {
      m_target = target;
      Id = id;
    }

    public CTween(ITweenTarget<T> target, TweenLerpData[] lerpData, LoopData loopData) {
      m_target = target;
      //play immediate
      Play(lerpData, loopData);
    }

    public CTween(ITweenTarget<T> target, TweenLerpData[] lerpData) {
      m_target = target;
      //play immediate
      Play(lerpData, new LoopData(0));
    }

    public CTween(ITweenTarget<T> target, TweenLerpData lerpData) {
      m_target = target;
      //play immediate
      Play(new TweenLerpData[] { lerpData }, new LoopData(0));
    }
    #endregion

    #region Public Methods
    public void Play(TweenLerpData[] lerpData, LoopData loopData) {
      //stop first if it's already playing
      if (IsPlaying) {
        Stop();
      }
      //instantiate the lerper
      m_lerper = new TweenLerper(m_values, lerpData, loopData);
    }

    public void Play(TweenLerpData lerpData, LoopData loopData) {
      Play(new TweenLerpData[] { lerpData }, loopData);
    }

    public void Pause() {
      m_isPaused = true;
    }

    public void Resume() {
      m_isPaused = false;
    }

    public void Stop() {
      if (IsPlaying) {
        //set lerper to null
        m_lerper = null;
      }
    }

    public void FastForward() {
      if (IsPlaying) {
        m_lerper.FastForward();
        //update the values
        SetValues();
        //stop
        Stop();
      }
    }
    #endregion

    public bool UpdateTween(float dt) {
      //bail with true if paused
      if (m_isPaused) {
        return true;
      }
      //check nullability of the lerper
      if (null != m_lerper) {
        if (m_lerper.Lerping(dt)) {
          //update the target values
          SetValues();
          return true;
        } else {
          //set done first
          m_lerper.SetDone();
          //update the values
          SetValues();
          //remove lerper if no longer updating
          Stop();
        }
      }
      //return false when done
      return false;
    }

    void SetValues() {
      m_target.SetTargetData(m_values);
    }
  }
}
