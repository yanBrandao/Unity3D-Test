using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestControl : MonoBehaviour {

	public GameObject bubbleToSpawn;
	public int level = 1;

	public List<GameObject> bubbleList;

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

	void Start() {
		bubbleList = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		bubbleList.RemoveAll (item => item == null);
	}
}
