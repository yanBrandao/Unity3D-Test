using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bubble : MonoBehaviour {
	[SerializeField]
	private Color bubbleColor;
	private List<Color> colorList;

	private MeshRenderer meshRender;

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
		GenerateBubbleColor ();
		meshRender = GetComponent<MeshRenderer> ();
		meshRender.material.color = bubbleColor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GenerateBubbleColor(){
		bubbleColor = colorList [Random.Range (0, TestControl.Instance.level)];
	}
}
