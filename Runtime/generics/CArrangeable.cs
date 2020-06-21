using System.Collections.Generic;
using UnityEngine;

namespace Wowsome {
  namespace Generic {
    public interface IArrangeable {
      int ArrangeIndex { get; set; }
      bool IsArranging { get; set; }
      RectTransform ArrangeTransform { get; }
      float DeltaArrange { get; set; }
    }

    public sealed class CArrangeable {
      List<IArrangeable> m_arrangeUnits;
      Vector2 m_anchorPos;

      public CArrangeable(ICollection<IArrangeable> units) {
        //add the unit to the list
        m_arrangeUnits = new List<IArrangeable>(units);
        //set the anchor pos
        RectTransform firstUnit = m_arrangeUnits[0].ArrangeTransform;
        Vector2 anchorPos = firstUnit.Pos();
        anchorPos.x -= firstUnit.Width() * 0.5f;
        m_anchorPos = anchorPos;
      }

      public void UpdateArrangeable(float dt) {
        //move the units that has true IsArranging flag
        for (int i = 0; i < m_arrangeUnits.Count; ++i) {
          if (m_arrangeUnits[i].IsArranging) {
            m_arrangeUnits[i].DeltaArrange += dt * 2f;
            if (m_arrangeUnits[i].DeltaArrange < 1f) {
              int idx = m_arrangeUnits[i].ArrangeIndex;
              m_arrangeUnits[i].ArrangeTransform.SetPos(
                Vector2.Lerp(m_arrangeUnits[i].ArrangeTransform.Pos(), GetToPos(idx), m_arrangeUnits[i].DeltaArrange)
              );
            } else {
              m_arrangeUnits[i].IsArranging = false;
              m_arrangeUnits[i].DeltaArrange = 0f;
            }
          }
        }
      }

      public void OnArrangeStart(IArrangeable unit) {
        //get the unit index
        int curIdx = unit.ArrangeIndex;
        //add the one before and/or after the unit index
        HashSet<int> checkedIdxs = new HashSet<int>();
        if (curIdx - 1 >= 0) {
          checkedIdxs.Add(curIdx - 1);
        }
        if (curIdx + 1 < m_arrangeUnits.Count) {
          checkedIdxs.Add(curIdx + 1);
        }
        //get the rect transform of the unit
        RectTransform unitTransform = unit.ArrangeTransform;
        //loop the idxs that should be checked
        foreach (int checkedIdx in checkedIdxs) {
          IArrangeable checkedUnit = m_arrangeUnits.Find(x => x.ArrangeIndex == checkedIdx);
          if (null != checkedUnit) {
            //skip the unit that is being arranged already
            if (checkedUnit.IsArranging) {
              continue;
            }
            RectTransform checkedTransform = checkedUnit.ArrangeTransform;
            //check the x pos of the unit against the checked one
            if ((unitTransform.Pos().x > checkedTransform.Pos().x && checkedIdx > curIdx) ||
              (unitTransform.Pos().x < checkedTransform.Pos().x && checkedIdx < curIdx)
            ) {
              //swap index
              int tempIdx = unit.ArrangeIndex;
              unit.ArrangeIndex = checkedUnit.ArrangeIndex;
              checkedUnit.ArrangeIndex = tempIdx;
              //set should move
              checkedUnit.IsArranging = true;
            }
          }
        }
        //sort
        m_arrangeUnits.Sort((x, y) => x.ArrangeIndex.CompareTo(y.ArrangeIndex));
      }

      public void OnArrangeEnd(IArrangeable unit) {
        unit.IsArranging = true;
      }

      Vector2 GetToPos(int idx) {
        //set the initial pos
        Vector2 toPos = m_anchorPos;
        //idx is inclusive
        for (int i = 0; i <= idx; ++i) {
          if (i == idx) {
            toPos.x += m_arrangeUnits[i].ArrangeTransform.Width() * 0.5f;
          } else {
            toPos.x += m_arrangeUnits[i].ArrangeTransform.Width();
          }
        }
        //return the pos
        return toPos;
      }
    }
  }
}
