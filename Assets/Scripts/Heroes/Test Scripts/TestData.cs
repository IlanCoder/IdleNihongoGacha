using Heroes.Scriptable;
using UnityEngine;

namespace Heroes.Test_Scripts
{
    public class TestData : MonoBehaviour
    {
        public Hero hero;
   
        public void LevelUp() {
            hero.LevelUp();
        }
    }
}
