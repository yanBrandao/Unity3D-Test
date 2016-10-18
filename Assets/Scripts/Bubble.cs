using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Bubble : MonoBehaviour {
	public float moveSpeed = 30f;
    public int level = 3;
	int pointsToScore;

    public GameObject bubbleEffect;
    public GameObject points;
    private TextMesh textPoints;

    [SerializeField]
	private Color bubbleColor;
	private List<Color> colorList;

	private Rigidbody currentRigidBody;

	private MeshRenderer meshRender;

	private bool hadACollision = false;

	enum BubbleState {Waiting, Movement, Stopped};

	private BubbleState currentState = BubbleState.Waiting;

	[SerializeField]
	private List<GameObject> sameColorConnectedBubbles; //Will contain a bubble with same color reference that collided with this bubble.

	[SerializeField]
	private bool passedThroughEndGame = false;

	private float minimunHorizontalValue = -7f;
	private float maximunHorizontalValue = 7.5f;
	private float minimunVerticalValue = 2.44f;
	private float maximunVerticalValue = 13f;
    private float horizontalOffset = 0.5f;
    private float verticalOffset = 0.88f;

	[SerializeField]
	private List<float> verticalValues;

	float checkRadius = 0.1f;

	Ray trace;
	Ray traceReflected;
	RaycastHit hit;

	void Awake(){
		colorList = new List<Color> ();
		verticalValues = new List<float> (); // Vertical values contains every possible position that bubble can be stay.
		for (int i = 0; i < 14; i++) {
			verticalValues.Add (minimunVerticalValue + (verticalOffset * i));
		}

		// Added possible colors that bubble can be colored.
		colorList.Add (Color.red); colorList.Add (Color.blue); colorList.Add (Color.green); colorList.Add (Color.white);
		colorList.Add (Color.magenta); colorList.Add (Color.HSVToRGB(330, 59,100)); colorList.Add (Color.HSVToRGB(39,100,100)); colorList.Add (Color.HSVToRGB(240, 100,50));

	}

	// Use this for initialization
	void Start () {
		sameColorConnectedBubbles = new List<GameObject> ();
		GenerateBubbleColor ();
        textPoints = points.GetComponent<TextMesh>();
        
		currentRigidBody = GetComponent<Rigidbody> ();
		meshRender = GetComponent<MeshRenderer> ();
		meshRender.material.color = bubbleColor;
		gameObject.name = "Bubble " + bubbleColor;
        TestControl.Instance.bubbleList.Add(this.gameObject); // Added this bubble to a list that contains every bubble in scene.
    }
	
	// Update is called once per frame
	void Update () {
		/* After every shoot, will be instantiate a new bubble on waiting state and
		 * current bubble will set to Movement and got force direct to touch or mouse rotation and position.
		 **/
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
		/* This first check stopped bubble and change his current position to work like a grid.
		 * The horizontal offset has 1 for odd Y axis and 0.5 for even Y axis;
		 * The vertical offset has 0.88f for every X axis, starting from 13 on top of the box.
		**/
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
                Debug.Log("[OnCollisionEnter] Before Round:" + thePosition);
                if (thePosition.y % 2 >= 1) {
                    if (thePosition.x + horizontalOffset >= maximunHorizontalValue)
                        thePosition.x = maximunHorizontalValue;
                    else
                        thePosition.x += horizontalOffset;
                } else {
                    if (thePosition.x + horizontalOffset >= maximunHorizontalValue)
                        thePosition.x = maximunHorizontalValue - horizontalOffset;
                }
                if (thePosition.y <= 3){
                    Debug.Log("[OnCollisionEnter]End Game");
                    SceneManager.LoadScene("GameOver");
                }
				

                Debug.Log ("[OnCollisionEnter]Position: " + Mathf.FloorToInt(thePosition.y) + " - " + 1 + " = " + (Mathf.FloorToInt(thePosition.y) - 1));
				thePosition.y = verticalValues[(Mathf.FloorToInt(thePosition.y) - 1)];
                Debug.Log("[OnCollisionEnter] After Round:" + thePosition);
                transform.localPosition = Vector3.one * 100;
				//The offset will be set in the returnCorrectPosition after check where bubble can be place.
				transform.localPosition = returnCorretPosition(thePosition);
			}
			/*This check will create a list of same colors neighborhood.
			 * If after this verification there are more then 2 bubble connecteds
			 * Particles will be call and Points will be calculated.
			 **/
            if (collision.gameObject.name.Equals(gameObject.name)) {
                sameColorConnectedBubbles.Add(collision.gameObject);
                if (sameColorConnectedBubbles.Count >= 2) {
					pointsToScore = (5 * sameColorConnectedBubbles.Count);
					textPoints.text = "" + pointsToScore;
                    foreach (GameObject bubble in sameColorConnectedBubbles) {
                        Object effectParticle = Instantiate(bubbleEffect, bubble.transform.localPosition, bubble.transform.localRotation);
                        Debug.Log("[OnCollisionEnter] Points: <color=blue>" + textPoints.text + "</color>");
                        Object pointsInstantiated = Instantiate(points, bubble.transform.localPosition, bubble.transform.localRotation);
						Points.Instance.score += pointsToScore;
                        Destroy(pointsInstantiated, 0.5f);
                        Destroy(effectParticle, 0.5f);
                        Destroy(bubble);
                    }
                    Object effect = Instantiate(bubbleEffect, gameObject.transform.localPosition, gameObject.transform.localRotation);
                    Destroy(effect, 0.5f);
                    Debug.Log("[OnCollisionEnter] Points: <color=blue>" + textPoints.text + "</color>");
					Points.Instance.score += pointsToScore;
                    Object pointsInst = Instantiate(points, gameObject.transform.localPosition, gameObject.transform.localRotation);
                    Destroy(pointsInst, 0.5f);
                    Destroy(this.gameObject);
                }
            }
        }
		if (collision.gameObject.name.Contains ("Left") || collision.gameObject.name.Contains ("Right")) {
			Debug.Log ("[OnCollisionEnter]Collision contacts: " + collision.contacts [0].normal);
			currentRigidBody.velocity = Vector3.Reflect (transform.localPosition, collision.contacts [0].normal);
		}
		/*This check verify if the collision is a collectable
		 * If is true, the power up will destroy every bubble with the same color using bubble list created before.
		 */
        if (collision.gameObject.tag.Equals("Collectable")) {
            Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.Normalize();
            currentRigidBody.velocity = direction * moveSpeed;
            int count = 1;
			pointsToScore = 5;
			Points.Instance.score += pointsToScore;
            foreach(GameObject _bubble in TestControl.Instance.bubbleList){
                if (_bubble.name.Contains(bubbleColor.ToString())) {
                    Object effectParticle = Instantiate(bubbleEffect, _bubble.transform.localPosition, _bubble.transform.localRotation);
                    Destroy(effectParticle, 0.5f);
                    textPoints.text = "" + (5 * count++);
					pointsToScore = (5 * count);
					Points.Instance.score += pointsToScore;
                    Debug.Log("[OnCollisionEnter] Points: <color=blue>" + textPoints.text + "</color>");
                    Object pointsInstantiated = Instantiate(points, _bubble.transform.localPosition, _bubble.transform.localRotation);
                    Destroy(pointsInstantiated, 0.5f);
                    Destroy(_bubble);
                }
            }
            Object effect = Instantiate(bubbleEffect, gameObject.transform.localPosition, gameObject.transform.localRotation);
            Destroy(effect, 0.5f);
            textPoints.text = "" + (5 * count);
			pointsToScore = (5 * count);
			Points.Instance.score += pointsToScore;
            Debug.Log("[OnCollisionEnter] Points: <color=blue>" + textPoints.text + "</color>");
            Object pointsInst = Instantiate(points, gameObject.transform.localPosition, gameObject.transform.localRotation);
            Destroy(pointsInst, 0.5f);
            Destroy(this.gameObject);

        }
	}
    
	/* This function try to offset bubbles 
	 */
    Vector3 returnCorretPosition(Vector3 objectPosition){
        Vector3 newPosition = Vector3.zero;
        if(!Physics.CheckSphere(objectPosition, checkRadius)) {
            Debug.Log("<color=red>[returnCorretPosition]</color>: Current position is available.");
            newPosition = objectPosition;
        } else {
			objectPosition.y -= verticalOffset; objectPosition.x -= horizontalOffset;
            if(!Physics.CheckSphere(objectPosition, checkRadius)) {
                Debug.Log("<color=red>[returnCorretPosition]</color>: Top left is available.");
                newPosition = objectPosition;
            } else {
				objectPosition.x += horizontalOffset*2;
                if (!Physics.CheckSphere(objectPosition, checkRadius)) {
                    Debug.Log("<color=red>[returnCorretPosition]</color>: Top right is available.");
                    newPosition = objectPosition;
                } else {
					objectPosition.y += verticalOffset*2;
                    if (!Physics.CheckSphere(objectPosition, checkRadius)) {
                        Debug.Log("<color=red>[returnCorretPosition]</color>: Bottom right is available.");
                        newPosition = objectPosition;
                    } else {
						objectPosition.x -= horizontalOffset*2;
                        if (!Physics.CheckSphere(objectPosition, checkRadius))
                        {
                            Debug.Log("<color=red>[returnCorretPosition]</color>: Bottom left is available.");
                            newPosition = objectPosition;
                        } else {
                            Debug.Log("<color=red>[returnCorretPosition]</color>: There's no position to fixed this object. (Quitting life!)");
                        }
                    }
                }
            }
        }
        return newPosition;
    }

    
}
