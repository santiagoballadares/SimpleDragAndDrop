using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	public static int score;
	public Text scoreText;
	private Cell[] cells;

	void Awake () {
		cells = GetComponentsInChildren <Cell> ();
	}

	void Start () {
		score = 0;
		GameManager.score = 0;
		UpdateScore ();
	}

	void OnItemPlaced () {
		score = 0;

		foreach (Cell c in cells) {
			Item item = c.GetComponentInChildren <Item> ();

			if (item != null) {
				if (c.cellCode == item.itemCode) {
					score += item.itemScorePoints;
				}
			}
		}

		Debug.Log ("Current score: " + score);
		AddScore (score);
		GameManager.score = score;
	}

	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}

	void UpdateScore () {
		scoreText.text = "Score: " + score;
	}
}
