using UnityEngine.SceneManagement;

namespace Wowsome {
  namespace Core {
    public delegate void ChangeScene(Scene scene);

    public interface ISystem {
      void InitSystem();
      void StartSystem(CavEngine gameEngine);
      void UpdateSystem(float dt);
    }

    public delegate void StartSceneController(ISceneStarter sceneStarter);

    public interface ISceneStarter {
      CavEngine Engine { get; }
      StartSceneController OnStartSceneController { get; set; }
      T GetController<T>() where T : class, ISceneController;
    }

    public interface ISceneController {
      void InitSceneController(ISceneStarter sceneStarter);
      void UpdateSceneController(float dt);
    }
  }
}
