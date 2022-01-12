namespace Wowsome {
  namespace Core {
    public interface ISystem {
      void InitSystem();
      void StartSystem(WEngine gameEngine);
      void UpdateSystem(float dt);
    }

    public delegate void StartSceneController(ISceneStarter sceneStarter);

    public interface ISceneStarter {
      WEngine Engine { get; }
      StartSceneController OnStartSceneController { get; set; }
      T GetController<T>(bool assertIfNull = true) where T : class, ISceneController;
    }

    public interface ISceneController {
      void InitSceneController(ISceneStarter sceneStarter);
      void UpdateSceneController(float dt);
    }
  }
}
