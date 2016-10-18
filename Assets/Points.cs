using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Points : MonoBehaviour {
	public int score = 0;
	private Text scoreText;

	public static Points Instance{
		get;
		private set;
	}

	void Awake() {
		if (Instance == null) {
			Instance = this;
		}

		scoreText = GetComponent<Text> ();
	}


	void Update(){
		scoreText.text = "" + score;
	}
}
