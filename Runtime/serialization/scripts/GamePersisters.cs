using UnityEngine;
using Wowsome.Core;

namespace Wowsome {
  namespace Serialization {
    public class GamePersisters : MonoBehaviour, ISystem {
      IPersister[] m_persisters;
      IDataSerializer m_serializer = new JsonDataSerializer();

      #region ISystem implementation

      public void InitSystem() {
        //get all the persisters from this object
        m_persisters = GetComponentsInChildren<IPersister>(true);
        //init them
        for (int i = 0; i < m_persisters.Length; ++i) {
          m_persisters[i].Init(m_serializer);
        }
        //start them
        for (int i = 0; i < m_persisters.Length; ++i) {
          m_persisters[i].StartPersister(this);
        }
      }

      public void StartSystem(WEngine gameEngine) { }

      public void UpdateSystem(float dt) { }

      #endregion

      public T GetPersister<T>() where T : class, IPersister {
        for (int i = 0; i < m_persisters.Length; ++i) {
          T persister = m_persisters[i] as T;
          if (null != persister) {
            return persister;
          }
        }
        return null;
      }

      public void SaveAll() {
        for (int i = 0; i < m_persisters.Length; ++i) {
          m_persisters[i].Save(m_serializer);
        }
      }

      public void ResetPersisters() {
        for (int i = 0; i < m_persisters.Length; ++i) {
          m_persisters[i].Reset(m_serializer);
        }
      }
    }
  }
}

