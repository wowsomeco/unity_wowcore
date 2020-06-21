using UnityEngine;

namespace Wowsome {
  public class Waypoint {
    float m_speed;

    public Vector2 Cur { get; private set; }

    public Vector2 Min { get; private set; }

    public Vector2 Max { get; private set; }

    public Vector2 Mid {
      get { return (Max + Min) / 2f; }
    }

    public float Progress { get; set; }

    public Waypoint(Vector2 start, Vector2 end, float speed) {
      Min = start;
      Max = end;
      m_speed = speed;
      Progress = 0f;
    }

    public bool Moving(float dt, bool isBackwards = false) {
      Progress += m_speed * (isBackwards ? -2f : 1f) * dt;
      Cur = Vector2.Lerp(Min, Max, Progress);
      bool isMoving = (Progress <= 1f && Progress > 0f);
      Progress = Progress.Clamp(0f, 1f);
      return isMoving;
    }

    public void Reset() {
      Cur = Min;
      Progress = 0f;
    }
  }

  public class CPathwayable {
    Waypoint[] m_wayPoints;
    int m_curIdx = 0;

    public Vector2 Cur { get; private set; }

    public CPathwayable(Vector2[] points, float speed) {
      Debug.Assert(points.Length > 1, "points array has to be more than 1!");
      int count = points.Length - 1;
      m_wayPoints = new Waypoint[count];
      for (int i = 0; i < count; ++i) {
        m_wayPoints[i] = new Waypoint(points[i], points[i + 1], speed);
      }
    }

    public bool Moving(float dt, bool isBackwards = false) {
      bool isMoving = m_wayPoints[m_curIdx].Moving(dt, isBackwards);
      //update the pos
      Cur = m_wayPoints[m_curIdx].Cur;
      //if the cur index waypoint has reached max, set next
      if (!isMoving) {
        if (isBackwards && m_curIdx == 0) {
          return false;
        } else if (!isBackwards && m_curIdx == m_wayPoints.Length - 1) {
          return false;
        }

        int delta = isBackwards ? -1 : 1;
        m_curIdx += delta;
        m_curIdx = m_curIdx.Clamp(0, m_wayPoints.Length - 1);
      }
      //return
      return true;
    }

    public void Reset() {
      m_curIdx = 0;
      for (int i = 0; i < m_wayPoints.Length; ++i) {
        m_wayPoints[i].Reset();
      }
    }

    public void SetProgress(Vector2 curPos) {
      Reset();
      Waypoint nearestPoint = GetNearestPoint(curPos);
      float distToMin = Vector2.Distance(curPos, nearestPoint.Min);
      float minToMax = Vector2.Distance(nearestPoint.Min, nearestPoint.Max);
      m_wayPoints[m_curIdx].Progress = (distToMin / minToMax).Clamp(0f, 1f);
    }

    Waypoint GetNearestPoint(Vector2 curPos) {
      m_curIdx = 0;
      Waypoint point = m_wayPoints[m_curIdx];
      float nearest = Vector2.Distance(curPos, point.Mid);

      for (int i = 0; i < m_wayPoints.Length; ++i) {
        float candidate = Vector2.Distance(curPos, m_wayPoints[i].Mid);

        if (candidate < nearest) {
          //we got a new nearest dist
          nearest = candidate;
          point = m_wayPoints[i];
          m_curIdx = i;
        }
      }

      return point;
    }
  }
}
