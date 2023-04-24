using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;


public class AtlasNetworkManager : MonoBehaviour
{
    private NetworkManager networkManager;
    public static AtlasNetworkManager instance;
    public TMP_InputField ipField;

    public UnityTransport Transport => (UnityTransport)networkManager.NetworkConfig.NetworkTransport;

    private void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    public void SetIp(string ip)
    {
        ip = ipField.text;
        Transport.ConnectionData.Address = ip;
    }
    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Singleton;
    }


}
