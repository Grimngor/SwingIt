using UnityEngine;
using System.Collections;

public class ObjectiveSpawnZone : MonoBehaviour {

	[HideInInspector]
	public BoxCollider2D targetZone;
	// Use this for initialization
	[HideInInspector]
	public float minX;
	[HideInInspector]
	public float maxX;
	[HideInInspector]
	public float minY;
	[HideInInspector]
	public float maxY;
	
	void Awake () {
		targetZone = GetComponent<BoxCollider2D> ();
		minX = targetZone.bounds.min.x;
		minY = targetZone.bounds.min.y;
		maxX = targetZone.bounds.max.x;
		maxY = targetZone.bounds.max.y;
	}
	
}
