using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //public enum Panel { Login, Menu, Lobby, Room }

    //[SerializeField] LoginPanel loginPanel;
    ////[SerializeField] MainPanel menuPanel;
    //[SerializeField] RoomPanel roomPanel;
    //[SerializeField] LobbyPanel lobbyPanel;

    //[SerializeField]
    //private ClientState curState = ClientState.JoiningLobby;

    //private void Start()
    //{
    //    SetActivePanel(Panel.Login);
    //}

    //private void Update()
    //{
    //    CheckCurrentState();
    //}

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    Debug.Log($"Create room failed with error : {message}({returnCode})");
    //}
    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("Create room success");
    //}
    //public override void OnJoinedRoom()
    //{
    //    SetActivePanel(Panel.Room);
    //}
    //public override void OnJoinRandomFailed(short returnCode, string message)
    //{
    //    Debug.Log($"Join random room failed with error : {message}({returnCode})");
    //}
    //public override void OnLeftRoom()
    //{
    //    SetActivePanel(Panel.Menu);
    //}
    //public override void OnConnected()
    //{
    //    SetActivePanel(Panel.Menu);
    //}

    //public override void OnJoinedLobby()
    //{
    //    SetActivePanel(Panel.Lobby);
    //}
    //public override void OnLeftLobby()
    //{
    //    SetActivePanel(Panel.Menu);
    //}
    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    lobbyPanel.UpdateRoomList(roomList);
    //}
    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    roomPanel.PlayerEnterRoom(newPlayer);
    //}
    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    roomPanel.PlayerLeftRoom(otherPlayer);
    //}
    //public override void OnMasterClientSwitched(Player newMasterClient)
    //{
    //    roomPanel.MasterClientSwitched(newMasterClient);
    //}
    //// 플레이어의 프로퍼티가 갱신된 경우
    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashTable changedProps)
    //{
    //    roomPanel.PlayerPropertyUpdate(targetPlayer, changedProps);
    //    roomPanel.AllPlayerReadyCheck();
    //}

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    switch (cause)
    //    {
    //        case DisconnectCause.ApplicationQuit:
    //            return;
    //        case DisconnectCause.DisconnectByClientLogic:
    //            break;
    //        default:
    //            Debug.LogError(cause);
    //            break;
    //    }

    //    SetActivePanel(Panel.Login);
    //}

    //private void SetActivePanel(Panel panel)
    //{
    //    loginPanel.gameObject.SetActive(panel == Panel.Login);
    //    menuPanel.gameObject.SetActive(panel == Panel.Menu);
    //    roomPanel.gameObject.SetActive(panel == Panel.Room);
    //    lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
    //}

    //private void CheckCurrentState()
    //{
    //    if (curState == PhotonNetwork.NetworkClientState) return;

    //    curState = PhotonNetwork.NetworkClientState;
    //    Debug.Log(curState);
    //}
}