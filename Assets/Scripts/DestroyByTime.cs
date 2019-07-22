using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField]
    private float lifetime;


    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
