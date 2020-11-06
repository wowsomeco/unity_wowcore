using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Wowsome {
  public delegate void DownloadedFile<T>(T file, bool err);

  public class ImageDownloader {
    public bool Downloading { get; private set; }

    public IEnumerator Download(string url, string filename, Action<bool> result) {
      string localPath = Path.Combine(Application.persistentDataPath, filename.LastSplit());
      if (File.Exists(localPath)) {
        result(true);
      } else {
        Downloading = true;

        string fullUrl = string.Format("{0}/{1}", url, filename.LastSplit());
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(fullUrl)) {
          yield return uwr.SendWebRequest();
          if (uwr.isNetworkError || uwr.isHttpError) {
            result(false);
          } else {
            // save the texture            
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            string ext = filename.LastSplit().ToLower();

            File.WriteAllBytes(localPath, ext.Contains("png") ? texture.EncodeToPNG() : texture.EncodeToJPG());
            result(true);
          }

          Downloading = false;
        }
      }
    }
  }

  public class JsonDownloader<T> {
    public bool Downloading { get; private set; }

    public IEnumerator Download(string url, DownloadedFile<T> result) {
      Downloading = true;

      using (UnityWebRequest uwr = UnityWebRequest.Get(url)) {
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError) {
          result(default(T), true);
        } else {
          // save
          string text = DownloadHandlerBuffer.GetContent(uwr);
          T json = JsonUtility.FromJson<T>(text);
          result(json, false);
        }

        Downloading = false;
      }
    }
  }
}
