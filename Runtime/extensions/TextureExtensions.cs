using System.IO;
using UnityEngine;

namespace Wowsome {
  public static class TextureExt {
    public static bool TextureExists(this string filename, out Texture2D t) {
      t = null;
      string localPath = Path.Combine(Application.persistentDataPath, filename);
      if (File.Exists(localPath)) {
        t = localPath.ToTexture2D();
        return true;
      }
      return false;
    }

    public static void SaveTexture(this Texture2D t, string filename) {
      string localPath = Path.Combine(Application.persistentDataPath, filename);
      File.WriteAllBytes(localPath, t.EncodeToPNG());
    }

    public static Sprite ToSprite(this Texture2D texture) {
      Sprite sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.one * 0.5f);
      return sprite;
    }

    public static Sprite ToSprite(this string filePath) {
      return filePath.ToTexture2D().ToSprite();
    }

    public static Texture2D ToTexture2D(this string filePath) {
      Texture2D t = new Texture2D(1, 1);
      t.LoadImage(File.ReadAllBytes(filePath));
      return t;
    }
  }
}