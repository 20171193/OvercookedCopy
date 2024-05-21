using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DebugGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string debugRoomName;

    void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"TestPlayer {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();
    }
    

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;
        TypedLobby typedLobby = new TypedLobby("DebugLobby", LobbyType.Default);

        PhotonNetwork.JoinOrCreateRoom(debugRoomName, options, typedLobby);
    }

    public override void OnJoinedRoom()
    {
        GameStart();
    }

    public void GameStart()
    {
        Debug.Log("Debug GameStart");
    }
}