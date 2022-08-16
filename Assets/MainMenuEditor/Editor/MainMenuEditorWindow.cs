using System.Collections;
using System.Collections.Generic;
using MainMenuEditor.Scripts.Buttons;
using MainMenuEditor.Scripts.Volume;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public enum MENUTYPE {
  [InspectorName("Change Menu Button")]
  MenuChanger = 1,
  [InspectorName("Change Scene Button")]
  SceneChanger = 2,
  [InspectorName("Audio Volume Slider")]
  AudioSlider = 3,
  [InspectorName("Audio Volume Toggle")]
  AudioMute = 4,
}

public class MainMenuEditorWindow : EditorWindow
{
  GameObject selection;
  MMObjectChanger objectChanger;
  MMSceneChanger sceneChanger;
  SMVolumeSlider volumeSlider;
  SMVolumeToggle volumeToggle;
  MENUTYPE menuType;
  bool buttonHasAnim;
  

  [MenuItem("Window/Main Menu Editor")]
  public static void ShowWindow() {
    EditorWindow.GetWindow(typeof(MainMenuEditorWindow));
  }

  void ChangeSelection() {
    selection = Selection.activeGameObject;
    CheckScripts();
  }

  void CheckScripts() {
    if (selection != null) {
      objectChanger = selection.GetComponent<MMObjectChanger>();
      if (objectChanger != null) {
        menuType = MENUTYPE.MenuChanger;
        if (objectChanger.menusToDeactivate.Count > 0) {
          deactivatesMenu = true;
        } else {
          deactivatesMenu = false;
        }
        if (objectChanger.menusToActivate.Count > 0) {
          activatesMenu = true;
        } else {
          activatesMenu = false;
        }
        return;
      }
      sceneChanger = selection.GetComponent<MMSceneChanger>();
      if (sceneChanger != null) {
        menuType = MENUTYPE.SceneChanger;
        scene = new Object();
        return;
      }
      volumeSlider = selection.GetComponent<SMVolumeSlider>();
      if (volumeSlider != null) {
        menuType = MENUTYPE.AudioSlider;     
        return;
      }
      volumeToggle = selection.GetComponent<SMVolumeToggle>();
      if (volumeToggle != null) {
        menuType = MENUTYPE.AudioMute;
        return;
      }
    }
  }

  private void Awake() {
    ChangeSelection();
  }

  private void OnSelectionChange() {
    ChangeSelection();
  }

  private void OnInspectorUpdate() {
    Repaint();
  }

  void OnGUI() {
    if (selection == null) {
      EditorGUILayout.LabelField("No Object selected", EditorStyles.boldLabel);
      return;
    }
    EditorGUILayout.LabelField(string.Format("Function of the object: {0}", selection.name), EditorStyles.boldLabel);
    menuType = (MENUTYPE)EditorGUILayout.EnumPopup(menuType);
    if (selection != null) {
      switch (menuType) {
        case MENUTYPE.MenuChanger:
          if (objectChanger == null) {
            AddMenuChangerScreen();
          } else {
            MenuChangerScreen();
          }    
          break;
        case MENUTYPE.SceneChanger:
          if (sceneChanger == null) {
            AddSceneChangerScreen();
          } else {
            SceneChangerScreen();
          }
          break;
        case MENUTYPE.AudioSlider:
          if (volumeSlider == null) {
            AddVolumeSliderScreen();
          } else {
            VolumeSliderScreen();
          }
          break;
        case MENUTYPE.AudioMute:
          if (volumeToggle == null) {
            AddVolumeToggleScreen();
          } else {
            VolumeToggleScreen();
          }
          break;
      }
    }
  }

  void CheckForClashScripts<T>() where T : Component {
    T clashScript = selection.GetComponent<T>();
    if (clashScript != null) {
      DestroyImmediate(clashScript);
    }
  }

  void WaitForAnimSection(MMButtonGeneral buttonScript) {
    buttonScript.waitForAnimation = EditorGUILayout.ToggleLeft(("Button has animation"),
     buttonScript.waitForAnimation);
  }

  void ResizeList(List<GameObject> list, int newSize) {
    if (list.Count == newSize) {
      return;
    }
    while (list.Count < newSize) {
      list.Add(null);
    }
    while (list.Count > newSize) {
      list.RemoveAt(list.Count - 1);
    }
  }

  void ResizeList(List<AudioSource> list, int newSize) {
    if (list.Count == newSize) {
      return;
    }
    while (list.Count < newSize) {
      list.Add(null);
    }
    while (list.Count > newSize) {
      list.RemoveAt(list.Count - 1);
    }
  }

  #region Menu Changer
  bool deactivatesMenu;
  bool activatesMenu;
  int deactivateMenuSize;
  int activateMenuSize;

  void AddMenuChangerScreen() {
    EditorGUILayout.LabelField("Current Button doesnt have the necesary script");
    if(GUILayout.Button("Add Script")) {
      CheckForClashScripts<MMButtonGeneral>();
      selection.AddComponent<MMObjectChanger>();
      CheckScripts();
    }
  }

  void MenuChangerScreen() {
    WaitForAnimSection(objectChanger);
    DeactivateMenusSection();
    ActivateMenusSection();
    ApplyMenuChange();
  }

  void DeactivateMenusSection() {  
    deactivatesMenu = EditorGUILayout.BeginToggleGroup("Deactivates Menus", deactivatesMenu);
      deactivateMenuSize = EditorGUILayout.DelayedIntField
        ("Menus to deactivate", objectChanger.menusToDeactivate.Count);
      ResizeList(objectChanger.menusToDeactivate, deactivateMenuSize); 
      for (int i = 0; i < deactivateMenuSize; i++) {
        objectChanger.menusToDeactivate[i] = (GameObject)EditorGUILayout.ObjectField
          (objectChanger.menusToDeactivate[i], typeof(GameObject), true);
      }
    EditorGUILayout.EndToggleGroup();
  }

  void ActivateMenusSection() {
    activatesMenu = EditorGUILayout.BeginToggleGroup("Activates Menus", activatesMenu);
      activateMenuSize = EditorGUILayout.DelayedIntField
        ("Menus to activate", objectChanger.menusToActivate.Count);
      ResizeList(objectChanger.menusToActivate, activateMenuSize);
      for (int i = 0; i < activateMenuSize; i++) {
        objectChanger.menusToActivate[i] = (GameObject)EditorGUILayout.ObjectField
          (objectChanger.menusToActivate[i], typeof(GameObject), true);
      }
    EditorGUILayout.EndToggleGroup();
  }
 
  void ApplyMenuChange() {
    if (GUILayout.Button("Apply")) {
      objectChanger.waitForAnimation = buttonHasAnim;
      if (!deactivatesMenu) {
        objectChanger.menusToDeactivate = new List<GameObject>();
      }
      if (!activatesMenu) {
        objectChanger.menusToActivate = new List<GameObject>();
      }
    }
  }


  #endregion

  #region Scene Changer
  Object scene;

  void AddSceneChangerScreen() {
    EditorGUILayout.LabelField("Current Button doesnt have the necesary script");
    if (GUILayout.Button("Add Script")) {
      CheckForClashScripts<MMButtonGeneral>();
      selection.AddComponent<MMSceneChanger>();
      CheckScripts();
    }
  }

  void SceneChangerScreen() {
    WaitForAnimSection(sceneChanger);
    SceneSection();
  }

  void SceneSection() {
    EditorGUILayout.LabelField("Current Target Scene", EditorStyles.boldLabel);
    if (sceneChanger.scene == null || sceneChanger.scene== "") {
      EditorGUILayout.LabelField("No current scene selected");
    } else {
      EditorGUILayout.LabelField(sceneChanger.scene);
    }   
    EditorGUILayout.LabelField("New Target Scene", EditorStyles.boldLabel);
    scene = EditorGUILayout.ObjectField(scene, typeof(SceneAsset), false) as SceneAsset;
    if (scene != null) {
      sceneChanger.scene = scene.name;
    }  
  }
  #endregion

  int audioSourceCout;

  void PlayerPrefSection<T>() where T : SMVolumeGeneral{
    T volumeScript = selection.GetComponent<T>();
    EditorGUILayout.LabelField("Player preferences key name", EditorStyles.boldLabel);
    volumeScript.playerPrefKey = EditorGUILayout.DelayedTextField(volumeScript.playerPrefKey);
  }

  void AudioSourceSection<T>() where T : SMVolumeGeneral {
    T volumeScript = selection.GetComponent<T>();
    ResizeList(volumeScript.audioObject, audioSourceCout);
    EditorGUILayout.LabelField("Audio Source to modify", EditorStyles.boldLabel);
    audioSourceCout = EditorGUILayout.DelayedIntField
       ("Numer of audio Source", volumeScript.audioObject.Count);
    for (int i = 0; i < audioSourceCout; i++) {
      volumeScript.audioObject[i] = (AudioSource)EditorGUILayout.ObjectField
      (volumeScript.audioObject[i], typeof(AudioSource), true);
    }
  }

  #region Audio Slider
  void AddVolumeSliderScreen() {
    EditorGUILayout.LabelField("Current slider doesnt have the necesary script");
    if (GUILayout.Button("Add Script")) {
      CheckForClashScripts<SMVolumeGeneral>();
      selection.AddComponent<SMVolumeSlider>();
      CheckScripts();
    }
  }

  void VolumeSliderScreen() {
    PlayerPrefSection<SMVolumeSlider>();
    AudioSourceSection<SMVolumeSlider>();
  }
  #endregion

  #region AudioToggle
  void AddVolumeToggleScreen() {
    EditorGUILayout.LabelField("Current toggle doesnt have the necesary script");
    if (GUILayout.Button("Add Script")) {
      CheckForClashScripts<SMVolumeGeneral>();
      selection.AddComponent<SMVolumeToggle>();
      CheckScripts();
    }
  }

  void VolumeToggleScreen() {
    PlayerPrefSection<SMVolumeToggle>();
    AudioSourceSection<SMVolumeToggle>();
    VolumeToggleSliderScreen();
  }

  void VolumeToggleSliderScreen() {
    EditorGUILayout.LabelField("SliderToModify if there is One", EditorStyles.boldLabel);
    volumeToggle.sliderToBlock = (Slider)EditorGUILayout.ObjectField
      (volumeToggle.sliderToBlock, typeof(Slider), true);
  }
  #endregion
}
