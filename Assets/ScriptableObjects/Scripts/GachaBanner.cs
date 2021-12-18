using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Banner", menuName = "Gacha/Banner", order = 1)]
public class GachaBanner : ScriptableObject
{
	#region VARS
	[Header("Banner Pool")]
	[SerializeField] List<Hero> pool = new List<Hero>();
	public List<Hero> Pool { get { return pool; } }
	[Header("Banner Pool Chances")]
	[SerializeField, Range(0.00001f, 100)] List<float> dropChances = new List<float>(); 
	#endregion
}
