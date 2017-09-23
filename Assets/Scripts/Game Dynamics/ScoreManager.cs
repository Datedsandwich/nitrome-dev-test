using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public static ScoreManager instance = null;

	public float score;
	[SerializeField]
	private Text scoreText;

	void Awake() {
		if(instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}

		 DontDestroyOnLoad(gameObject);
		 Reset();
	}

	public void Reset() {
		score = 0f;
		UpdateScoreText();
	}

	public void IncreaseScore(float amount) {
		score += amount;
		UpdateScoreText();
	}

	private void UpdateScoreText() {
		scoreText.text = "Score: " + score;
	}
}
