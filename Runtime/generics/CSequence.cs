using System.Collections.Generic;

namespace Wowsome {
  /// @class  CSequence<T>
  ///
  /// @brief
  /// this class is geared toward a queue that has an ability to reset and add the previous
  /// sequences back in.
  ///
  /// @tparam T   Generic type parameter.
  public class CSequence<T> {
    Queue<T> m_sequences = new Queue<T>();
    List<T> m_initialSequences = new List<T>();

    public CSequence() { }

    public CSequence(IEnumerable<T> sequences) {
      AddSequences(sequences);
    }

    public void ClearAll() {
      m_sequences.Clear();
      m_initialSequences.Clear();
    }

    public List<T> GetInitialSequences() {
      return m_initialSequences;
    }

    public void AddSequence(T sequence) {
      m_initialSequences.Add(sequence);
      m_sequences.Enqueue(sequence);
    }

    public void AddSequences(IEnumerable<T> sequences) {
      m_initialSequences = new List<T>(sequences);
      ResetSequences();
    }

    public IList<T> ResetSequences() {
      m_sequences.Clear();
      for (int i = 0; i < m_initialSequences.Count; ++i) {
        m_sequences.Enqueue(m_initialSequences[i]);
      }
      return m_initialSequences;
    }

    public int Count() {
      return m_sequences.Count;
    }

    public bool Compare(T x, T y) {
      return EqualityComparer<T>.Default.Equals(x, y);
    }

    public bool IsMatchPeek(T seq) {
      if (m_sequences.Count == 0) {
        return false;
      }

      if (Compare(m_sequences.Peek(), seq)) {
        //Debug.Log("match peek");
        m_sequences.Dequeue();
        return true;
      } else {
        ResetSequences();
      }
      return false;
    }

    public T DequeueSequence() {
      if (m_sequences.Count == 0) {
        return default(T);
      }
      return m_sequences.Dequeue();
    }
  }
}
