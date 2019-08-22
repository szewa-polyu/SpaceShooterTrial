using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private GameObject playerExplosion;

    [SerializeField]
    private int scoreValue;
    
    private GameController gameController;


    private void Start()
    {
        GameObject gameControllerObj = GameObject.FindWithTag("GameController");
        if (gameControllerObj != null)
        {
            gameController = gameControllerObj.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;
        }

        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);

        if (other.tag == "Player")
        {
            Transform otherTransform = other.transform;
            GameObject newPlayerExplosion = Instantiate(playerExplosion, otherTransform.position, otherTransform.rotation);
            gameController.GameOver();
        }

        gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);     
    }
}
