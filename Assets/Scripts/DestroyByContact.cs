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
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }

        if (explosion != null)
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.CompareTag("Player"))
        {
            Transform otherTransform = other.transform;
            GameObject newPlayerExplosion = Instantiate(playerExplosion, otherTransform.position, otherTransform.rotation);
            gameController.GameOver();
        }

        //gameController.AddScore(scoreValue);
        Destroy(other.gameObject);
        Destroy(gameObject);     
    }
}
