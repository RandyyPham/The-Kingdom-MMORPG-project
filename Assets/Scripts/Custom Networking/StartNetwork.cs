using Unity.Netcode;
using UnityEngine;

public class StartNetwork : MonoBehaviour
{
    [SerializeField] private GameObject ServerCanvas;

    private void Awake()
    {
        ServerCanvas.SetActive(false);
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        ServerCanvas.SetActive(true);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }
}
