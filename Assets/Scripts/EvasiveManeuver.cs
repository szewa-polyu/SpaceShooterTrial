using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour
{
    [SerializeField]
    private float dodge;

    [SerializeField]
    private float smoothing;

    [SerializeField]
    private float tilt;

    [SerializeField]
    private Vector2 startWait;    

    [SerializeField]
    private Vector2 maneuverTime;

    [SerializeField]
    private Vector2 maneuverWait;

    private float targetManeuver;

    private Transform player;
    private bool isPlayerExist;

    [SerializeField]
    private Rigidbody rigidBody;

    private Vector3 position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private Vector3 velocity
    {
        get { return rigidBody.velocity; }
        set { rigidBody.velocity = value; }
    }

    [SerializeField]
    private RestrictPositionMode restrictPositionMode;

    [SerializeField]
    private Boundary boundary;

    private IDictionary<RestrictPositionMode, Action> restrictPostionModeDict;

    private Coroutine evadeCoroutine;

    private Func<float, float, float> randomRange = UnityEngine.Random.Range;


    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        isPlayerExist = playerObj != null;
        if (isPlayerExist)
        {
            player = playerObj.transform;
        }

        restrictPostionModeDict = new Dictionary<RestrictPositionMode, Action>
        {
            { RestrictPositionMode.Clamp, RetrictPositionByClamp },
            { RestrictPositionMode.PeriodicBC, RetrictPositionByPeriodicBC }
        };
        StartEvade();
    }

    private void FixedUpdate()
    {        
        SetVelocity();
        RestrictPosition();
        Tilt();     
    }


    #region fixed update routines

    private void SetVelocity()
    {
        Vector3 myVelocity = velocity;
        float newManeuver = Mathf.MoveTowards(myVelocity.x, targetManeuver, Time.deltaTime * smoothing);
        velocity = new Vector3(newManeuver, 0.0f, myVelocity.z);
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


    #region evade

    private void StartEvade()
    {
        evadeCoroutine = StartCoroutine(Evade());
    }

    private void StopEvade()
    {
        if (evadeCoroutine != null)
        {

            StopCoroutine(evadeCoroutine);
        }
    }

    private IEnumerator Evade()
    {
        yield return new WaitForSeconds(randomRange(startWait.x, startWait.y));

        while (true)
        {
            //targetManeuver = randomRange(1, dodge) * -Mathf.Sign(transform.position.x);
            targetManeuver = isPlayerExist ? player.position.x : randomRange(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(randomRange(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(randomRange(maneuverWait.x, maneuverWait.y));
        }
    }

    #endregion
}