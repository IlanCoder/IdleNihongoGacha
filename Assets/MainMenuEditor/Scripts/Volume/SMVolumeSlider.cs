using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManager {
  [RequireComponent(typeof(Slider))]
  public class SMVolumeSlider : SMVolumeGeneral {
    Slider slider;
    float prefValue;

    void Awake() {
      slider = GetComponent<Slider>();
      slider.onValueChanged.AddListener(delegate { SetVolumeValue(); });
      CheckPlayerPrefKey();
      GetVolumeValue();
    }

    void SetVolumeValue() {
      foreach(AudioSource audio in audioObject) {
        audio.volume = slider.value;
      }
      prefValue = slider.value;
      PlayerPrefs.SetFloat(playerPrefKey, slider.value);
    }

    void GetVolumeValue() {
      slider.value = prefValue;
      foreach (AudioSource audio in audioObject) {
        audio.volume = prefValue;
      }
    }

    void CheckPlayerPrefKey() {
      if (!PlayerPrefs.HasKey(playerPrefKey)) {
        PlayerPrefs.SetFloat(playerPrefKey, slider.value);
        prefValue = audioObject[0].volume;
        return;
      }
      prefValue = PlayerPrefs.GetFloat(playerPrefKey);
    }
  }
}

