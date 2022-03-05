using UnityEngine;

namespace Wowsome {
  public enum AndroidPlatform {
    Google,
    Amazon,
  }

  public static class PlatformUtil {
    public static string GetStringByPlatform(string ios, string google, string amazon) {
      if (Application.platform == RuntimePlatform.IPhonePlayer) return ios;

      return AppSettings.AndroidPlatform == AndroidPlatform.Google ? google : amazon;
    }

    public static string GetStringByPlatform(string ios, string google) {
      if (Application.platform == RuntimePlatform.IPhonePlayer) return ios;

      return google;
    }
  }
}