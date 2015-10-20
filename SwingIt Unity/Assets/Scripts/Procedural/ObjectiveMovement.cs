using UnityEngine;
using System.Collections;

public class ObjectiveMovement: MonoBehaviour {
	
	public float speed;
	public enum mType 
	{
		Rebote, Cerrado
	}
	public mType movementType;
	public int startPoint;
	public int nextPoint;
	private int actualPoint;
	private Vector2 dir;
	private int dirPoint;
	public Vector2 posNextPoint;
	public Vector2 posActualPoint;
	EdgeCollider2D zoneMovement;
	// Use this for initialization
	GameObject objective;


	void Awake () {
		zoneMovement = GetComponent<EdgeCollider2D>();
		objective = GameObject.Find ("Objective");
		Initialize ();
	}

	void Update(){
		objective.transform.position += new Vector3(dir.x*speed*Time.deltaTime,dir.y*speed*Time.deltaTime,0);

		//Comprobamos si tiene que pasar al siguiente punto
		bool changePoint = false;
		if (dir.x != 0) {
			if (dir.x > 0 && objective.transform.position.x >= posNextPoint.x){
				changePoint = true;
			}else if (dir.x < 0 && objective.transform.position.x <= posNextPoint.x){
				changePoint = true;
			}
		}
		if (dir.y != 0){
			if (dir.y > 0 && objective.transform.position.y >= posNextPoint.y){
				changePoint = true;
			}else if (dir.y < 0 && objective.transform.position.y <= posNextPoint.y){
				changePoint = true;
			}else{
				changePoint = false;
			}
		//Se calcula el proximo punto y la nueva direccion
		}
		if (changePoint) {
			objective.transform.position = posNextPoint;
			actualPoint = nextPoint;
			posActualPoint = posNextPoint;
			nextPoint += dirPoint;
			if (nextPoint < 0 || nextPoint == zoneMovement.pointCount){
				if (movementType == mType.Rebote){
					dirPoint *=-1;
					nextPoint = actualPoint + dirPoint;
				}else if (movementType == mType.Cerrado){
					if (nextPoint < 0)
						nextPoint = zoneMovement.pointCount-1;
					else
						nextPoint = 0;
				}
			}
			dir = zoneMovement.points[nextPoint] - zoneMovement.points[actualPoint];
			dir = dir.normalized;
			posNextPoint = zoneMovement.points [nextPoint];
			posNextPoint = new Vector2 (posNextPoint.x + transform.position.x, posNextPoint.y + transform.position.y);
		}
	}

	public void Initialize(){
		actualPoint = startPoint;
		posActualPoint = zoneMovement.points [actualPoint];
		zoneMovement.gameObject.transform.position = new Vector3 (objective.transform.position.x - posActualPoint.x, objective.transform.position.y - posActualPoint.y, 0);
		dir = zoneMovement.points[nextPoint] - zoneMovement.points[actualPoint];
		dir = dir.normalized;
		if (nextPoint > actualPoint)
			dirPoint = 1;
		else
			dirPoint = -1;
		posNextPoint = zoneMovement.points [nextPoint];
		posNextPoint = new Vector2 (posNextPoint.x + transform.position.x, posNextPoint.y + transform.position.y);
		posActualPoint = new Vector2 (posActualPoint.x + transform.position.x, posActualPoint.y + transform.position.y);
	}

}
