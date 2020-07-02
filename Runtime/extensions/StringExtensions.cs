using System;

namespace Wowsome {
  public enum ComparisonType {
    Match,
    Contain,
    StartsWith,
    EndsWith,
    NotEqual
  }

  public static class StringExt {
    public static bool IsEqual(this string lhs, string rhs) {
      //check length first, for faster comparison
      if (lhs.Length == rhs.Length) {
        if (string.Compare(lhs, rhs, true) == 0) {
          return true;
        }
      }
      return false;
    }

    public static bool Matches(this string lhs, string rhs, ComparisonType type) {
      switch (type) {
        case ComparisonType.Match:
          return lhs.IsEqual(rhs);
        case ComparisonType.Contain:
          return lhs.Contains(rhs);
        case ComparisonType.StartsWith:
          return lhs.StartsWith(rhs, StringComparison.Ordinal);
        case ComparisonType.EndsWith:
          return lhs.EndsWith(rhs, StringComparison.Ordinal);
        case ComparisonType.NotEqual:
          return !lhs.IsEqual(rhs);
      }

      return false;
    }

    public static string ToUnderscore(this string str) {
      return str.Replace(" ", "_");
    }

    public static string ToUnderscoreLower(this string str) {
      return str.ToUnderscore().ToLower();
    }

    public static string Capitalize(this string str, char separator = ' ', string replaceWith = " ") {
      string capitalized = "";
      string[] splits = str.Split(separator);

      for (int i = 0; i < splits.Length; i++) {
        String s = splits[i];
        capitalized += $"{s[0].ToString().ToUpper()}{s.Substring(1)}";
        if (i < splits.Length - 1) capitalized += replaceWith;
      }

      return capitalized;
    }
  }
}
