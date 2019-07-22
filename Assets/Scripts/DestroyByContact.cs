using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private GameObject playerExplosion;

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
        }

        Destroy(other.gameObject);
        Destroy(gameObject);     
    }
}
