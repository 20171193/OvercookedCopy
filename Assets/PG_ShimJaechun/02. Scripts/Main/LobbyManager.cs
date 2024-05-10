using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

namespace Jc
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private static LobbyManager inst;
        public static LobbyManager Inst { get { return inst; } }    

        public enum Panel { Login, Main, Campagin, Lobby, Room }

        [SerializeField]
        private LoginPanel loginPanel;

        [SerializeField]
        private MainPanel mainPanel;

        [SerializeField]
        private CampaginPanel campaignPanel;

        [SerializeField]
        private LobbyPanel lobbyPanel;

        [SerializeField]
        private RoomPanel roomPanel;

        [SerializeField]
        private ClientState curState = ClientState.JoiningLobby;

        private void Awake()
        {
            inst = this;
        }

        private void Start()
        {
            // 클라이언트 현 상태에 따른 함수 호출
            if (PhotonNetwork.IsConnected)
                OnConnectedToMaster();
            else if (PhotonNetwork.InRoom)
                OnJoinedRoom();
            else if (PhotonNetwork.InLobby)
                OnJoinedLobby();
            else
                OnDisconnected(DisconnectCause.None);

        }

        private void Update()
        {
            CheckCurrentState();
        }

        // 로그인 성공
        public override void OnConnectedToMaster()
        {
            SetActivePanel(Panel.Main);
        }

        // 
        //public override void OnConnected()
        //{
        //    //SetActivePanel(Panel.Menu);
        //}


        // 연결이 실패한 경우
        public override void OnDisconnected(DisconnectCause cause)
        {
            switch (cause)
            {
                case DisconnectCause.ApplicationQuit:
                    return;
                case DisconnectCause.DisconnectByClientLogic:
                    break;
                default:
                    Debug.Log(cause);
                    break;
            }

            SetActivePanel(Panel.Login);
        }

        // 방 생성 실패
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Create room failed with error : {message}({returnCode})");
        }

        // 방 생성
        public override void OnCreatedRoom()
        {
            Debug.Log("Create room success");
        }

        // 방 참가
        public override void OnJoinedRoom()
        {
            SetActivePanel(Panel.Room);
        }

        // 랜덤참가 실패 
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"Join random room failed with error : {message}({returnCode})");
        }

        // 방 나가기
        public override void OnLeftRoom()
        {
            // 로비로 이동
            SetActivePanel(Panel.Lobby);
        }

        // 로비로 이동
        public override void OnJoinedLobby()
        {
            SetActivePanel(Panel.Lobby);
        }

        // 로비에서 나가기 
        public override void OnLeftLobby()
        {
            SetActivePanel(Panel.Main);
        }

        // 방 리스트가 업데이트된 경우 (로비)
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            lobbyPanel.UpdateRoomList(roomList);
        }

        // 플레이어가 방에 입장한 경우 (방)
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            roomPanel.PlayerEnterRoom(newPlayer);
        }

        // 플레이어가 방에서 퇴장한 경우 (방)
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            roomPanel.PlayerLeftRoom(otherPlayer);
        }

        // 방장이 변경된 경우 (방)
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            roomPanel.MasterClientSwitched(newMasterClient);
        }

        // 플레이어의 프로퍼티가 갱신된 경우
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashTable changedProps)
        {
            roomPanel.PlayerPropertiesUpdate(targetPlayer, changedProps);
        }

        public void SetActivePanel(Panel panel)
        {
            loginPanel.gameObject.SetActive(panel == Panel.Login);
            mainPanel.gameObject.SetActive(panel == Panel.Main);
            campaignPanel.gameObject.SetActive(panel == Panel.Campagin);
            roomPanel.gameObject.SetActive(panel == Panel.Room);
            lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
        }

        private void CheckCurrentState()
        {
            if (curState == PhotonNetwork.NetworkClientState) return;

            curState = PhotonNetwork.NetworkClientState;
            Debug.Log(curState);
        }
    }
}