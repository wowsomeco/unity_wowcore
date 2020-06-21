using UnityEngine;
using System;

namespace Wowsome {
  namespace Tween {
    [Serializable]
    public enum Easing {
      Linear,
      InQuad,
      OutQuad,
      InOutQuad,
      InCubic,
      OutCubic,
      InOutCubic,
      InQuart,
      OutQuart,
      InOutQuart,
      InExpo,
      OutExpo,
      InOutExpo,
      InCirc,
      OutCirc,
      InOutCirc,
      InQuint,
      OutQuint,
      InOutQuint,
      InBounce,
      OutBounce,
      InOutBounce,
      InBack,
      OutBack,
      InOutBack
    }

    public class CEasings {
      public static float GetEasing(float t, Easing easeType) {
        float result = t;

        switch (easeType) {
          case Easing.InQuad:
            result = Quadratic.In(t);
            break;
          case Easing.OutQuad:
            result = Quadratic.Out(t);
            break;
          case Easing.InOutQuad:
            result = Quadratic.InOut(t);
            break;
          case Easing.InCubic:
            result = Cubic.In(t);
            break;
          case Easing.OutCubic:
            result = Cubic.Out(t);
            break;
          case Easing.InOutCubic:
            result = Cubic.InOut(t);
            break;
          case Easing.InQuart:
            result = Quartic.In(t);
            break;
          case Easing.OutQuart:
            result = Quartic.Out(t);
            break;
          case Easing.InOutQuart:
            result = Quartic.InOut(t);
            break;
          case Easing.InExpo:
            result = Exponential.In(t);
            break;
          case Easing.OutExpo:
            result = Exponential.Out(t);
            break;
          case Easing.InOutExpo:
            result = Exponential.InOut(t);
            break;
          case Easing.InCirc:
            result = Circular.In(t);
            break;
          case Easing.OutCirc:
            result = Circular.Out(t);
            break;
          case Easing.InOutCirc:
            result = Circular.InOut(t);
            break;
          case Easing.InQuint:
            result = Quintic.In(t);
            break;
          case Easing.OutQuint:
            result = Quintic.Out(t);
            break;
          case Easing.InOutQuint:
            result = Quintic.InOut(t);
            break;
          case Easing.InBounce:
            result = Bounce.In(t);
            break;
          case Easing.OutBounce:
            result = Bounce.Out(t);
            break;
          case Easing.InOutBounce:
            result = Bounce.InOut(t);
            break;
          case Easing.InBack:
            result = Back.In(t);
            break;
          case Easing.OutBack:
            result = Back.Out(t);
            break;
          case Easing.InOutBack:
            result = Back.InOut(t);
            break;
          default:
            break;
        }

        return result;
      }
    }

    public class Quadratic {
      public static float In(float k) {
        return k * k;
      }

      public static float Out(float k) {
        return k * (2f - k);
      }

      public static float InOut(float k) {
        if ((k *= 2f) < 1f) return 0.5f * k * k;
        return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
      }
    };

    public class Cubic {
      public static float In(float k) {
        return k * k * k;
      }

      public static float Out(float k) {
        return 1f + ((k -= 1f) * k * k);
      }

      public static float InOut(float k) {
        if ((k *= 2f) < 1f) return 0.5f * k * k * k;
        return 0.5f * ((k -= 2f) * k * k + 2f);
      }
    };

    public class Quartic {
      public static float In(float k) {
        return k * k * k * k;
      }

      public static float Out(float k) {
        return 1f - ((k -= 1f) * k * k * k);
      }

      public static float InOut(float k) {
        if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
        return -0.5f * ((k -= 2f) * k * k * k - 2f);
      }
    };

    public class Quintic {
      public static float In(float k) {
        return k * k * k * k * k;
      }

      public static float Out(float k) {
        return 1f + ((k -= 1f) * k * k * k * k);
      }

      public static float InOut(float k) {
        if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
        return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
      }
    };

    public class Sinusoidal {
      public static float In(float k) {
        return 1f - Mathf.Cos(k * Mathf.PI / 2f);
      }

      public static float Out(float k) {
        return Mathf.Sin(k * Mathf.PI / 2f);
      }

      public static float InOut(float k) {
        return 0.5f * (1f - Mathf.Cos(Mathf.PI * k));
      }
    };

    public class Exponential {
      public static float In(float k) {
        return Mathf.Approximately(k, 0f) ? 0f : Mathf.Pow(1024f, k - 1f);
      }

      public static float Out(float k) {
        return Mathf.Approximately(k, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * k);
      }

      public static float InOut(float k) {
        if (Mathf.Approximately(k, 0f)) return 0f;
        if (Mathf.Approximately(k, 1f)) return 1f;
        if ((k *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, k - 1f);
        return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
      }
    };

    public class Circular {
      public static float In(float k) {
        return 1f - Mathf.Sqrt(1f - k * k);
      }

      public static float Out(float k) {
        return Mathf.Sqrt(1f - ((k -= 1f) * k));
      }

      public static float InOut(float k) {
        if ((k *= 2f) < 1f) return -0.5f * (Mathf.Sqrt(1f - k * k) - 1);
        return 0.5f * (Mathf.Sqrt(1f - (k -= 2f) * k) + 1f);
      }
    };

    public class Elastic {
      public static float In(float k) {
        if (Mathf.Approximately(k, 0f)) return 0;
        if (Mathf.Approximately(k, 1f)) return 1;
        return -Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
      }

      public static float Out(float k) {
        if (Mathf.Approximately(k, 0f)) return 0;
        if (Mathf.Approximately(k, 1f)) return 1;
        return Mathf.Pow(2f, -10f * k) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) + 1f;
      }

      public static float InOut(float k) {
        if ((k *= 2f) < 1f) return -0.5f * Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
        return Mathf.Pow(2f, -10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) * 0.5f + 1f;
      }
    };

    public class Back {
      static float s = 1.70158f;
      static float s2 = 2.5949095f;

      public static float In(float k) {
        return k * k * ((s + 1f) * k - s);
      }

      public static float Out(float k) {
        return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
      }

      public static float InOut(float k) {
        if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
        return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
      }
    };

    public class Bounce {
      public static float In(float k) {
        return 1f - Out(1f - k);
      }

      public static float Out(float k) {
        if (k < (1f / 2.75f)) {
          return 7.5625f * k * k;
        } else if (k < (2f / 2.75f)) {
          return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
        } else if (k < (2.5f / 2.75f)) {
          return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
        } else {
          return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
        }
      }

      public static float InOut(float k) {
        if (k < 0.5f) return In(k * 2f) * 0.5f;
        return Out(k * 2f - 1f) * 0.5f + 0.5f;
      }
    };
  }
}
