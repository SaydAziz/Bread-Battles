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
 
 
    Button BlueButton, RedButton;
 
    static TMP_Text[] BlueText = new TMP_Text[5];
    static TMP_Text[] RedText = new TMP_Text[5];

    static PlayerInfo[] blueTeam = new PlayerInfo[5];
    static PlayerInfo[] redTeam = new PlayerInfo[5];
    
    private static int redSlots = 5;
    private static int blueSlots = 5;
    private List<PlayerInfo> playersInRoom = new List<PlayerInfo>();

    
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
        //RedButton.onClick.AddListener(SelectTeamRed);
        BlueButton.onClick.AddListener(SelectTeamBlue);

        if(PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
        
        
        if(PV.IsMine)
        {
            PV.RPC("updateUserID", RpcTarget.AllBuffered);
        }

    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }

    public void SelectTeamBlue()
    {
        Debug.Log("Players in room: " + playersInRoom);
        Debug.Log("PlayerID: " + player.userID + " Team: " + player.team);

        foreach(PlayerInfo x in playersInRoom)
        {
            if(x.userID == null)
            {

            }
            else if (x.userID == PhotonNetwork.LocalPlayer.UserId && x.team == 1)
            {
                Debug.Log("Already on team BLUE");
                Debug.Log("PlayerID: " + x.userID + " Team: " + x.team);
                return;
            }
        }
        PV.RPC("RPC_JoinBlue", RpcTarget.All);
    }

    public void SelectTeamRed()
    {
        
    }

    [PunRPC]
    public void RPC_JoinBlue()
    {
        //System
        //blueTeam[blueSlots-1] = playersInRoom[];
        blueTeam[blueSlots-1].team = 1;
        blueSlots--;

        //Visual 
        for(int i = 4; i >= 0; i--)
        {
            bool check = BlueText[i].text.Equals(".");
            if(check)
            {
                BlueText[i].text = PV.Owner.NickName;
                return;
            }
        }
    }

    [PunRPC]
    public void updateUserID()
    {
        player = controller.GetComponent<PlayerInfo>();

        player.userID = PhotonNetwork.LocalPlayer.UserId;
        
        playersInRoom.Add(player);
    }
    
}   
