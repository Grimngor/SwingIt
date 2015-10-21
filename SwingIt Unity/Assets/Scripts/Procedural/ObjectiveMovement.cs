using UnityEngine;
using System.Collections;

//ObjectiveMovement. Clase encargada de mover el objetivo
public class ObjectiveMovement: MonoBehaviour {
	
	public float speed;
	public enum mType 
	{
		Rebote, Cerrado
	}
	public mType movementType;

	/* startPoint: Posicion en el vector del punto inicial del movimiento
	 * nextPointIni: Posicion en el vector del siguiente punto al que se desplaza al inicar el movimiento
	 * nextPoint: Posicion en el vector del siguiente punto al que se desplaza actualmente
	 * actualPoint: Posicion en el vector del punto actual*/
	public int startPoint;
	public int nextPointIni;
	private int nextPoint;
	private int actualPoint;

	/* dir: Vector2 de la direccion del movimiento actual del objetivo
	 * dirPoint: Direccion con la que recorre el vector de posiciones. 1 Derecha, -1 Izquierda*/
	private Vector2 dir;
	private int dirPoint;

	/* posNextPoint: Vector2 de la posicion en el mundo del proximo punto al que se esta desplazando
	 * posActualPoint: Vector2 de la posicion en el mundo del punto del que parte*/
	public Vector2 posNextPoint;
	public Vector2 posActualPoint;


	EdgeCollider2D zoneMovement;
	GameObject objective;

	/// <summary>
	/// Awake. Se obtienen los componentes y los objetos necesarios en tiempo de ejecucion
	/// </summary>
	void Awake () {
		zoneMovement = GetComponent<EdgeCollider2D>();
		objective = GameObject.Find ("Objective");
	}

	/// <summary>
	/// Update. Se mueve el objetivo en funcion de los puntos asignados en un EdgeCollider2D
	/// </summary>
	void Update(){
		//Movemos el objetivo
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
		}
		//Se calcula el proximo punto y la nueva direccion
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

	/// <summary>
	/// Initalize. Funcion publica que inicializa los puntos de partida para el movimiento y la direccion.
	/// Se llama desde la clase ProceduralLevel cuando se genere un nivel con movimiento.
	/// </summary>
	public void Initialize(){
		nextPoint = nextPointIni;
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
