using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Wowsome;

namespace Tests {
  public class ExtensionsTests {
    [UnityTest]
    public IEnumerator TestStringExtensions() {
      string str1 = "abc";
      string str2 = "abc";

      Assert.AreEqual(str1.Matches(str2, ComparisonType.Match), true);
      Assert.AreEqual(str1.IsEqual(str2), true);

      str2 = "cba";
      Assert.AreEqual(str1.Matches(str2, ComparisonType.Match), false);

      yield return null;
    }

    [UnityTest]
    public IEnumerator TestListExtensions() {
      // shuffle test
      List<string> strings1 = new List<string>() { "a", "b", "c" };
      List<string> strings2 = new List<string>(strings1);
      strings2.Shuffle();
      Assert.AreEqual(strings1.IsEqual(strings2), false);

      yield return null;
    }

    [UnityTest]
    public IEnumerator TestVec2Extensions() {
      // clamp & equality test
      Vector2 v1 = new Vector2(10f, 10f);
      v1 = v1.ClampVec2(Vector2.zero, Vector2.one);
      Assert.AreEqual(v1.IsEqual(Vector2.one), true);

      yield return null;
    }
  }
}