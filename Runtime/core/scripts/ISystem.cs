namespace Wowsome {
  namespace Core {
    public interface ISystem {
      void InitSystem();
      void StartSystem(CavEngine gameEngine);
      void UpdateSystem(float dt);
      void OnChangeScene(int index);
    }

    public interface ISceneStarter {
      CavEngine Engine { get; }
      T GetController<T>() where T : class, ISceneController;
    }

    public interface ISceneController {
      void InitSceneController(ISceneStarter sceneStarter);
      void StartSceneController(ISceneStarter sceneStarter);
      void UpdateSceneController(float dt);
    }
  }
}
