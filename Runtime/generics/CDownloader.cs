using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Wowsome {
  public class DownloadResponse<T> {
    public T Data { get; private set; }
    public string Error { get; private set; }
    public long ResponseCode { get; private set; }
    public bool IsError { get; private set; }

    public DownloadResponse(T d, UnityWebRequest uwr) {
      Data = d;
      Error = uwr.error;
      ResponseCode = uwr.responseCode;
      IsError = uwr.isNetworkError || uwr.isHttpError;
    }
  }

  public class ImageDownloader {
    public bool Downloading { get; private set; }

    public IEnumerator Download(string url, string filename, Action<DownloadResponse<Texture2D>> result = null, Action exist = null) {
      string localPath = Path.Combine(Application.persistentDataPath, filename.LastSplit());
      if (File.Exists(localPath)) {
        exist?.Invoke();
      } else {
        Downloading = true;

        string fullUrl = string.Format("{0}/{1}", url, filename.LastSplit());
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(fullUrl)) {
          yield return uwr.SendWebRequest();
          if (uwr.isNetworkError || uwr.isHttpError) {
            result?.Invoke(new DownloadResponse<Texture2D>(null, uwr));
          } else {
            // save the texture            
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            string ext = filename.LastSplit().ToLower();

            File.WriteAllBytes(localPath, ext.Contains("png") ? texture.EncodeToPNG() : texture.EncodeToJPG());
            result?.Invoke(new DownloadResponse<Texture2D>(texture, uwr));
          }

          Downloading = false;
        }
      }
    }
  }

  public class JsonDownloader<T> {
    public bool Downloading { get; private set; }

    public IEnumerator Download(string url, Action<DownloadResponse<T>> result) {
      Downloading = true;

      using (UnityWebRequest uwr = UnityWebRequest.Get(url)) {
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError) {
          result(new DownloadResponse<T>(default(T), uwr));
        } else {
          // save
          string text = DownloadHandlerBuffer.GetContent(uwr);
          T json = JsonUtility.FromJson<T>(text);
          result(new DownloadResponse<T>(json, uwr));
        }

        Downloading = false;
      }
    }
  }
}
