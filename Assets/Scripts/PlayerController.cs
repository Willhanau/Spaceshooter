using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;
    public GameObject shot;
    public Transform[] shotSpawns;
    public float fireRate;
    public float nextFire;
    private int powerLevel;
	private GameController gameController;

#if UNITY_IOS
    //mobile platform
    public SimpleTouchPad touchPad;
    public SimpleCanTouchArea areaButton;
    private Quaternion calibrationQuaternion;
#endif

	public int PowerLevel {
		set{ powerLevel = value; }
		get{ return powerLevel; }
	}

	public int NumberShotSpawns {
		get{ return shotSpawns.Length-1; }
	}

    void Start()
    {
#if UNITY_IOS
        CalibrateAccelerometer();
#endif
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		else if (gameController == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}
		powerLevel = gameController.GetSavedPowerLevel;
    }

    void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetButton("Fire1") && Time.time > nextFire)
#endif
#if UNITY_IOS 
        if (areaButton.CanFire() == true && Time.time > nextFire)
#endif
        {
            nextFire = Time.time + fireRate;

            for(int i = 0; i <= powerLevel; i++)
            {
                Instantiate(shot, shotSpawns[i].position, shotSpawns[i].rotation);
            }
            GetComponent<AudioSource>().Play();
        }
    }

	void FixedUpdate()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
#endif

        //Motion Controls
        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //Vector3 accelerationRaw = Input.acceleration;
        //Vector3 acceleration = FixAcceleration(accelerationRaw);

#if UNITY_IOS
        //Touch Controls
        Vector2 direction = touchPad.GetDirection();
        Vector3 movement = new Vector3(direction.x, 0.0f, direction.y);
#endif

        GetComponent<Rigidbody>().velocity = movement * speed;
        GetComponent<Rigidbody>().position = new Vector3(
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);

    }

#if UNITY_IOS
    //Used to calibrate the Input.acceleration input for mobile platforms*
    void CalibrateAccelerometer()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotationQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotationQuaternion);
    }

    //Get the 'calibrated' value from the Input
    Vector3 FixAcceleration(Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }
#endif

}
