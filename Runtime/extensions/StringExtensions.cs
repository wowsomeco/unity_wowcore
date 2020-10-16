using System;
using System.Collections.Generic;

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
        capitalized += Concat(s[0].ToString().ToUpper(), s.Substring(1));
        if (i < splits.Length - 1) capitalized += replaceWith;
      }

      return capitalized;
    }

    public static string Concat(params string[] str) {
      string concat = string.Empty;

      foreach (string s in str) {
        concat += s;
      }

      return concat;
    }

    /// <summary>
    /// Shuffles the order of the char in the string defined as param.
    /// </summary>
    /// <param name="str">The origin string to shuffle</param>
    /// <example>
    /// <code>
    /// string str = "string1";
    /// str = str.Shuffle();
    /// </code>
    /// </example>
    public static string Shuffle(this string str) {
      char[] array = str.ToCharArray();
      Random rng = new Random();
      int n = array.Length;
      while (n > 1) {
        n--;
        int k = rng.Next(n + 1);
        var value = array[k];
        array[k] = array[n];
        array[n] = value;
      }
      return new string(array);
    }

    /// <summary>
    /// Splits a string separated with the separator defined as the param. 
    /// </summary>
    /// <param name="str">The origin of the string</param>
    /// <param name="separator">Separator</param>
    /// <returns>A new string that has been shuffled</returns>
    /// <example>
    /// <code>
    /// string str = "abc def";
    /// str.SplitAndShuffle();
    /// </code>
    /// </example>
    public static string SplitAndShuffle(this string str, char separator = ' ') {
      string newStr = "";

      string[] splits = str.Split(separator);
      for (int i = 0; i < splits.Length; ++i) {
        string s = splits[i];
        newStr += s.Shuffle();
        if (i < splits.Length - 1) newStr += separator;
      }

      return newStr;
    }

    /// <summary>
    /// Replaces a string with defined random char count times.
    /// </summary>
    /// <param name="str">The original string</param>
    /// <param name="count">How total amount of the char in string to be replaced</param>
    /// <param name="with">The new char to replace with</param>    
    public static string ReplaceRandom(this string str, int count, char with) {
      char[] chars = str.ToCharArray();

      HashSet<int> rands = MathExtensions.GetUniqueRandomValue(0, str.Length, count);
      foreach (int idx in rands) {
        chars[idx] = with;
      }

      string newStr = "";
      foreach (char c in chars) {
        newStr += c;
      }

      return newStr;
    }

    /// <summary>
    /// Gets how many times a certain char appears in a string
    /// </summary>
    /// <param name="str">The original string</param>
    /// <param name="ch">The character to investigate</param>    
    public static int CountChar(this string str, char ch) {
      int count = 0;

      for (int i = 0; i < str.Length; ++i) {
        if (str[i] == ch) ++count;
      }

      return count;
    }

    /// <summary>
    /// Checks whether the string is empty
    /// </summary>
    public static bool IsEmpty(this string str) {
      return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// Returns the last character from a string.
    /// </summary>
    public static string Last(this string str) {
      if (str.IsEmpty()) return null;
      return str[str.Length - 1].ToString();
    }

    /// <summary>
    /// Returns the numbers that might exist in a string.
    /// </summary>
    public static int ExtractNumbers(this string str) {
      string num = string.Empty;
      foreach (char ch in str) {
        if (char.IsNumber(ch)) {
          num += ch;
        }
      }
      return num.IsEmpty() ? -1 : int.Parse(num);
    }
  }
}
