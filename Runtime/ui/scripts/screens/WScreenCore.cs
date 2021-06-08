namespace Wowsome.UI {
  public enum ViewState {
    WillAppear,
    DidAppear,
    WillDisappear,
    DidDisappear
  }

  public struct VisibilityEv {
    public string ScreenId { get; set; }
    public ViewState State { get; set; }
  }

  public interface IScreenObject {
    void InitScreenObject(WScreenManager controller);
    void UpdateScreenObject(float dt);
  }
}
