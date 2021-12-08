using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManager {
  [RequireComponent(typeof(Toggle))]
  public class SMVolumeToggle : SMVolumeGeneral {
    public Slider sliderToBlock;
    Toggle toggle;
    bool prefValue;
    
    private void Awake() {
      toggle = GetComponent<Toggle>();
      toggle.onValueChanged.AddListener(delegate { ToggleSound(); });
      CheckPlayerPrefKey();
      GetVolumeMute();
    }

    void ToggleSound() {
      if (sliderToBlock != null) {
        BlockSlider();
      }
      foreach (AudioSource audio in audioObject) {
        audio.mute = !toggle.isOn;
      }
      prefValue = toggle.isOn;
      if (prefValue) {
        PlayerPrefs.SetFloat(playerPrefKey, 1);
        return;
      }
      PlayerPrefs.SetFloat(playerPrefKey, 0);
    }

    void BlockSlider() {
      sliderToBlock.interactable = toggle.isOn;
    }

    void GetVolumeMute() {
      foreach (AudioSource audio in audioObject) {
        audio.mute = !prefValue;
      }
      toggle.isOn = prefValue;    
    }

    void CheckPlayerPrefKey() {
      if (!PlayerPrefs.HasKey(playerPrefKey)) {
        if (toggle.isOn) {
          PlayerPrefs.SetFloat(playerPrefKey, 1);
          prefValue = true;
          return;
        }
        PlayerPrefs.SetFloat(playerPrefKey, 0);
        prefValue = false;
        return;
      }
      float tempPrefVal = PlayerPrefs.GetFloat(playerPrefKey);
      if (tempPrefVal > 0) {
        prefValue = true;
        return;
      }
      prefValue = false;
    }
  }
}

