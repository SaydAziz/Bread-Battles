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
    GameObject controller;

    Button BlueButton, RedButton;

    TMP_Text[] BlueText = new TMP_Text[5];
    TMP_Text[] RedText = new TMP_Text[5];

    


    public int Team = 0;
    private int RedSlots = 5;
    private int BlueSlots = 5;
    
    void Awake()
    {
        PV = GetComponent<PhotonView>();

        RedButton = GameObject.Find("SwitchR").GetComponent<Button>();
        BlueButton = GameObject.Find("SwitchB").GetComponent<Button>();  

        BlueText[0] = GameObject.Find("B_Player 1").GetComponent<TMP_Text>();
        BlueText[1] = GameObject.Find("B_Player 2").GetComponent<TMP_Text>();  
        BlueText[2] = GameObject.Find("B_Player 3").GetComponent<TMP_Text>();  
        BlueText[3] = GameObject.Find("B_Player 4").GetComponent<TMP_Text>();  
        BlueText[4] = GameObject.Find("B_Player 5").GetComponent<TMP_Text>();  

        RedText[0] = GameObject.Find("Player 1").GetComponent<TMP_Text>();
        RedText[1] = GameObject.Find("Player 2").GetComponent<TMP_Text>();  
        RedText[2] = GameObject.Find("Player 3").GetComponent<TMP_Text>();  
        RedText[3] = GameObject.Find("Player 4").GetComponent<TMP_Text>();  
        RedText[4] = GameObject.Find("Player 5").GetComponent<TMP_Text>();  

    }
    
    void Start()
    {
        RedButton.onClick.AddListener(SelectTeamRed);
        BlueButton.onClick.AddListener(SelectTeamBlue);


        if(PV.IsMine)
        {
            //CreateController();
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


    public void SelectTeamBlue()
    {
        if (BlueSlots != 0 && Team == 0)
        {
            Team = 1;
            PV.RPC("RPC_AssignBlueSlot", RpcTarget.All, BlueSlots);
            Debug.Log("BlueSlots: " + BlueSlots);
            Debug.Log("Team: " + Team);

        }
        else if (BlueSlots != 0 && Team == 2)
        {
            PV.RPC("RPC_RemovedRedSlot", RpcTarget.All, RedSlots);
            Team = 1;
            PV.RPC("RPC_AssignBlueSlot", RpcTarget.All, BlueSlots);
            Debug.Log("BlueSlots: " + BlueSlots);
            Debug.Log("Team: " + Team);
        }

        
    }

    public void SelectTeamRed()
    {
        if (RedSlots != 0 && Team == 0)
        {
            Team = 2;
            PV.RPC("RPC_AssignRedSlot", RpcTarget.All, RedSlots);
            Debug.Log("RedSlots: " + RedSlots);
            Debug.Log("Team: " + Team);
        }
        else if (RedSlots != 0 && Team == 1)
        {
            PV.RPC("RPC_RemovedBlueSlot", RpcTarget.All, BlueSlots);
            Team = 2;
            PV.RPC("RPC_AssignRedSlot", RpcTarget.All, RedSlots);
            Debug.Log("RedSlots: " + RedSlots);
            Debug.Log("Team: " + Team);
        }
        
        
    }

    [PunRPC]
    void RPC_AssignRedSlot(int Slots)
    {
        RedText[Slots - 1].text = PV.Owner.NickName;
        RedSlots -= 1;
    }

    [PunRPC]
    void RPC_AssignBlueSlot(int Slots)
    {
        BlueText[Slots - 1].text = PV.Owner.NickName;
        BlueSlots -= 1;
    }

    [PunRPC]
    void RPC_RemovedRedSlot(int Slots)
    {
        RedText[Slots].text = ".";
        RedSlots += 1;
    }

    [PunRPC]
    void RPC_RemovedBlueSlot(int Slots)
    {
        BlueText[Slots].text = ".";
        BlueSlots += 1;
    }

}
