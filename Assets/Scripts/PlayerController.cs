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

    [SerializeField]
    private AudioSource weaponAudio;

    private Coroutine spawnShotsRoutine;

    private IDictionary<RestrictPositionMode, Action> restrictPostionModeDict;

    private Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private Vector3 velocity { get { return rigidBody.velocity; } }


    private void Start()
    {
        restrictPostionModeDict = new Dictionary<RestrictPositionMode, Action>
        {
            { RestrictPositionMode.Clamp, RetrictPositionByClamp },
            { RestrictPositionMode.PeriodicBC, RetrictPositionByPeriodicBC }
        };

        StartSpawnShots();
    }

    private void FixedUpdate()
    {
        Accelerate();
        RestrictPosition();
        Tilt();
        //LogPosition();
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
        Vector3 myVelocity = velocity;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, myVelocity.x * tilt);
    }


    #region spawn shots

    private void StartSpawnShots()
    {
        spawnShotsRoutine = StartCoroutine(SpawnShots());
    }

    private void EndSpawnShots()
    {
        if (spawnShotsRoutine != null)
        {
            StopCoroutine(spawnShotsRoutine);
            spawnShotsRoutine = null;
        }
    }

    private IEnumerator SpawnShots()
    {
        float nextFire = 0.0f;

        while (true)
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                GameObject newShot = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

                Rigidbody newShotRigidBody = newShot.GetComponent<Rigidbody>();
                if (newShotRigidBody != null)
                {
                    newShotRigidBody.velocity += velocity;
                }

                weaponAudio.Play();
            }

            yield return null;
        }
    }       

    #endregion


    #region log

    private void LogPosition()
    {
        Debug.Log("position: " + position);
    }

    private void LogVelocity()
    {        
        Debug.Log("velocity: " + velocity);
    }    

    #endregion


    #region boundary conditions

    public void RetrictPositionByClamp()
    {
        Vector3 currentPosition = position;
        position = new Vector3
        (
            Mathf.Clamp(currentPosition.x, boundary.XMin, boundary.XMax),
            0.0f,
            Mathf.Clamp(currentPosition.z, boundary.ZMin, boundary.ZMax)
        );
    }

    private void RetrictPositionByPeriodicBC()
    {
        Vector3 currentPosition = position;  // cloned
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

        position = new Vector3
        (
            adjustedX,
            0.0f,
            adjustedZ
        );
    }

    #endregion
}
