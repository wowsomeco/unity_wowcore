using TMPro;

namespace Wowsome {
  public static class TMPExt {
    public static TMP_Text Set(this TMP_Text t, string text) {
      t.text = text;

      return t;
    }
  }
}