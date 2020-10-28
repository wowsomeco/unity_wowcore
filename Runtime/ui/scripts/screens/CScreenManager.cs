using Wowsome.Core;

namespace Wowsome {
  namespace UI {
    public class CScreenManager : CViewManager, ISceneController {
      #region ISceneController implementation
      public void InitSceneController(ISceneStarter sceneStarter) {
        InitViewManager(sceneStarter);
      }

      public void UpdateSceneController(float dt) {
        UpdateViewManager(dt);
      }
      #endregion            
    }
  }
}