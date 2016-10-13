using UnityEngine;
using System.Collections;

public class SelfRotate : MonoBehaviour {

	public float yRotationSpeed = 10f;
	public float xRorationSpeed = 5f;
	public float zRorationSpeed = 5f;
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up, yRotationSpeed * Time.deltaTime);
		transform.Rotate (Vector3.right, xRorationSpeed * Time.deltaTime);
		transform.Rotate (Vector3.forward, zRorationSpeed * Time.deltaTime);
	}
}
