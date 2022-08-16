using Heroes.Scriptable;
using TMPro;
using UnityEngine;

namespace Heroes.Test_Scripts
{
	public class UpdateText : MonoBehaviour {

		void OnEnable() => Hero.OnLevelUp += UpdateLvlText;

		private void OnDisable() => Hero.OnLevelUp -= UpdateLvlText;

		void UpdateLvlText(Hero hero) {
			Debug.Log(hero.name);
			GetComponent<TextMeshProUGUI>().text = hero.Level.ToString();
		}
	}
}
