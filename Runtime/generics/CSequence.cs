using System.Collections.Generic;

namespace Wowsome {
  /// <summary>
  /// CSequence<T>
  /// </summary>
  /// <description>
  /// this class is geared toward a queue that has an ability to reset and add the previous
  /// sequences back in.
  /// </description>
  /// <typeparam name="T">Generic type parameter</typeparam>
  public class CSequence<T> {
    Queue<T> _sequences = new Queue<T>();
    List<T> _initialSequences = new List<T>();

    public CSequence() { }

    public CSequence(IEnumerable<T> sequences) {
      AddSequences(sequences);
    }

    public void ClearAll() {
      _sequences.Clear();
      _initialSequences.Clear();
    }

    public List<T> GetInitialSequences() {
      return _initialSequences;
    }

    public void AddSequence(T sequence) {
      _initialSequences.Add(sequence);
      _sequences.Enqueue(sequence);
    }

    public void AddSequences(IEnumerable<T> sequences) {
      _initialSequences = new List<T>(sequences);
      ResetSequences();
    }

    public IList<T> ResetSequences() {
      _sequences.Clear();
      for (int i = 0; i < _initialSequences.Count; ++i) {
        _sequences.Enqueue(_initialSequences[i]);
      }
      return _initialSequences;
    }

    public int Count() {
      return _sequences.Count;
    }

    public bool Compare(T x, T y) {
      return EqualityComparer<T>.Default.Equals(x, y);
    }

    public bool MatchesPeek(T seq) {
      if (_sequences.Count == 0) {
        return false;
      }

      if (Compare(_sequences.Peek(), seq)) {
        _sequences.Dequeue();
        return true;
      } else {
        ResetSequences();
      }
      return false;
    }

    public T DequeueSequence() {
      if (_sequences.Count == 0) {
        return default(T);
      }
      return _sequences.Dequeue();
    }
  }
}
