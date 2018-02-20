using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private GameController gameController;
	private int numHits;
	public int maxHits;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//if (other.tag == "Boundary" || other.tag == "Enemy")
		if(other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyProjectile"))
		{
			return;
		}

		if (explosion != null)
		{
			numHits++;
			if (numHits == maxHits) {
				Instantiate (explosion, transform.position, Quaternion.identity);
				gameController.AddScore (scoreValue);
				Destroy (gameObject);
			}
			if (other.CompareTag ("Player")) { 
				Instantiate (playerExplosion, other.transform.position, Quaternion.identity);
				gameController.GameOver ();
			}
		}
		Destroy (other.gameObject);
	}
}
