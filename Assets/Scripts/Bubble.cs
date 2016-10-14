using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bubble : MonoBehaviour {
	public float moveSpeed = 10f;

	[SerializeField]
	private Color bubbleColor;
	private List<Color> colorList;

	private Rigidbody currentRigidBody;

	private MeshRenderer meshRender;

	private bool hadACollision = false;

	enum BubbleState {Waiting, Movement, Stopped};

	private BubbleState currentState = BubbleState.Waiting;

	public List<GameObject> connectedBubbles;

	Ray a;
	Ray b;
	RaycastHit hit;

	void Awake(){
		colorList = new List<Color> ();
		colorList.Add (Color.black); colorList.Add (Color.blue);
		colorList.Add (Color.cyan); colorList.Add (Color.gray);
		colorList.Add (Color.gray); colorList.Add (Color.green);
		colorList.Add (Color.grey); colorList.Add (Color.magenta);
		colorList.Add (Color.red); colorList.Add (Color.yellow);
		colorList.Add (Color.white);
	}

	// Use this for initialization
	void Start () {
		connectedBubbles = new List<GameObject> ();
		GenerateBubbleColor ();
		currentRigidBody = GetComponent<Rigidbody> ();
		meshRender = GetComponent<MeshRenderer> ();
		meshRender.material.color = bubbleColor;
		gameObject.name = "Bubble " + bubbleColor;
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetButtonDown ("Fire1") && currentState == BubbleState.Waiting) {
			currentState = BubbleState.Movement;
			Vector3 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			difference.Normalize ();

			float rotateZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.Euler (0f, 0f, rotateZ - 90);
		}
		if (hadACollision == false && (currentState != BubbleState.Waiting && currentState != BubbleState.Stopped)) {
			transform.Translate (Vector3.up * Time.deltaTime * moveSpeed);
		}

		if (currentState == BubbleState.Movement) {
			a = new Ray (transform.position, transform.up);
			if (Deflect (a, out b, out hit)) {
				Debug.Log ("Deflected");
				Debug.DrawLine (a.origin, hit.point);
				Debug.DrawLine (b.origin, b.origin + 10 * b.direction);

			}
		}
	}

	void GenerateBubbleColor (){
		bubbleColor = colorList [Random.Range (0, TestControl.Instance.level)];
	}

	void OnCollisionEnter (Collision collision){
		Debug.Log(collision.gameObject.name);
		if (collision.gameObject.name.Contains ("Up") || collision.gameObject.name.Contains ("Bubble")) {
			currentState = BubbleState.Stopped;
			hadACollision = true;
			Destroy (currentRigidBody);
		}
		if (collision.gameObject.name.Contains ("Left") || collision.gameObject.name.Contains ("Right")) {
			float rotateAngle = Mathf.Atan2 (b.origin.y, b.origin.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 0f, rotateAngle);
		}
	}

	bool Deflect (Ray ray, out Ray deflected, out RaycastHit hit){
		if (Physics.Raycast (ray, out hit)) {
			Vector3 normal = hit.normal;
			Vector3 deflect = Vector3.Reflect (ray.direction, normal);

			deflected = new Ray (hit.point, deflect);
			return true;
		}

		deflected = new Ray (Vector3.zero, Vector3.zero);
		return false;
	}
}
