using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
	public GameObject enemyBossShip;
	public PlayerController player;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

	public Text livesText;
    public Text scoreText;
    public Text waveCountText;
    public Text gameOverText;
    public GameObject restartButton;
	public GameObject optionMenu;
	public Slider volumeSlider;

    private bool gameOver;
    private int score;
    private int waveCount;
	private int numLives = 3;
	private Vector3 playerSpawn = new Vector3 (0, 0, 0);
	public GameObject playerPrefab;
	private int savedPowerLevel = 0;
	private int numBosses = 1;

    void Start()
    {
        gameOver = false;
		UpdateLives ();
        gameOverText.text = "";
        restartButton.SetActive(false);
        score = 0;
        UpdateScore();
        waveCount = 1;
        UpdateWaveCount();
		volumeSlider.value = AudioListener.volume;
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
		while(!gameOver)
        {
			if (waveCount % 5 == 0) {
				for (int i = 0; i < numBosses; i++) {
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate (enemyBossShip, spawnPosition, spawnRotation);
				}
				numBosses++;
			}
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }

            hazardCount++;
            if(spawnWait != 0.02f)
            {
                spawnWait -= 0.02f;
            }

            yield return new WaitForSeconds(waveWait);

            waveCount++;
            UpdateWaveCount();
            if(waveWait != 0.05f)
            {
                waveWait -= 0.05f;
            }

        }
    }

	public bool GetGameOver {
		get{ return gameOver; }
	}

	public int GetSavedPowerLevel{
		get{ return savedPowerLevel; }
	}

    public void UpdatePowerLevel()
    {
		if (savedPowerLevel != player.NumberShotSpawns)
		{
			savedPowerLevel = (this.score / 100);
			player.PowerLevel = savedPowerLevel; 
		}
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
		UpdatePowerLevel ();
    }

	void UpdateLives(){
		livesText.text = "Lives left: " + numLives;
	}

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateWaveCount()
    {
        waveCountText.text = "On Wave: " + waveCount;
    }

    public void GameOver()
    {
		if (numLives <= 0) {
			gameOverText.text = "Game Over!";
			restartButton.SetActive (true);
			gameOver = true;
		} else {
			RespawnPlayer ();
		}
    }

	private void RespawnPlayer(){
		numLives--;
		GameObject newPlayer = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);
		this.player = newPlayer.GetComponent<PlayerController>();
		UpdateLives ();
		StartCoroutine(RespawnInvulnerability(newPlayer));
	}

	IEnumerator RespawnInvulnerability(GameObject newPlayer){
		newPlayer.GetComponent<MeshCollider> ().enabled = false;
		newPlayer.GetComponent<Renderer> ().material.color = Color.red;
		yield return new WaitForSeconds(3.0f);
		newPlayer.GetComponent<Renderer> ().material.color = Color.white;
		newPlayer.GetComponent<MeshCollider> ().enabled = true;
	}

	public void OpenOptions()
	{
		Time.timeScale = 0;
		optionMenu.SetActive (true);
	}

	public void UnPauseGame()
	{
		optionMenu.SetActive (false);
		Time.timeScale = 1;
	}

    public void RestartGame()
    {
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }

	public void ChangeVolume(float value)
	{
		AudioListener.volume = value;
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

}
