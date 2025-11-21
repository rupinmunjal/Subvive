using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;

    public GameObject camera;
    
    void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            IsLocalPlayer();
        }
        else
        {
            IsRemotePlayer();
        }
    }
    
    public void IsLocalPlayer()
    {
        movement.enabled = true;
        camera.SetActive(true);
    }

    public void IsRemotePlayer()
    {
        movement.enabled = false;
        camera.SetActive(false);
    }
    
}

