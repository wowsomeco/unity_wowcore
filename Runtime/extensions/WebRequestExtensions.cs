using UnityEngine.Networking;

namespace Wowsome {
  public static class WebRequestExt {
    public static bool HasErrors(this UnityWebRequest uwr, bool checkStatusCode = true) {
      if (checkStatusCode && uwr.responseCode != 200) return true;

      return uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError || uwr.result == UnityWebRequest.Result.ProtocolError;
    }
  }
}