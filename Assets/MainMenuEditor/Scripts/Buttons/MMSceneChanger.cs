using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MenuManager {
  public class MMSceneChanger : MMButtonGeneral {
    public string scene;

    private void Start() {
      if (!waitForAnimation) {
        GetComponent<Button>().onClick.AddListener
          (() => ChangeScene());
      } else {
        anim = GetComponent<Animator>();
        GetComponent<Button>().onClick.AddListener
          (() => AnimationChangeScene());
      }
    }

    void ChangeScene() {
      SceneManager.LoadScene(scene);
    }

    void AnimationChangeScene() {
      anim.Play("Click");
      StartCoroutine(WaitForAnimToChangeScene());
    }

    IEnumerator WaitForAnimToChangeScene() {
      yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
      ChangeScene();
    }
  }
}

