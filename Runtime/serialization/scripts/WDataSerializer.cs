using System.IO;
using UnityEngine;

namespace Wowsome.Serialization {
  public interface IDataSerializer {
    void Save<T>(T data, string path, bool isPrettyPrint = false);
    T Load<T>(string path);
    bool Exists(string path);
    void Delete(string path);
  }

  public class JsonDataSerializer : IDataSerializer {
    public void Save<T>(T data, string path, bool isPrettyPrint = false) {
#if UNITY_EDITOR
      Debug.Log(path);
#endif
      string jsondata = JsonUtility.ToJson(data, isPrettyPrint);
      StreamWriter streamWriter = File.CreateText(path);
      streamWriter.Write(jsondata);
      streamWriter.Close();
    }

    public T Load<T>(string path) {
      if (Exists(path)) {
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(jsonData);
      }
      return default(T);
    }

    public bool Exists(string path) {
      return File.Exists(path);
    }

    public void Delete(string path) {
      if (File.Exists(path)) {
        File.Delete(path);
      }
    }
  }

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
}


