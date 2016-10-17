using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Bubble : MonoBehaviour {
	public float moveSpeed = 30f;
    public int level = 3;

	[SerializeField]
	private Color bubbleColor;
	private List<Color> colorList;

	private Rigidbody currentRigidBody;

	private MeshRenderer meshRender;

	private bool hadACollision = false;

	enum BubbleState {Waiting, Movement, Stopped};

	private BubbleState currentState = BubbleState.Waiting;

	[SerializeField]
	private List<GameObject> sameColorConnectedBubbles;

	[SerializeField]
	private bool passedThroughEndGame = false;

	private float minimunHorizontalValue = -7f;
	private float maximunHorizontalValue = 7.5f;
	private float minimunVerticalValue = 2.44f;
	private float maximunVerticalValue = 13f;

	[SerializeField]
	private List<float> verticalValues;

	float checkRadius = 0.1f;

	Ray trace;
	Ray traceReflected;
	RaycastHit hit;

	void Awake(){
		colorList = new List<Color> ();
		verticalValues = new List<float> ();
		for (int i = 0; i < 14; i++) {
			verticalValues.Add (minimunVerticalValue + (0.88f * i));
		}
		colorList.Add (Color.black); colorList.Add (Color.blue);
		colorList.Add (Color.cyan); colorList.Add (Color.gray);
		colorList.Add (Color.gray); colorList.Add (Color.green);
		colorList.Add (Color.grey); colorList.Add (Color.magenta);
		colorList.Add (Color.red); colorList.Add (Color.yellow);
		colorList.Add (Color.white);
	}

	// Use this for initialization
	void Start () {
		sameColorConnectedBubbles = new List<GameObject> ();

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
			gameObject.name += " Movement";
			Vector3 direction = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
			direction.Normalize ();

			currentRigidBody.velocity = direction * moveSpeed;
		}
	}

	void GenerateBubbleColor (){
		bubbleColor = colorList [Random.Range (0, level)];
	}

	void OnCollisionEnter (Collision collision){
		/*if (collision.gameObject.name.Equals (gameObject.name)) {
			currentState = BubbleState.Stopped;
			hadACollision = true;
			Destroy (currentRigidBody);
			sameColorConnectedBubbles.Add (collision.gameObject);
			if (sameColorConnectedBubbles.Count >= 2) {
				foreach (GameObject bubble in sameColorConnectedBubbles) {
					Destroy (bubble);
				}
				Destroy (this.gameObject);
			}
		}*/
		if (collision.gameObject.name.Contains ("Up") || collision.gameObject.name.Contains ("Bubble") && collision.gameObject.name.Contains("Stopped")) {
			if (currentState == BubbleState.Movement) {
				currentState = BubbleState.Stopped;
				gameObject.name = "Bubble " + bubbleColor + " Stopped";
				hadACollision = true;
				Destroy (currentRigidBody);
				transform.localRotation = Quaternion.identity;
				Vector3 thePosition = transform.localPosition;
                thePosition.x = Mathf.Round (thePosition.x);
				thePosition.y = Mathf.Floor (thePosition.y);
				if (thePosition.y % 2 >= 1) {
					thePosition.x += 0.5f;
				}
                if (thePosition.y <= 3){
                    Debug.Log("[OnCollisionEnter]End Game");
                    SceneManager.LoadScene("GameOver");
                }
				Debug.Log (thePosition);
				Debug.Log ("[OnCollisionEnter]Position: " + Mathf.FloorToInt(thePosition.y) + " - " + 1 + " = " + (Mathf.FloorToInt(thePosition.y) - 1));
				thePosition.y = verticalValues[(Mathf.FloorToInt(thePosition.y) - 1)];

                //float calc = (13 % thePosition.y);
                //Debug.Log ("13 % " + thePosition.y + ": " + calc);
                //thePosition.y = 13 - ((13 % thePosition.y) * 0.85f);
                //Debug.Log (thePosition);
				transform.localPosition = thePosition;
			}
		}
		if (collision.gameObject.name.Contains ("Left") || collision.gameObject.name.Contains ("Right")) {
			Debug.Log ("[OnCollisionEnter]Collision contacts: " + collision.contacts [0].normal);
			currentRigidBody.velocity = Vector3.Reflect (transform.localPosition, collision.contacts [0].normal);
		}
	}
}
