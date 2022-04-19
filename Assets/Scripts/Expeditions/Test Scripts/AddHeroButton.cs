using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expedition.Scriptable;

namespace Expedition.View {
  public class AddHeroButton : MonoBehaviour {
    public Hero hero;
   
    public void AddHeroToParty(BasicExpedition expedition) {
      if (hero == null) return;
      expedition.AddHero(hero);
    }
  }
}
