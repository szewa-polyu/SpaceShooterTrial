using System;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private AudioSource weaponAudio;

    [SerializeField]
    private AudioClip[] weaponClips;

    [SerializeField]
    private GameObject shot;

    [SerializeField]
    private Transform shotSpawn;

    [SerializeField]
    [Range(0.01f, Single.MaxValue)]
    private float firePeriod;

    [SerializeField]
    [Range(0.01f, Single.MaxValue)]
    private float firePeriodAugmentedFactor;

    private float minFirePeriod { get { return firePeriod / firePeriodAugmentedFactor; } }
    private float maxFirePeriod { get { return firePeriod * firePeriodAugmentedFactor; } }
    private float firePeriodToUse { get { return UnityEngine.Random.Range(minFirePeriod, maxFirePeriod); } }

    [SerializeField]
    [Range(0.01f, Single.MaxValue)]
    private float delay;

    private Coroutine fireRoutine;

    [SerializeField]
    private Rigidbody rigidBody;

    private Vector3 velocity { get { return rigidBody.velocity; } }


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        StartFire();
    }


    #region fire

    private void StartFire()
    {
        fireRoutine = StartCoroutine(Fire());
    }

    private void EndFire()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
            fireRoutine = null;
        }
    }

    private IEnumerator Fire()
    {
        float nextFire = 0.0f;

        while (true)
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + firePeriodToUse;
                GameObject newShot = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

                Rigidbody newShotRigidBody = newShot.GetComponent<Rigidbody>();
                if (newShotRigidBody != null)
                {
                    newShotRigidBody.velocity += velocity;
                }

                weaponAudio.clip = weaponClips[UnityEngine.Random.Range(0, weaponClips.Length)];
                weaponAudio.Play();
            }

            yield return null;
        }
    }

    #endregion
}
