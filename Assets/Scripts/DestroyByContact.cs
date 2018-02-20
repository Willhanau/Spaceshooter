using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private GameController gameController;

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
            Instantiate(explosion, transform.position, Quaternion.identity);
        }

        //if (other.tag == "Player") 
        if(other.CompareTag("Player"))
        { 
            Instantiate(playerExplosion, other.transform.position, Quaternion.identity);
            gameController.GameOver();
        }
        gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
