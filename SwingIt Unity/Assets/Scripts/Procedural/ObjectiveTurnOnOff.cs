using UnityEngine;
using System.Collections;

public class ObjectiveTurnOnOff : MonoBehaviour {

	public float timeOn;
	public float timeOff;
	private float timeActual;
	GameObject objective;
	// Use this for initialization
	void Awake () {
		objective = GameObject.Find ("Objective");
	}
	
	// Update is called once per frame
	void Update () {
		timeActual -= Time.deltaTime;
		if (timeActual <= 0) {
			if (objective.activeInHierarchy) {
				objective.SetActive(false);
				timeActual = timeOff;
			}else{
				objective.SetActive(true);
				timeActual = timeOn;
			}
		}
	}

	public void Initialize(){
		timeActual = timeOn;
	}
}
