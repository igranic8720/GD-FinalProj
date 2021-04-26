// FILE:    Launcher.cs
// DATE:    4/25/2021
// DESC:    This file facilitates the photon network callbacks.
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;


    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;

    [SerializeField] GameObject startGameButton;
    // FUNCTION:    Awake
    // DESC:        setting the instance of the object upon awake();
    // PARAMETERS:  void
    void Awake()
    {
        Instance = this;
    }
    // FUNCTION:    Start
    // DESC:        connects to the network using the settings found in the project.
    // PARAMETERS:  void
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // FUNCTION:    Update
    // DESC:        nothing.
    // PARAMETERS:  void
    void Update()
    {
        
    }
    // FUNCTION:    OnConnectedToMaster
    // DESC:        join lobby, syncs scene
    // PARAMETERS:  void
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // FUNCTION:    OnJoinedLobby
    // DESC:        after joining a lobby, the title menu opens. gets a random nickname.
    // PARAMETERS:  void
    public override void OnJoinedLobby()
    {
        MenuMan.Instance.OpenMenu("title");

        Debug.Log("joined lobby");

        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
    }
    // FUNCTION:    CreateRoom
    // DESC:        creates a room on the photon network. takes the user input for the name of the room.
    // PARAMETERS:  void
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        RoomOptions roomOpt = new RoomOptions();
        roomOpt.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomNameInputField.text , roomOpt, null);
        MenuMan.Instance.OpenMenu("loading");
    }
    // FUNCTION:    CancelCreateRoom
    // DESC:        stopping the creation of a room. resets the inputfield.
    // PARAMETERS:  void
    public void CancelCreateRoom()
    {
        roomNameInputField.text = "";
    }
    // FUNCTION:    OnJoinedRoom
    // DESC:        after joining a room, the player list is updated.  starting the game button is available to the master client of the room.
    // PARAMETERS:  void
    public override void OnJoinedRoom()
    {
        MenuMan.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().setup(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    // FUNCTION:    OnMasterClientSwitched
    // DESC:        sets the startgame button active for the other player if the master client leaves the room
    // PARAMETERS:  void
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    // FUNCTION:    OnCreateRoomFailed
    // DESC:        opens the menu for errors. only called if photonnetwork fails to create a room.
    // PARAMETERS:  void
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed";
        MenuMan.Instance.OpenMenu("error");
    }
    // FUNCTION:    LeaveRoom
    // DESC:        leaves a room.
    // PARAMETERS:  void
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuMan.Instance.OpenMenu("loading");
    }
    // FUNCTION:    OnLeftRoom
    // DESC:        after leaving a room, it opens back up the title menu
    // PARAMETERS:  void
    public override void OnLeftRoom()
    {
        MenuMan.Instance.OpenMenu("title");
    }
    // FUNCTION:    OnRoomListUpdate
    // DESC:        when room list of players changes, the room menu updates the list of players.
    // PARAMETERS:  List<RoomInfo> roomList
    public override void OnRoomListUpdate(List<RoomInfo> roomList) 
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().setup(roomList[i]);
        }
    }
    // FUNCTION:    JoinRoom
    // DESC:        joins a room using the parameter
    // PARAMETERS:  RoomInfo info
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuMan.Instance.OpenMenu("loading");
    }
    // FUNCTION:    OnJoinRoomFailed
    // DESC:        if the client fails to join a room, the error menu opens.
    // PARAMETERS:  short returnCode, string message
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        MenuMan.Instance.OpenMenu("error");
        errorText.text = message;
    }
    // FUNCTION:    OnPlayerEnteredRoom
    // DESC:        if the player enters a room, the item list must be updated.
    // PARAMETERS:  Player newPlayer
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().setup(newPlayer);
    }
    // FUNCTION:    StartGame
    // DESC:        starts the match
    // PARAMETERS:  void
    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
    // FUNCTION:    exitClient
    // DESC:        exits the client
    // PARAMETERS:  void
    public void exitClient()
    {
        Application.Quit();
    }
}
