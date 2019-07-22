using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;

    [SerializeField]
    private Rigidbody rigidbody;

    private Vector3 angularVelocity
    {
        get { return rigidbody.angularVelocity; }
        set { rigidbody.angularVelocity = value; }
    }
    

    private void Start()
    {
        angularVelocity = Random.insideUnitSphere * tumble;
    }

    private void Update()
    {
        
    }
}
