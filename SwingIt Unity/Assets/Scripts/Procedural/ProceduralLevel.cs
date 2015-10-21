using UnityEngine;
using System.Collections;

public class ProceduralLevel : MonoBehaviour {

	//level: Nivel actual del juego
	public int level;

	public int[] numLevels;

	//tier_X: Arrays de Prefabs. Cada tier contiene sus prefabs que se iran generando de forma procedural
	public GameObject[] tier_1;
	public GameObject[] tier_2;
	public GameObject[] tier_3;
	public GameObject[] tier_4;
	public GameObject[] tier_5;
	public GameObject[] tier_6;

	//pool_tier_X: Arrays de Objetos. Se carga en memoria al comenzar una instancia de cada prefab.
	private GameObject[] pool_tier_1;
	private GameObject[] pool_tier_2;
	private GameObject[] pool_tier_3;
	private GameObject[] pool_tier_4;
	private GameObject[] pool_tier_5;
	private GameObject[] pool_tier_6;

	private GameObject ropeAnchor;
	private Ball ball;
	private GameObject objective;

	private GameObject prefabPrevious;

	/// <summary>
	/// Awake. Se asigna nivel a 1. Se obtienen los objetos necesarios y se cargan las pools.
	/// </summary>
	void Awake () {
		if (level < 0)
			level = 0;
		objective = GameObject.Find ("Objective");
		ball = GameObject.Find ("RopeAnchor/Ball").GetComponent<Ball> ();
		ropeAnchor = GameObject.Find ("RopeAnchor");
		pool_tier_1 = new GameObject[tier_1.Length];
		for (int i=0; i< tier_1.Length; i++) {
			pool_tier_1[i] = (GameObject)Instantiate(tier_1[i]);
			pool_tier_1[i].transform.parent = transform;
			pool_tier_1[i].SetActive(false);
		}
		pool_tier_2 = new GameObject[tier_2.Length];
		for (int i=0; i< tier_2.Length; i++) {
			pool_tier_2[i] = (GameObject)Instantiate(tier_2[i]);
			pool_tier_2[i].transform.parent = transform;
			pool_tier_2[i].SetActive(false);
		}

		pool_tier_3 = new GameObject[tier_3.Length];
		for (int i=0; i< tier_3.Length; i++) {
			pool_tier_3[i] = (GameObject)Instantiate(tier_3[i]);
			pool_tier_3[i].transform.parent = transform;
			pool_tier_3[i].SetActive(false);
		}
		pool_tier_4 = new GameObject[tier_4.Length];
		for (int i=0; i< tier_4.Length; i++) {
			pool_tier_4[i] = (GameObject)Instantiate(tier_4[i]);
			pool_tier_4[i].transform.parent = transform;
			pool_tier_4[i].SetActive(false);
		}
		/*
		pool_tier_5 = new GameObject[tier_5.Length];
		for (int i=0; i< tier_5.Length; i++) {
			pool_tier_5[i] = (GameObject)Instantiate(tier_5[i]);
		}
		pool_tier_6 = new GameObject[tier_6.Length];
		for (int i=0; i< tier_6.Length; i++) {
			pool_tier_6[i] = (GameObject)Instantiate(tier_6[i]);
		}*/

		//Se asigna el primer prefab como previo y se hace un NextLevel
		prefabPrevious = pool_tier_1 [0];
		NextLevel ();
	}

	/// <summary>
	/// Nextlevel. Metodo que se llama al completar la fase y genera la siguiente.
	/// </summary>
	public void NextLevel(){
		level++;
		prefabPrevious.SetActive (false);
		GameObject nextPrefab;
		if (level <= numLevels [0]) {
			nextPrefab = pool_tier_1 [Random.Range (0, pool_tier_1.Length)];
			nextPrefab.SetActive (true);
			NextTier1 (nextPrefab);
		} else if (level <= (numLevels [0] + numLevels [1])) {
			nextPrefab = pool_tier_2 [Random.Range (0, pool_tier_2.Length)];
			nextPrefab.SetActive (true);
			NextTier1 (nextPrefab);
			NextTier2(nextPrefab);
		} else if (level <= (numLevels [0] + numLevels [1] + numLevels[2])){
			nextPrefab = pool_tier_3 [Random.Range (0, pool_tier_3.Length)];
			nextPrefab.SetActive (true);
			NextTier1 (nextPrefab);
			NextTier2(nextPrefab);
			//NextTier3(nextPrefab);
		}else{
			nextPrefab = pool_tier_4 [Random.Range (0, pool_tier_4.Length)];
			nextPrefab.SetActive (true);
			NextTier1 (nextPrefab);
			NextTier2(nextPrefab);
			//NextTier3(nextPrefab);
			NextTier4(nextPrefab);
		}
		prefabPrevious = nextPrefab;
	}

	/// <summary>
	/// NextsTier1. Genera la parte de tier1 de la siguiente fase. 
	/// Tier1: Se encarga de posicionar el objetivo dentro de una zona de spawn
	/// </summary>
	/// <param name="prefab">Prefab.</param>
	private void NextTier1(GameObject prefab){
		ObjectiveSpawnZone tier = prefab.transform.FindChild("SpawnZone").GetComponent<ObjectiveSpawnZone> ();
		float posX = Random.Range (tier.minX, tier.maxX);
		float posY = Random.Range (tier.minY, tier.maxY);
		objective.transform.position = new Vector3 (
			posX,posY, 0);
	}

	private void NextTier2(GameObject prefab){
		if (prefab.transform.FindChild ("Movement").gameObject.activeInHierarchy) {
			prefab.transform.FindChild ("Movement").GetComponent<ObjectiveMovement>().Initialize();
		}
	}

	private void NextTier3(GameObject prefab){

	}

	private void NextTier4(GameObject prefab){
		if (prefab.transform.FindChild ("TurnOnOff").gameObject.activeInHierarchy) {
			prefab.transform.FindChild ("TurnOnOff").GetComponent<ObjectiveTurnOnOff>().Initialize();
		}
	}
}
