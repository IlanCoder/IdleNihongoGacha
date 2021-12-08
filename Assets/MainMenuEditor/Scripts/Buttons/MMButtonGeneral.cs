using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManager {
  [DisallowMultipleComponent]
  [RequireComponent(typeof(Button))]
  public class MMButtonGeneral : MonoBehaviour {
    public bool waitForAnimation;
    [HideInInspector]
    public Animator anim;
  }
}

