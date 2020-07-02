# Wowcore

A Collection of utilities and extensions as the core foundation of any 2D Game Projects that we do.

## Contents

- Tween
    - Can be attached directly to a Gameobject, where each Gameobject can have one or more Tween(s).
    - Can be also instantiated programmatically accordingly.

    > Tween Samples

    ```csharp
    // Fade Tween
    ITween m_tweenFade = new CTweenFade(TargetType.Image, gameObject, new FadeData(0f, 1f));
    // Rotation Tween
    ITween m_tweenRotation = new CTweenRotation(TargetType.RectTransform, gameObject, new RotationData(50f, 0.5f))
            .SetCompleteCallback(OnCompleteRotation)
            .SetLoop(3, Loop.Yoyo);
    // Move Tween
    ITween m_tweenMove = new CTweenMove(
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

    > You can also Chain the tween(s) accordingly

    ```csharp
    // this chainer will play all the tweens one at a time.
    m_chainer = new CTweenChainer(TweenerType.StepByStep); // either StepByStep OR Simultaneously
    m_chainer.Add(m_tweenRotation).Add(m_tweenMove).Add(m_tweenFade).PlayExistingAll(() => print("On Done"));
    ```

    Take a look at the [Unit Tests](https://github.com/wowsomeco/unity_wowcore/tree/master/Runtime/tween/scripts/test) for more details of how to use the Tweens.

- Extensions
    - C# Native Extensions e.g. List Extensions, Int Extensions, Float Extensions, String Extensions, etc.
    - Unity Extensions e.g. UI Extensions, Rect Transform Extensions, etc.
- UI Helpers
    - Screen Manager
    - Gesture Handlers
    - etc.

## Documentation

Coming Soon
