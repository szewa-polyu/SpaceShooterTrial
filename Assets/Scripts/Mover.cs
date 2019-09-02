using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    [Range(0.01f, Single.MaxValue)]
    private float speedAugmentedFactor;

    private float minSpeed { get { return speed / speedAugmentedFactor; } }
    private float maxSpeed { get { return speed * speedAugmentedFactor; } }
    private float speedToUse { get { return UnityEngine.Random.Range(minSpeed, maxSpeed); } }

    [SerializeField]
    private Rigidbody rigidBody;    


    private void Start()
    {        
        rigidBody.AddForce(speedToUse * transform.forward);
    }
}
