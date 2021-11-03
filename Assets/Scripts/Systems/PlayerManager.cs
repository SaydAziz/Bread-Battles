using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    [SerializeField] GameObject controller;
    [SerializeField] PlayerInfo player;
 
    private List<PlayerInfo> playersInRoom = new List<PlayerInfo>();

    
    void Awake()
    {
        PV = GetComponent<PhotonView>(); 

    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        

    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();

    }


    
}   
