using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	public float pendulumForce;
	public float launchForce;
	private bool applyForce;
	private Rigidbody2D compRigiBody2D;
	[HideInInspector]
	public DistanceJoint2D compJoint;
	public float fastForward;
	private ProceduralLevel proceduralLevel;

	//TimeWin es el tiempo que tiene que pasar posado en el objetivo para ganar
	public float timeWin = 1;

	void Start () {
		applyForce = false;
		compRigiBody2D = GetComponent<Rigidbody2D> ();
		compRigiBody2D.velocity = new Vector2 (pendulumForce, 0);
		compJoint = GetComponent<DistanceJoint2D> ();
		compJoint.connectedAnchor = new Vector2(transform.parent.position.x,transform.parent.position.y);
		proceduralLevel = GameObject.Find ("ProceduralLevel").GetComponent<ProceduralLevel> ();
	}

	void Update () {
		if (Input.GetKey (KeyCode.A)) {
			Time.timeScale = fastForward;
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


	void OnTriggerEnter2D(Collider2D other){
		if (other.name.Contains ("Limit")) {
			transform.localPosition = new Vector3(0,-0.705f,0);
			compJoint.enabled = true;
			compRigiBody2D.velocity = new Vector2 (pendulumForce, 0);
			timeWin = 1;
		}
	}

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
