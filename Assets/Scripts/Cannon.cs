﻿using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {
	public int rotationOffset = 0;

	public Transform BubbleSpawnPosition;

	public GameObject Bubble;

	void Start() {
		Instantiate (Bubble, BubbleSpawnPosition.localPosition, BubbleSpawnPosition.rotation);
	}

	public IEnumerator DoDelay(float seconds, System.Action callback){
		yield return new WaitForSeconds (seconds);
		callback ();
	}

	public void InstantiateAfterDelay(){
		Instantiate(Bubble, BubbleSpawnPosition.localPosition, BubbleSpawnPosition.rotation);
	}

	// Update is called once per frame
	void Update () {
		Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		difference.Normalize ();

		float rotateZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler (0f, 0f, rotateZ + rotationOffset);

		if (Input.GetButtonDown ("Fire1")) {
			StartCoroutine (DoDelay(0.1f, InstantiateAfterDelay));
		}
	}



}