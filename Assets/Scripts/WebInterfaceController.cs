using UnityEngine;
using System.Runtime.InteropServices;

public class WebInterfaceController : MonoBehaviour
{
    [SerializeField]
    private BGScaler bgScaler;

    [SerializeField]
    private Transform player;


    private void Update()
    {
        this.SendPlayerPositionAsString();
        this.SendPlayerPositionAsVector();        
    }


    private string GetPlayerPositionAsString()
    {
        return player != null ? player.position.ToString() : Vector3.zero.ToString();
    }

    private Vector3 GetPlayerPositionAsVector()
    {
        return player != null ? player.position : Vector3.zero;
    }


    // https://github.com/elraccoone/react-unity-webgl/wiki/Communication-from-React-to-Unity
    #region communication from web to unity

    public void SetBgScale(float aScale)
    {
        bgScaler.Scale = aScale;
    }

    #endregion


    // https://github.com/elraccoone/react-unity-webgl/wiki/Communication-from-Unity-to-React
    #region communication from unity to web

    [DllImport("__Internal")]
    private static extern void SendPlayerPositionAsString(string position);
    
    private void SendPlayerPositionAsString()
    {
        WebInterfaceController.SendPlayerPositionAsString(GetPlayerPositionAsString());
    }

    [DllImport("__Internal")]
    private static extern void SendPlayerPositionAsVector(Vector3 position);

    private void SendPlayerPositionAsVector()
    {
        WebInterfaceController.SendPlayerPositionAsVector(GetPlayerPositionAsVector());
    }

    #endregion
}
