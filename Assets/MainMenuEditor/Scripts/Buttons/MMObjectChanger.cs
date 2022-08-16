using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuEditor.Scripts.Buttons {
  public class MMObjectChanger : MMButtonGeneral {
    
    public List<GameObject> menusToDeactivate = new List<GameObject>();
    public List<GameObject> menusToActivate = new List<GameObject>();
    
    private void Start() {
      if (!waitForAnimation) {
        GetComponent<Button>().onClick.AddListener(() => ChangeMenu());
      } else {
        anim = GetComponent<Animator>();
        GetComponent<Button>().onClick.AddListener(() => AnimationChangeMenu());
      } 
    }

    void ChangeMenu() {
      foreach(GameObject menuScreen in menusToDeactivate) {
        menuScreen.SetActive(false);
      }
      foreach (GameObject menuScreen in menusToActivate) {
        menuScreen.SetActive(true);
      }
    }

    void AnimationChangeMenu() {
      anim.Play("Click");
      StartCoroutine(WaitForAnimToChangeMenu());
      
    }

    IEnumerator WaitForAnimToChangeMenu() {
      yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
      /*while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
        yield return 0;
      }*/
      ChangeMenu();
    }
  }
}

