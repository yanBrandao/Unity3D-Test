using UnityEngine;
using System.Collections;

public class TestControl : MonoBehaviour {

	public GameObject bubbleToSpawn;
	public int level = 1;

	public static TestControl Instance{
		get; 
		private set;
	}

	void Awake(){
		if (Instance == null) {
			Instance = this;
		}
		DontDestroyOnLoad (gameObject);
		Physics.gravity = new Vector3 (0f, 10f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 positionToSpawn = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		positionToSpawn.z = 0;
		if (Input.GetButtonDown ("Fire1")) {
			Instantiate (bubbleToSpawn, positionToSpawn, Quaternion.identity);
		}*/
	}
}
