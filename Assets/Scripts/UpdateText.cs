using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateText : MonoBehaviour {

	void OnEnable() => Hero.OnLevepUp += UpdateLvlText;

  void UpdateLvlText(Hero hero) {
    GetComponent<TextMeshProUGUI>().text = hero.Level.ToString();
	}
}
