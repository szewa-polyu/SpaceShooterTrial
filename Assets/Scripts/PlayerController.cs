using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RestrictPositionMode
{
    Clamp,
    PeriodicBC
}

[Serializable]
public struct Boundary
{
    public float XMin, XMax, ZMin, ZMax;
}


public class PlayerController : MonoBehaviour
{            
    [SerializeField]
    private float accelerationFactor;

    [SerializeField]
    private float tilt;

    [SerializeField]
    private RestrictPositionMode restrictPositionMode;

    [SerializeField]
    private Boundary boundary;

    [SerializeField]
    private Rigidbody rigidBody;

    [SerializeField]
    public GameObject shot;

    [SerializeField]
    public Transform shotSpawn;

    [SerializeField]
    private float fireRate = 0.5f;

    private float nextFire = 0.0f;

    private IDictionary<RestrictPositionMode, Action> restrictPostionModeDict;


    private void Start()
    {
        restrictPostionModeDict = new Dictionary<RestrictPositionMode, Action>
        {
            { RestrictPositionMode.Clamp, RetrictPositionByClamp },
            { RestrictPositionMode.PeriodicBC, RetrictPositionByPeriodicBC }
        };

        StartSpawnShot();
    }

    private void FixedUpdate()
    {
        Accelerate();
        RestrictPosition();
        Tilt();
        LogPosition();
        //LogVelocity();
    }


    private void Accelerate()
    {
        float deltaTime = Time.deltaTime;

        // 0 < value < 1
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 acceleration = new Vector3(moveHorizontal, 0.0f, moveVertical) * accelerationFactor;
        //Debug.Log("acceleration: " + acceleration);

        rigidBody.AddForce(acceleration);
    }

    private void RestrictPosition()
    {
        restrictPostionModeDict[restrictPositionMode]();                
    }

    private void Tilt()
    {
        Vector3 velocity = rigidBody.velocity;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, velocity.x * tilt);
    }

    private void SpawnShot()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject newShot = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

            Rigidbody newShotRigidBody = newShot.GetComponent<Rigidbody>();
            if (newShotRigidBody != null)
            {
                newShotRigidBody.velocity = rigidBody.velocity;
            }
        }
    }

    private IEnumerator SpawnShots()
    {
        while (true)
        {
            yield return null;
            SpawnShot();
        }
    }

    private void StartSpawnShot()
    {
        StartCoroutine(SpawnShots());
    }


    #region log

    private void LogPosition()
    {
        Debug.Log("position: " + transform.position);
    }

    private void LogVelocity()
    {        
        Debug.Log("velocity: " + rigidBody.velocity);
    }    

    #endregion


    #region boundary conditions

    public void RetrictPositionByClamp()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3
        (
            Mathf.Clamp(currentPosition.x, boundary.XMin, boundary.XMax),
            0.0f,
            Mathf.Clamp(currentPosition.z, boundary.ZMin, boundary.ZMax)
        );
    }

    private void RetrictPositionByPeriodicBC()
    {
        Vector3 currentPosition = transform.position;  // cloned
        float adjustedX = currentPosition.x;
        float adjustedZ = currentPosition.z;        

        // left bound
        if (currentPosition.x < boundary.XMin)
        {
            adjustedX = boundary.XMax - (boundary.XMin - currentPosition.x);
        }

        // right bound
        if (currentPosition.x > boundary.XMax)
        {
            adjustedX = boundary.XMin + (currentPosition.x - boundary.XMax);
        }

        // top bound
        if (currentPosition.z < boundary.ZMin)
        {
            adjustedZ = boundary.ZMax - (boundary.ZMin - currentPosition.z);            
        }

        // bottom bound
        if (currentPosition.z > boundary.ZMax)
        {
            adjustedZ = boundary.ZMin + (currentPosition.z - boundary.ZMax);
        }

        transform.position = new Vector3
        (
            adjustedX,
            0.0f,
            adjustedZ
        );
    }

    #endregion
}
