﻿using System.IO;
using UnityEngine;

namespace Wowsome.Serialization {
  public interface IDataSerializer {
    void Save<T>(T data, string path, bool isPrettyPrint = false);
    T Load<T>(string path);
    bool Exists(string path);
    void Delete(string path);
  }

  /// <summary>
  /// Use this to save or load your data under any path you like,
  /// You need to provide the fullPath e.g. /Assets/x/y etc.
  /// </summary>
  public class JsonSerializer : IDataSerializer {
    public void Save<T>(T data, string fullPath, bool isPrettyPrint = false) {
      Print.Info(fullPath);

      string jsondata = JsonUtility.ToJson(data, isPrettyPrint);

      StreamWriter streamWriter = File.CreateText(fullPath);
      streamWriter.Write(jsondata);
      streamWriter.Close();
    }

    public T Load<T>(string fullPath) {
      if (Exists(fullPath)) {
        string jsonData = File.ReadAllText(fullPath);
        return JsonUtility.FromJson<T>(jsonData);
      }

      return default(T);
    }

    public bool Exists(string fullPath) => File.Exists(fullPath);

    public void Delete(string fullPath) {
      if (Exists(fullPath)) {
        File.Delete(fullPath);
      }
    }
  }

  /// <summary>
  /// Use this to save your data under Application.persistentDataPath,
  /// Ideal for mobile
  /// </summary>
  public class PersistentSerializer : IDataSerializer {
    public void Save<T>(T data, string path, bool isPrettyPrint = false) {
      string fullPath = CombinePath(path);

      Print.Info(fullPath);

      string jsondata = JsonUtility.ToJson(data, isPrettyPrint);

      StreamWriter streamWriter = File.CreateText(fullPath);
      streamWriter.Write(jsondata);
      streamWriter.Close();
    }

    public T Load<T>(string path) {
      if (Exists(path)) {
        string fullPath = CombinePath(path);

        string jsonData = File.ReadAllText(fullPath);
        return JsonUtility.FromJson<T>(jsonData);
      }

      return default(T);
    }

    public bool Exists(string path) {
      string fullPath = CombinePath(path);

      return File.Exists(fullPath);
    }

    public void Delete(string path) {
      string fullPath = CombinePath(path);

      if (File.Exists(fullPath)) {
        File.Delete(fullPath);
      }
    }

    string CombinePath(string path) => Path.Combine(Application.persistentDataPath, path);
  }

  /// <summary>
  /// Use this to save your data in PlayerPrefs path
  /// Ideal for web
  /// </summary>
  public class PlayerPrefsSerializer : IDataSerializer {
    public void Save<T>(T data, string path, bool isPrettyPrint = false) {
      string strValue = JsonUtility.ToJson(data);
      PlayerPrefs.SetString(path, strValue);
    }

    public T Load<T>(string path) {
      if (!Exists(path)) return default(T);

      string strValue = PlayerPrefs.GetString(path);

      return JsonUtility.FromJson<T>(strValue);
    }

    public bool Exists(string path) {
      return PlayerPrefs.HasKey(path);
    }

    public void Delete(string path) {
      if (!Exists(path)) return;

      PlayerPrefs.DeleteKey(path);
    }
  }

  public static class CrossPlatformSerializer {
    public static IDataSerializer Create() {
      Print.Info(Application.platform);

      if (Application.platform == RuntimePlatform.WebGLPlayer) {
        return new PlayerPrefsSerializer();
      }

      return new PersistentSerializer();
    }
  }
}


