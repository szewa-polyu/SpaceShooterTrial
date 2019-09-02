using UnityEngine;

public class BGScroller : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed;

    [SerializeField]
    private float tileSizeZ;
    
    private Vector3 startPosition;


    private void Start()
    {
        startPosition = transform.position;        
    }

    private void Update()
    {        
        float newPosition = Mathf.Repeat(Time.time * Mathf.Abs(scrollSpeed), tileSizeZ);
        transform.position = startPosition + Vector3.forward * newPosition * Mathf.Sign(scrollSpeed);
    }
}
