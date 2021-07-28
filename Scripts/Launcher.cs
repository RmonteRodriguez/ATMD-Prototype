using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class ProfileData
{
    public string username;
    public int level;
    public int xp;
}

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField usernameField;
    public static ProfileData myProfile = new ProfileData();

    public GameObject mainMenu;
    public GameObject roomMenu;

    public GameObject buttonRoom;

    private List<RoomInfo> roomList;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        myProfile = Data.LoadProfile();
        usernameField.text = myProfile.username;

        Connect();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");

        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom()
    {
        StartGame();

        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();

        base.OnJoinRandomFailed(returnCode, message);
    }

    public void Connect()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.GameVersion = "0.0.2";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;

        PhotonNetwork.CreateRoom("", options);
    }

    public void CloseAllWindows()
    {
        mainMenu.SetActive(false);
        roomMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        CloseAllWindows();
        mainMenu.SetActive(true);
    }

    public void OpenRoomList()
    {
        CloseAllWindows();
        roomMenu.SetActive(true);
    }

    private void ClearRoomList()
    {
        Transform content = roomMenu.transform.Find("Scroll View/Viewport/Content");
        foreach (Transform a in content) Destroy(a.gameObject);
    }

    public override void OnRoomListUpdate(List<RoomInfo> p_list)
    {
        roomList = p_list;
        ClearRoomList();

        Debug.Log("Loaded Rooms @ " + Time.time);
        Transform content = roomMenu.transform.Find("Scroll View/Viewport/Content");

        foreach(RoomInfo a in roomList)
        {
            GameObject newRoomButton = Instantiate(buttonRoom, content) as GameObject;

            newRoomButton.transform.Find("Name").GetComponent<Text>().text = a.Name;
            newRoomButton.transform.Find("Players").GetComponent<Text>().text = a.PlayerCount + " / " + a.MaxPlayers;

            newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); });
        }

        base.OnRoomListUpdate(roomList);
    }

    public void JoinRoom(Transform p_button)
    {
        Debug.Log("Joing Room @ " + Time.time);
        string t_roomName = p_button.transform.Find("Name").GetComponent<Text>().text;
        PhotonNetwork.JoinRoom(t_roomName);
    }

    public void RefreshRoomList()
    {
        PhotonNetwork.Disconnect();
        Connect();
    }

    public void StartGame()
    {
        if(string.IsNullOrEmpty(usernameField.text))
        {
            myProfile.username = "Random_User_" + Random.Range(100, 1000);
        }
        else
        {
            myProfile.username = usernameField.text;
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Data.SaveProfile(myProfile);
            PhotonNetwork.LoadLevel(1);
        }
    }
}
