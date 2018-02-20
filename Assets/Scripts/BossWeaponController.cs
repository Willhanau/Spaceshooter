using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponController : MonoBehaviour {

	public GameObject shot;
	public GameObject enemyShip;
	public Transform[] shotSpawn;
	public Transform shipSpawn;
	public float fireRate;
	public float delay;
	private GameController gameController;
	private AudioSource audioSource;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		else if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}
		audioSource = GetComponent<AudioSource>();
		InvokeRepeating("Fire", delay, fireRate);
	}

	void Fire()
	{
		if (!gameController.GetGameOver) {
			Instantiate (shot, shotSpawn [0].position, shotSpawn [0].rotation);
			Instantiate (shot, shotSpawn [1].position, shotSpawn [1].rotation);
			Instantiate (enemyShip, shipSpawn.position, shipSpawn.rotation);
			audioSource.Play ();
		}
	}
}
