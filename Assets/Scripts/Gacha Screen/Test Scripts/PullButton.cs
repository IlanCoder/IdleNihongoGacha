using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gacha;

public class PullButton : MonoBehaviour
{
  [SerializeField]GachaBanner banner;

  public void Pull() {
    banner.PullBanner();
	}
}
