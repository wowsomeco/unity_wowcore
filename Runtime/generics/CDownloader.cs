﻿using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Wowsome.Generic;

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
      IsError = uwr.HasErrors();
    }
  }

  public class ImageDownloader {
    public bool Downloading { get; private set; }

    public IEnumerator Download(string url, string filename, Action<DownloadResponse<Texture2D>> result = null, Action<Texture2D> ifExists = null) {
      string localPath = Path.Combine(Application.persistentDataPath, filename.LastSplit());

      if (File.Exists(localPath)) {
        ifExists?.Invoke(localPath.ToTexture2D());
      } else {
        Downloading = true;

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)) {
          yield return uwr.SendWebRequest();

          if (uwr.HasErrors()) {
            Print.Log(() => "yellow", $"download error: {uwr.error}, url {url}");

            result?.Invoke(new DownloadResponse<Texture2D>(null, uwr));
          } else {
            // save the texture            
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);

            File.WriteAllBytes(localPath, filename.Contains("png") ? texture.EncodeToPNG() : texture.EncodeToJPG());
            result?.Invoke(new DownloadResponse<Texture2D>(texture, uwr));
          }

          Downloading = false;
        }
      }
    }
  }

  public class JsonDownloader<T> {
    public WObservable<bool> Downloading { get; private set; } = new WObservable<bool>(false);

    public IEnumerator Download(string url, Action<DownloadResponse<T>> result) {
      Downloading.Next(true);

      using (UnityWebRequest uwr = UnityWebRequest.Get(url)) {
        yield return uwr.SendWebRequest();

        if (uwr.HasErrors()) {
          result(new DownloadResponse<T>(default(T), uwr));
        } else {
          // save
          string text = DownloadHandlerBuffer.GetContent(uwr);
          T json = JsonUtility.FromJson<T>(text);
          result(new DownloadResponse<T>(json, uwr));
        }

        Downloading.Next(false);
      }
    }
  }
}
