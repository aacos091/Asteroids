using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject enemy;
    public GameObject bloodClot;
    public GameObject[] hazards;
    private int score;
    private int hiscore;
    private int asteroidsRemaining;
    private int lives;
    private int wave;
    private int increaseEachWave = 4;
    private float spawnTime;
    public int lifeTime;
    public int enemySpawnDelay;
    public float lastTimeSpawned;
    private bool enemySpawned;
    public int pointsForLives;

    private SceneController sceneControl;


    public Text scoreText;
    public Text livesText;
    public Text waveText;
    public Text hiscoreText;

    // Use this for initialization
    void Start()
    {

        hiscore = PlayerPrefs.GetInt("hiscore", 0);
        lastTimeSpawned = 0;
        enemySpawnDelay = (Random.Range(10, 15));
        BeginGame();
        sceneControl = GameObject.Find("SceneController").GetComponent<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {

        // Quit if player presses escape
        //if (Input.GetKey("escape"))
        //    Application.Quit();

        if (wave > 1)
        {
            if (Time.time > lastTimeSpawned + enemySpawnDelay)
            {
                if (enemySpawned == false)
                {
                    enemySpawned = true;
                    SpawnEnemy();
                    enemySpawnDelay = (Random.Range(30, 45));
                    spawnTime = Time.time;
                    lastTimeSpawned = Time.time;

                }

            }
            if (Time.time > lifeTime + spawnTime)
            {
                enemySpawned = false;
                Destroy(GameObject.FindWithTag("Enemy"));
            }
        }

        if (wave == 3) 
        {
            sceneControl.newScene("Win");
        }

    }

    private void FixedUpdate()
    {
        if (pointsForLives > 1000)
        {
            lives++;
            livesText.text = "LIVES: " + lives;
            pointsForLives = pointsForLives - 1000;
        }
    }

    void BeginGame()
    {

        score = 0;
        lives = 3;
        wave = 1;
        lastTimeSpawned = 0;
        pointsForLives = 0;

        // Prepare the HUD
        scoreText.text = "SCORE:" + score;
        hiscoreText.text = "HISCORE: " + hiscore;
        livesText.text = "LIVES: " + lives;
        waveText.text = "WAVE: " + wave;

        SpawnAsteroids();


    }


    void SpawnAsteroids()
    {

        DestroyExistingAsteroids();

        // Decide how many asteroids to spawn
        // If any asteroids left over from previous game, subtract them
        asteroidsRemaining = (wave * increaseEachWave);

        // Spawn a Blood Clot
        Instantiate(bloodClot,
            new Vector3(Random.Range(-9.0f, 9.0f),
                Random.Range(-6.0f, 6.0f), 0),
            Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f)));

        for (int i = 0; i < asteroidsRemaining; i++)
        {

            // Spawn an asteroid
            GameObject asteroid = hazards[Random.Range(0, hazards.Length)];
            Instantiate(asteroid,
                new Vector3(Random.Range(-9.0f, 9.0f),
                    Random.Range(-6.0f, 6.0f), 0),
                Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f)));

        }



        waveText.text = "WAVE: " + wave;
    }

    void SpawnEnemy()
    {
        // Randomizes direction enemy spawns from 1 = right 2 = left
        int enemyDirection = (Random.Range(1, 3));
        if (enemyDirection == 1)
        {
            Instantiate(enemy,
        new Vector3(12, Random.Range(-4.0f, 4.0f), 0),
        Quaternion.Euler(0, 0, 90));
        }
        else
        {
            Instantiate(enemy,
        new Vector3(-12, Random.Range(-4.0f, 4.0f), 0),
        Quaternion.Euler(0, 0, -90));
        }
    }

    public void IncrementScoreSmall()
    {
        score = score + 100;
        pointsForLives = pointsForLives + 100;

        scoreText.text = "SCORE:" + score;

        if (score > hiscore)
        {
            hiscore = score;
            hiscoreText.text = "HISCORE: " + hiscore;

            // Save the new hiscore
            PlayerPrefs.SetInt("hiscore", hiscore);
        }

        // Has player destroyed all asteroids?
        if (asteroidsRemaining < 1)
        {

            // Start next wave
            wave++;
            lastTimeSpawned = Time.time;
            SpawnAsteroids();

        }
    }

    public void IncrementScoreLarge()
    {
        score = score + 20;
        pointsForLives = pointsForLives + 20;

        scoreText.text = "SCORE:" + score;

        if (score > hiscore)
        {
            hiscore = score;
            hiscoreText.text = "HISCORE: " + hiscore;

            // Save the new hiscore
            PlayerPrefs.SetInt("hiscore", hiscore);
        }

        // Has player destroyed all asteroids?
        if (asteroidsRemaining < 1)
        {

            // Start next wave
            wave++;
            lastTimeSpawned = Time.time;
            SpawnAsteroids();

        }
    }

    public void IncrementScoreEnemy()
    {
        score = score + 1000;
        pointsForLives = pointsForLives + 1000;

        scoreText.text = "SCORE:" + score;

        if (score > hiscore)
        {
            hiscore = score;
            hiscoreText.text = "HISCORE: " + hiscore;

            // Save the new hiscore
            PlayerPrefs.SetInt("hiscore", hiscore);
        }

        // Has player destroyed all asteroids?
        if (asteroidsRemaining < 1)
        {

            // Start next wave
            wave++;
            lastTimeSpawned = Time.time;
            SpawnAsteroids();

        }
    }

    public void DecrementLives()
    {
        lives--;
        livesText.text = "LIVES: " + lives;

        // Has player run out of lives?
        if (lives < 1)
        {
            // Restart the game
            //BeginGame();

            // Go to the "Game Over" scene
            sceneControl.newScene("GameOver");
        }
    }

    public void DecrementAsteroids()
    {
        asteroidsRemaining--;
    }

    public void SplitAsteroid()
    {
        // Two extra asteroids
        // - big one
        // + 3 little ones
        // = 2
        asteroidsRemaining += 2;

    }

    void DestroyExistingAsteroids()
    {
        GameObject[] asteroids =
            GameObject.FindGameObjectsWithTag("Large Asteroid");

        foreach (GameObject current in asteroids)
        {
            GameObject.Destroy(current);
        }

        GameObject[] asteroids2 =
            GameObject.FindGameObjectsWithTag("Small Asteroid");

        foreach (GameObject current in asteroids2)
        {
            GameObject.Destroy(current);
        }

        // Destroy enemy
        Destroy(GameObject.FindWithTag("Enemy"));
    }

}
