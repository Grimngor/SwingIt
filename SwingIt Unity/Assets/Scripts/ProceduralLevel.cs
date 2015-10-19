using UnityEngine;
using System.Collections;

public class ProceduralLevel : MonoBehaviour {

	public int level;
	public GameObject[] tier_1;
	public GameObject[] tier_2;
	public GameObject[] tier_3;
	public GameObject[] tier_4;
	public GameObject[] tier_5;
	public GameObject[] tier_6;
	private GameObject[] pool_tier_1;
	private GameObject[] pool_tier_2;
	private GameObject[] pool_tier_3;
	private GameObject[] pool_tier_4;
	private GameObject[] pool_tier_5;
	private GameObject[] pool_tier_6;

	private GameObject ropeAnchor;
	private Ball ball;
	private GameObject objective;

	// Use this for initialization
	void Awake () {
		level = 1;
		objective = GameObject.Find ("Objective");
		ball = GameObject.Find ("RopeAnchor/Ball").GetComponent<Ball> ();
		ropeAnchor = GameObject.Find ("RopeAnchor");
		pool_tier_1 = new GameObject[tier_1.Length];
		for (int i=0; i< tier_1.Length; i++) {
			pool_tier_1[i] = (GameObject)Instantiate(tier_1[i]);
		}
		/*pool_tier_2 = new GameObject[tier_2.Length];
		for (int i=0; i< tier_2.Length; i++) {
			pool_tier_2[i] = (GameObject)Instantiate(tier_2[i]);
		}
		pool_tier_3 = new GameObject[tier_3.Length];
		for (int i=0; i< tier_3.Length; i++) {
			pool_tier_3[i] = (GameObject)Instantiate(tier_3[i]);
		}
		pool_tier_4 = new GameObject[tier_4.Length];
		for (int i=0; i< tier_4.Length; i++) {
			pool_tier_4[i] = (GameObject)Instantiate(tier_4[i]);
		}
		pool_tier_5 = new GameObject[tier_5.Length];
		for (int i=0; i< tier_5.Length; i++) {
			pool_tier_5[i] = (GameObject)Instantiate(tier_5[i]);
		}
		pool_tier_6 = new GameObject[tier_6.Length];
		for (int i=0; i< tier_6.Length; i++) {
			pool_tier_6[i] = (GameObject)Instantiate(tier_6[i]);
		}*/
		NextLevel ();
	}
	
	public void NextLevel(){
		level++;

	}
}
