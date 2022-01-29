# Wowcore

A Collection of utilities and extensions as the core foundation of any Unity 2D Game Projects that we do. It focuses more on extending the methods as well as other stuffs that are currently not available in Unity by default.

## Getting Started

Right now it serves as a git submodule for backward compatibility.
Add it as a submodule in your project by running:

```console
git submodule add git@github.com:wowsomeco/unity_wowcore.git Assets/wcore
```

from your root project.

## Core Concept

We believe that classic hierarchical initiation is still the best approach for game development e.g. even though Unity makes it easy for us to initalize components by simply adding Awake() as well as Update() methods in them, we're quite against this approach since you can't really control the execution order (don't ever bring up Script Execution Order to hack this issue since you'll end up solving issues with another ones). That's why most if not all of the code in this submodule require you to call the Init() as well as Update(float dt) in them manually. That way we can be sure which script needs to be executed one prior to another in the caller class(es). This also means you can control the Update lifecycle of the children objects when the game is paused, all you need to do is put something condition to return if it's currently paused prior to calling the children update methods and all the children gameobject will automagically get paused e.g.

```csharp
// the game controller update
public void UpdateSceneController(float dt) {
  // when paused, it will return and wont call the update loop below
  if (IsPaused) return;

  for (int i = 0; i < _gameObjects.Count; ++i) {
    _gameObjects[i].UpdateObject(dt);
  }
}
```

## Contents

- WEngine
  This is the core engine that exists as a singleton accross scene. Just simply create a prefab and add it in the Hierarchy for every Unity scene you have in the game. It consists of array of ISystem. Basically ISystem is like a script that only needs to be instantiated once e.g. Audio System, Cache Manager, etc.

- SceneStarter
  This is basically the root Gameobject that calls Unity built-in Start() and Update() methods respectively. this acts as the starting point in the hierarchy to init another (Sub)Controller(s) in the scene. it will also grab the CaveEngine instance and distribute it to each ISceneController object references. Just simply create an empty gameobject and attach this script to it, then whenever you need a SubController in the scene, simply create a Gameobject with your custom script that implements ISceneController and make sure you drag that Gameobject as one of the array items in SceneStarter's Scene Controller Objs via Editor.

- Timer

- Tween

  - Can be attached directly to a Gameobject, where each Gameobject can have one or more Tween(s).
  - Can be also instantiated and updated programmatically accordingly e.g. no Awake() or Update() calls internally, you need to call it from your own script. That way you

  ### Tween Samples

  ```csharp
  // Fade Tween
  ITween tweenFade = new CTweenFade(TargetType.Image, gameObject, new FadeData(0f, 1f));
  // Rotation Tween
  ITween tweenRotation = new CTweenRotation(TargetType.RectTransform, gameObject, new RotationData(50f, 0.5f))
          .SetCompleteCallback(OnCompleteRotation)
          .SetLoop(3, Loop.Yoyo);
  // Move Tween
  ITween tweenMove = new CTweenMove(
          TargetType.RectTransform
          , gameObject
          , new MoveData(
              Vector2.zero
              , 1f
              , TweenType.To
              , Easing.Linear
              , 1f
              ));
  ```

  ### You can also Chain the tween(s) accordingly

  ```csharp
  // this chainer will play all the tweens one at a time.
  m_chainer = new CTweenChainer(TweenerType.StepByStep); // either StepByStep OR Simultaneously
  m_chainer.Add(tweenRotation).Add(tweenMove).Add(tweenFade).PlayExistingAll(() => print("On Done"));
  ```

  Take a look at the [Unit Tests](https://github.com/wowsomeco/unity_wowcore/tree/master/Runtime/tween/scripts/test) for more details of how to use the Tweens.

- Extensions

  - C# Native Extensions e.g. List Extensions, Int Extensions, Float Extensions, String Extensions, etc.

  - Extension Samples

  ```csharp
  // List Extensions
  // Map method that maps List<T> to List<U>
  List<Vector2> vecs = new List<Vector2>() {
      new Vector2(10f, 20f),
      new Vector2(20f, 30f),
      new Vector2(30f, 40f),
    };

  List<float> toFloats = vecs.Map(item => item.x + item.y);
  // Fold method that has a callback for each of the list item that can be used to get a total of some numeric values, get the biggest number a List<int> , etc.
  float sumX = vecs.Fold(0f, (prev, cur) => prev += cur.x);
  ```

  - Unity Extensions e.g. UI Extensions, Rect Transform Extensions, etc.

- UI Helpers
  - Screen Manager
  - Gesture Handlers
  - etc.

## Documentation

Coming Soon
