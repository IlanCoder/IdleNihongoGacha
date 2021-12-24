using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gacha.Scriptable;

namespace Gacha.View {
  public class PullButton : MonoBehaviour {
    public GachaBanner banner;

    public void Pull() {
      if (banner == null) return;
      banner.PullBanner();
    }
  }
}

