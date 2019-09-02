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
        WebInterfaceController.SendPlayerPositionAsString(player.position.ToString());
    }

    [DllImport("__Internal")]
    private static extern void SendPlayerPositionAsVector(string position);

    private void SendPlayerPositionAsVector()
    {
        WebInterfaceController.SendPlayerPositionAsVector(player.position.ToString());
    }

    #endregion
}
