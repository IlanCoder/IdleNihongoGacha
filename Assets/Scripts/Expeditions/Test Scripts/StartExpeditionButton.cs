using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Expedition.Scriptable;

namespace Expedition.View {
  [RequireComponent(typeof(Button))]
  public class StartExpeditionButton : MonoBehaviour {
    public BasicExpedition expedition;

    private void OnEnable() {
      if (expedition == null) return;
      GetComponent<Button>().interactable = !expedition.OnExpedition;
    }

    public void StartExpedition() {
      expedition.StartExpedition();
    }
  }
}
