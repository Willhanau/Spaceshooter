using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;
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
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			audioSource.Play ();
		}
    }

}
