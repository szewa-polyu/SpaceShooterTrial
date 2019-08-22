using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject hazard;

    [SerializeField]
    private Vector3 spawnValues;

    [SerializeField]
    private int hazardCount;

    [SerializeField]
    private float startWait;

    [SerializeField]
    private float spawnWait;

    [SerializeField]
    private float waveWait;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text restartText;

    [SerializeField]
    private Text gameOverText;

    private int _score;
    private int score
    {
        get { return _score; }
        set
        {
            _score = value;
            UpdateScore();
        }
    }

    private bool isGameOver;
    private bool isRestart;
   
    
    private void Start()
    {
        isGameOver = false;
        isRestart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }
    }


    #region privates

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                GameObject newHazard = Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (isGameOver)
            {
                restartText.text = "Press 'R' to restart";
                isRestart = true;
                break;
            }
        }
    }

    private void UpdateScore()
    {
        scoreText.text = $"Score: {score}";
    }

    #endregion


    #region publics

    public void AddScore(int increment)
    {
        score += increment;
    }

    public void GameOver()
    {
        gameOverText.text = "Game over!";
        isGameOver = true;
    }

    #endregion
}
