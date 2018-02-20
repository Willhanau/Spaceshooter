using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

	void OnTriggerExit(Collider other)
    {
		if(other.CompareTag("Projectile") || other.CompareTag("EnemyProjectile")){
			Destroy(other.gameObject);
		}else{
			float xPos = other.transform.position.x;
			float yPos = other.transform.position.y;
			other.transform.position = new Vector3 (xPos, yPos, 16.0f);
		}
	}
}
