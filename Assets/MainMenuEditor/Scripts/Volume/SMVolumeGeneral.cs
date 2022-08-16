using System.Collections.Generic;
using UnityEngine;

namespace MainMenuEditor.Scripts.Volume {
  [DisallowMultipleComponent]
  public class SMVolumeGeneral : MonoBehaviour {
    public string playerPrefKey;
    public List<AudioSource> audioObject;
  }
}

