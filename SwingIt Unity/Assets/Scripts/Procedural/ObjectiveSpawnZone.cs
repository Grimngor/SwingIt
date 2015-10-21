using UnityEngine;
using System.Collections;

//ObjectiveSpawnZone. Clase que tiene la zona de spawn del objetivo
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

	/// <summary>
	/// Awake. Se obtiene el componente collider y se calcula los puntos minimos y maximos del cuadrado
	/// </summary>
	void Awake () {
		targetZone = GetComponent<BoxCollider2D> ();
		minX = targetZone.bounds.min.x;
		minY = targetZone.bounds.min.y;
		maxX = targetZone.bounds.max.x;
		maxY = targetZone.bounds.max.y;
	}
	
}
