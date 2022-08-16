using Banners.Scriptable;
using UnityEngine;

namespace Banners.Test_Scripts {
  public class PullButton : MonoBehaviour {
    public GachaBanner banner;

    public void Pull() {
      if (banner == null) return;
      banner.PullBanner();
    }
  }
}

