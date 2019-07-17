using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Rigidbody rigidBody;    


    private void Start()
    {        
        rigidBody.AddForce(speed * transform.forward);
    }

    private void Update()
    {
        
    }
}
