using UnityEngine;
using System.Collections;

public class Tier1 : MonoBehaviour {

	public BoxCollider2D targetZone;
	// Use this for initialization
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	void Start () {
		targetZone = GetComponent<BoxCollider2D> ();

	}

}
