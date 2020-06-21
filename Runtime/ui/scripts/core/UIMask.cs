using UnityEngine;
using UnityEngine.UI;

namespace Wowsome {
  namespace UI {
    public class UIMask : MonoBehaviour {
      void Start() {
        //this is a workaround since adding a mask component to a gameobject on unity editor gives null reference error
        //hence we add manually here
        gameObject.AddComponent<Mask>();
      }
    }
  }
}
