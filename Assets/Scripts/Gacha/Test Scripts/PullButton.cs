using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gacha;

public class PullButton : MonoBehaviour
{
  [SerializeField]ActiveBanners banners;

  public void Pull() {
    if (banners == null) return;
    banners.GetDefaultBanner().PullBanner();
	}
}
