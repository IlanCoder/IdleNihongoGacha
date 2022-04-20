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
      ChangeInteractable(expedition);
      BasicExpedition.OnExpeditionSateChange += ChangeInteractable;
    }

    private void OnDisable() => BasicExpedition.OnExpeditionSateChange -= ChangeInteractable;

    public void StartExpedition() {
      expedition.StartExpedition();
    }

    private void ChangeInteractable(BasicExpedition expeditionState) {
      GetComponent<Button>().interactable = !expeditionState.OnExpedition;
    }
  }
}
