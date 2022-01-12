using System;
using System.Collections.Generic;

namespace Wowsome.Generic {
  /// <summary>
  /// Uses for detecting value changes
  /// </summary>    
  public class WConditional<T> {
    /// <summary>
    /// The callback that gets called whenever one of the values changes during Check()
    /// </summary>    
    public Action OnChange { get; set; }
    /// <summary>
    /// The list of the value that will be observed for the changes
    /// </summary>    
    public List<WObservable<T>> Values { get; private set; }

    public WConditional(params WObservable<T>[] values) {
      Values = new List<WObservable<T>>();
      foreach (var v in values) {
        Values.Add(v);
      }
    }

    /// <summary>
    /// Checker to check the next / new values.
    /// if it's different from the current ones,
    /// then the OnChange will get triggered,
    /// as well as the Value(s) so that the observer(s) can react to the changes
    /// </summary>
    public void Check(params T[] values) {
      if (values.Length != Values.Count) return;

      for (int i = 0; i < Values.Count; ++i) {
        WObservable<T> v = Values[i];
        if (!v.Value.Equals(values[i])) {
          v.Next(values[i]);
          OnChange?.Invoke();

          break;
        }
      }
    }
  }
}

