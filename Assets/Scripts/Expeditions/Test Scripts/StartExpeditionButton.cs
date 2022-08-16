using Expeditions.Managers;
using Expeditions.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Expeditions.Test_Scripts {
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
