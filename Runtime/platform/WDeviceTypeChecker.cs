using System;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace Wowsome {
  [Serializable]
  public enum DeviceType {
    Tablet,
    Phone
  }

  public static class WDeviceTypeChecker {
    public static DeviceType GetDeviceType() {
#if UNITY_IOS
      bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
      if (deviceIsIpad) {
        return DeviceType.Tablet;
      }

      bool deviceIsIphone = UnityEngine.iOS.Device.generation.ToString().Contains("iPhone");
      if (deviceIsIphone) {
        return DeviceType.Phone;
      }
#endif

      float aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
      bool isTablet = (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f);

      return isTablet ? DeviceType.Tablet : DeviceType.Phone;
    }

    private static float DeviceDiagonalSizeInInches() {
      float screenWidth = Screen.width / Screen.dpi;
      float screenHeight = Screen.height / Screen.dpi;
      float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

      return diagonalInches;
    }
  }
}