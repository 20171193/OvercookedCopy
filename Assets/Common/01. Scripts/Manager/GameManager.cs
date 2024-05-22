using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Jc;

namespace Jc
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public Transform[] spawnposs;

        [SerializeField]
        private ClientState curState = ClientState.JoiningLobby;

        // 인게임 게임매니저
        private void Start()
        {
            // 일반 모드
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("일반 모드입니다.");
                PhotonNetwork.LocalPlayer.SetLoad(true);
            }
            // 디버깅 모드
            else
            {
                Debug.Log("디버그 모드입니다.");
                PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void Update()
        {
            ClientState cState = PhotonNetwork.NetworkClientState;
            if (curState != cState)
            {
                Debug.Log(cState);
                curState = cState;
            }
        }
        public override void OnConnectedToMaster()
        {
            RoomOptions options = new RoomOptions() { IsVisible = false };
            PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
        }

        // 디버깅모드 게임시작
        public override void OnJoinedRoom()
        {
            GameStart();
        }

        // 연결 실패 시 타이틀 씬으로 이동
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected : {cause}");
            Manager.Scene.LoadScene(SceneManager.SceneType.Title);
        }

        // 게임을 퇴장한 경우
        public override void OnLeftRoom()
        {
            Manager.Scene.LoadLevel(SceneManager.SceneType.Campagin);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            Manager.Scene.LoadScene(SceneManager.SceneType.Main);
            PhotonNetwork.LeaveRoom();
        }

        private void GameStart()
        { 
            PhotonNetwork.Instantiate("Chef_Robot", spawnposs[0].position, spawnposs[0].rotation, 0);
        }
    }
}