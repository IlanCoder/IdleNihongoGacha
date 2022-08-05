using UnityEngine;
using UnityEngine.UI;
using Expedition.Scriptable;

namespace Expedition.View {
    [RequireComponent(typeof(Button))]
    public class EndEarlyButton : MonoBehaviour {
        // Start is called before the first frame update
        [SerializeField] BasicExpedition _expedition;
        Button _button;

        private void Start() => _button = GetComponent<Button>();

        private void OnEnable() => BasicExpedition.OnEarlyFinishChange += ChangeClickable;

        private void OnDisable() => BasicExpedition.OnEarlyFinishChange -= ChangeClickable;

        public void EndExpedition() {
            _expedition.FinishEarly();
        }

        void ChangeClickable(bool enable) {
            _button.interactable = enable;
        }
    }
}
