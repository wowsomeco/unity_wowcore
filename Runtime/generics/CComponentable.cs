using System.Collections.Generic;

namespace Wowsome {
  namespace Generic {
    public class CComponentable<T> where T : class {
      bool m_allowDuplicate = false;

      public List<T> Components { get; private set; }

      public CComponentable(bool allowDuplicate) {
        Components = new List<T>();
        m_allowDuplicate = allowDuplicate;
      }

      public bool AddComponent(T comp) {
        //if it doesnt allow duplicate and the list contains the component, bail
        if (!m_allowDuplicate && Components.Contains(comp)) {
          return false;
        }
        //finally add it to the list
        Components.Add(comp);
        return true;
      }

      public void AddComponents(IEnumerable<T> comps) {
        foreach (T comp in comps) {
          AddComponent(comp);
        }
      }

      public bool RemoveComponent(T comp) {
        return Components.Remove(comp);
      }

      public ST GetComponent<ST>() where ST : class, T {
        for (int i = 0; i < Components.Count; ++i) {
          ST sT = Components[i] as ST;
          if (null != sT) {
            return sT;
          }
        }
        return null;
      }

      public HashSet<ST> GetComponents<ST>() where ST : class, T {
        HashSet<ST> m_specificTypes = new HashSet<ST>();
        for (int i = 0; i < Components.Count; ++i) {
          ST sT = Components[i] as ST;
          if (null != sT) {
            m_specificTypes.Add(sT);
          }
        }
        return m_specificTypes;
      }
    }
  }
}
