using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSetter : MonoBehaviour {
	private Text scoreText;

	void Awake () {
		scoreText = GetComponent <Text> ();
		scoreText.text = "Score: " + GameManager.score;
	}
}
