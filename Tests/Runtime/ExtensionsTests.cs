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

      str1 = "Abc Def";
      Assert.AreEqual(str1.ToUnderscoreLower(), "abc_def");

      str1 = "abc abc";
      Assert.AreEqual(str1.Capitalize(), "Abc Abc");
      str1 = "abc_def";
      Assert.AreEqual(str1.Capitalize('_'), "Abc Def");
      str1 = "abc abc";
      Assert.AreEqual(str1.Capitalize(' ', ","), "Abc,Abc");

      str1 = "Abc Def";
      // replaces the string with '-' 3 times and check whether the string now contains exactly 3 '-'
      Assert.AreEqual(str1.ReplaceRandom(3, '-').CountChar('-'), 3);

      yield return null;
    }

    [UnityTest]
    public IEnumerator TestListExtensions() {
      // shuffle test
      List<string> strings1 = new List<string>() { "a", "b", "c" };
      List<string> strings2 = new List<string>(strings1);
      strings2.Shuffle();
      Assert.AreEqual(strings1.IsEqual(strings2), false);

      // map test
      List<Vector2> vecs = new List<Vector2>() {
        new Vector2(10f, 20f),
        new Vector2(20f, 30f),
        new Vector2(30f, 40f),
      };

      List<float> toFloats = vecs.Map(item => item.x + item.y);
      toFloats.Loop((item, i) => {
        switch (i) {
          case 0:
            Assert.AreEqual(Mathf.Approximately(item, 30f), true);
            break;
          case 1:
            Assert.AreEqual(Mathf.Approximately(item, 50f), true);
            break;
          case 2:
            Assert.AreEqual(Mathf.Approximately(item, 70f), true);
            break;
          default:
            break;
        }
      });

      // fold test
      float sumX = vecs.Fold(0f, (prev, cur) => prev += cur.x);
      float sumY = vecs.Fold(0f, (prev, cur) => prev += cur.y);
      Assert.AreEqual(Mathf.Approximately(sumX, 60f), true);
      Assert.AreEqual(Mathf.Approximately(sumY, 90f), true);

      yield return null;
    }

    [UnityTest]
    public IEnumerator TestVec2Extensions() {
      // clamp & equality test
      Vector2 v1 = new Vector2(10f, 20f);
      v1 = v1.ClampVec2(Vector2.zero, Vector2.one);
      Assert.AreEqual(v1.IsEqual(Vector2.one), true);

      // aspect ratio test
      Vector2 v = new Vector2(320f, 160f);
      v = v.AspectRatio(160f);
      Assert.AreEqual(v.IsEqual(new Vector2(160f, 80f)), true);

      yield return null;
    }
  }
}