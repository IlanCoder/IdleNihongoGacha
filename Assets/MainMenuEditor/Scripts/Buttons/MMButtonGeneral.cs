using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManager {
  [DisallowMultipleComponent]
  [RequireComponent(typeof(Button))]
  public class MMButtonGeneral : MonoBehaviour {
    public bool waitForAnimation;
    [HideInInspector]
    public Animator anim;
  }
}

