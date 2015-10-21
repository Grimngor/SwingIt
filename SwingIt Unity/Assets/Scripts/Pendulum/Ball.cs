using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	/* pendulumForce: Fuerza de la oscilacion del pendulo
	 * launchForce: Fuerza de lanzamiento al soltar el objeto*/
	public float pendulumForce;
	public float launchForce;

	//applyForce: boolean para saber cuando llega al punto con mayor velocidad, ajustandola a pendulumForce
	private bool applyForce;
	private Rigidbody2D compRigiBody2D;
	[HideInInspector]
	public DistanceJoint2D compJoint;
	public float fastForward;
	private ProceduralLevel proceduralLevel;

	//TimeWin es el tiempo que tiene que pasar posado en el objetivo para ganar
	public float timeWin = 1;

	/// <summary>
	/// Start. Se inicializa la bola. Se obtiene todos los compenentes y objetos necesarios
	/// </summary>
	void Start () {
		applyForce = false;
		compRigiBody2D = GetComponent<Rigidbody2D> ();
		compRigiBody2D.velocity = new Vector2 (pendulumForce, 0);
		compJoint = GetComponent<DistanceJoint2D> ();
		compJoint.connectedAnchor = new Vector2(transform.parent.position.x,transform.parent.position.y);
		proceduralLevel = GameObject.Find ("ProceduralLevel").GetComponent<ProceduralLevel> ();
	}

	/// <summary>
	/// Update. Se controla el movimiento del pendulo y, cuando el usuario pulsa, se suelta la bola.
	/// </summary>
	void Update () {
		if (Input.GetKey (KeyCode.A)) {
			Time.timeScale = fastForward;
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			transform.localPosition = new Vector3(0,-0.705f,0);
			compJoint.enabled = true;
			compRigiBody2D.velocity = new Vector2 (pendulumForce, 0);
			timeWin = 1;
			proceduralLevel.NextLevel();
		}
		if (compJoint.enabled) {
			//**Cuando se pueda aplicar fuerza y este en el punto mas bajo, se le aplica la fuerza de pendulo para mantener el arco
			if (applyForce && compRigiBody2D.velocity.y >= 0) {
				if (compRigiBody2D.velocity.x < 0) {
					compRigiBody2D.velocity = new Vector2 (-pendulumForce, 0);
				} else {
					compRigiBody2D.velocity = new Vector2 (pendulumForce, 0);
				}
				applyForce = false;
			} else if (!applyForce && compRigiBody2D.velocity.y <= 0) {
				applyForce = true;
			}
			//**Se lanza la bola, desactivando el joint y aplicando una fuerza de lanzamiento a la direccion que lleve
			if (Input.GetKeyUp (KeyCode.Space) || Input.touchCount > 0) {
				compJoint.enabled = false;
				compRigiBody2D.velocity = compRigiBody2D.velocity.normalized*launchForce;
			}
		}
	}

	/// <summary>
	/// OnTriggerEnter2D. Comprueba si llega a tocar el limite donde el usuario pierde la partida
	/// </summary>
	/// <param name="other">Collider2D con el que colisiona</param>
	void OnTriggerEnter2D(Collider2D other){
		if (other.name.Contains ("Limit")) {
			transform.localPosition = new Vector3(0,-0.705f,0);
			compJoint.enabled = true;
			compRigiBody2D.velocity = new Vector2 (pendulumForce, 0);
			timeWin = 1;
		}
	}

	/// <summary>
	/// OnTriggerStay2D. Comprueba que se mantiene la bola en el objetivo un tiempo igual o mayor a timeWin.
	/// </summary>
	/// <param name="other">Collider2D con el que colisiona.</param>
	void OnTriggerStay2D(Collider2D other) {
		if (other.name.Contains ("Target")) {
			timeWin -= Time.deltaTime;
			if (timeWin <= 0){
				transform.localPosition = new Vector3(0,-0.705f,0);
				compJoint.enabled = true;
				compRigiBody2D.velocity = new Vector2 (pendulumForce, 0);
				timeWin = 1;
				proceduralLevel.NextLevel();
			}
		}
	}
}
