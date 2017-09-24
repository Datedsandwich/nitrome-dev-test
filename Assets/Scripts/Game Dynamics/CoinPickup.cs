using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {
	private ScoreManager scoreManager;

	// Use this for initialization
	void Start () {
		scoreManager = ScoreManager.instance;
	}
	
	void OnTriggerEnter2D (Collider2D other) {
        if (other.isTrigger && other.gameObject.CompareTag("Player")) {
			scoreManager.IncreaseScore(10f);
			Destroy(gameObject);
		}
    }
}
