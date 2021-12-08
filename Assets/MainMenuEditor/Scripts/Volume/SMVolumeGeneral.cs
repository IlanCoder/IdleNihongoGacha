using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManager {
  [DisallowMultipleComponent]
  public class SMVolumeGeneral : MonoBehaviour {
    public string playerPrefKey;
    public List<AudioSource> audioObject;
  }
}

