using Expeditions.Managers;
using Expeditions.Scriptable;
using Heroes.Scriptable;
using UnityEngine;

namespace Expeditions.Test_Scripts {
    public class AddHeroButton : MonoBehaviour {
    public Hero hero;
   
    public void AddHeroToParty(BasicExpedition expedition) {
      expedition.AddHero(hero);
    }
  }
}
