# Wowcore

A Collection of utilities and extensions as the core foundation of any Unity 2D Game Projects that we do. It focuses more on extending the methods as well as other stuffs that are currently not available in Unity by default.

## Getting Started

Right now it serves as a git submodule for backward compatibility.
Add it as a submodule in your project by running:

```console
git submodule add git@github.com:wowsomeco/unity_wowcore.git Assets/wcore
```

from your root project.

## Contents

- WEngine
  This is the core engine that exists as a singleton accross scene. Just simply create a prefab and add it in the Hierarchy for every Unity scene you have in the game. It consists of array of ISystem. Basically ISystem is like a script that only needs to be instantiated once e.g. Audio System, Cache Manager, etc.

- SceneStarter
  This is basically the root Gameobject that calls Unity built-in Start() and Update() methods respectively. this acts as the starting point in the hierarchy to init another (Sub)Controller(s) in the scene. it will also grab the CaveEngine instance and distribute it to each ISceneController object references. Just simply create an empty gameobject and attach this script to it, then whenever you need a SubController in the scene, simply create a Gameobject with your custom script that implements ISceneController and make sure you drag that Gameobject as one of the array items in SceneStarter's Scene Controller Objs via Editor.

- Timer

- Tween

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

  - Unity Extensions e.g. UI Extensions, Rect Transform Extensions, Sprite Renderer Extensions, etc.

## Documentation

Coming Soon
