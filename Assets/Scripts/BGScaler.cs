using UnityEngine;

public class BGScaler : MonoBehaviour
{
    public float Scale;

    [SerializeField]
    private Transform background;    


    private void Start()
    {
        Scale = background.localScale.y;
    }

    private void Update()
    {
        Vector3 originalScale = background.localScale;
        background.localScale = new Vector3(originalScale.x, Scale, originalScale.z);
    }
}
