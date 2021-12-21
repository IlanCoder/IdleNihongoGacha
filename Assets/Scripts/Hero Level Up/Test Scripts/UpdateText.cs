using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour {

	void OnEnable() => Hero.OnLevelUp += UpdateLvlText;

	private void OnDisable() => Hero.OnLevelUp -= UpdateLvlText;

	void UpdateLvlText(Hero hero) {
		Debug.Log(hero.name);
    GetComponent<TextMeshProUGUI>().text = hero.Level.ToString();
	}
}