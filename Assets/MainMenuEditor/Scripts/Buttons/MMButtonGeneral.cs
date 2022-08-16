using UnityEngine;
using UnityEngine.UI;

namespace MainMenuEditor.Scripts.Buttons {
  [DisallowMultipleComponent]
  [RequireComponent(typeof(Button))]
  public class MMButtonGeneral : MonoBehaviour {
    public bool waitForAnimation;
    [HideInInspector]
    public Animator anim;
  }
}

