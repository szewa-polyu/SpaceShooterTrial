using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject hazard;

    [SerializeField]
    private Vector3 spawnValues;

    [SerializeField]
    private int hazardCount;

    [SerializeField]
    private float startWait;

    [SerializeField]
    private float spawnWait;

    [SerializeField]
    private float waveWait;

    
    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }


    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                GameObject newHazard = Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }
}
