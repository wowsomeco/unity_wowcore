using System;
using System.Collections.Generic;

namespace Wowsome {
  /// <summary>
  /// Collection of Dictionary Extensions that are currently non existent in C# Lib.
  /// </summary>
  public static class DictionaryExt {
    public delegate void Iterator<TKey, TValue>(TKey key, TValue value, int idx);

    public static void Iterate<TKey, TValue>(this IDictionary<TKey, TValue> dict, Iterator<TKey, TValue> onEach) {
      int i = 0;
      foreach (var kv in dict) {
        onEach?.Invoke(kv.Key, kv.Value, i);
        ++i;
      }
    }
  }
}