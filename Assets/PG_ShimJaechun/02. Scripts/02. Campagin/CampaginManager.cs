using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

namespace Jc
{
    public class CampaginManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private ClientState curState = ClientState.JoiningLobby;
        [SerializeField]
        private GameObject masterBus;

        [SerializeField]
        private GameObject pauseGroup;

        [SerializeField]
        private int spawnIndex = 0;

        [SerializeField]
        private Transform[] spawnPoses;

        public UnityAction OnCampaiginSetted;

        private void Start()
        {
            Manager.Sound.PlayBGM(SoundManager.BGMType.Campagin);
            Manager.Scene.FadeOut();

            // 일반 모드 
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("일반 모드입니다.");
                PhotonNetwork.LocalPlayer.SetLoad(true);
                LoadMasterData();
            }
            // 디버깅 모드
            else
            {
                Debug.Log("디버그 모드입니다.");
                PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        private void OnPause(InputValue value)
        {
            pauseGroup.SetActive(!pauseGroup.activeSelf);
        }

        public void OnClickContinueButton()
        {
            pauseGroup.SetActive(false);
        }
        public void OnClickQuitButton()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            Manager.Scene.LoadLevel(SceneManager.SceneType.Main);
        }

        private void LoadMasterData()
        {
            bool[] getStageInfo = Manager.PlableData.LoadUserStageScore();
            for(int i =0; i<getStageInfo.Length; i++)
            {
                if(getStageInfo[i] == false)
                {
                    spawnIndex = i;
                    masterBus.transform.position = spawnPoses[i].transform.position;
                    return;
                }
            }
        }

        // 디버그모드 : 바로 마스터서버로 입장
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


        // 접속이 끊긴 경우 타이틀 씬으로 이동
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Disconnected : {cause}");
            Manager.Scene.LoadScene(SceneManager.SceneType.Title);
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left Room");
            Manager.Scene.LoadLevel(SceneManager.SceneType.Main);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            masterBus.GetComponent<PlayerInput>().enabled = true;
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
        {
            if (changedProps.ContainsKey(CustomProperty.LOAD))
            {
                if (PlayerLoadCount() == PhotonNetwork.PlayerList.Length)
                {
                    Debug.Log($"PlayerLoadCount = {PlayerLoadCount()}, Photon = {PhotonNetwork.PlayerList.Length}");
                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.CurrentRoom.SetGameStart(true);
                }
            }
        }

        public override void OnRoomPropertiesUpdate(PhotonHashtable changedProps)
        {
            if (changedProps.ContainsKey(CustomProperty.GAMESTART))
            {
                GameStart();
            }
        }
        private void GameStart()
        {
            if (PhotonNetwork.IsMasterClient)
                StartCoroutine(Extension.ActionDelay(5.0f, () => masterBus.GetComponent<PlayerInput>().enabled = true));
            else
                masterBus.GetComponent<PlayerInput>().enabled = false;

            OnCampaiginSetted?.Invoke();
        }

        private int PlayerLoadCount()
        {
            int loadCount = 0;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.GetLoad())
                    loadCount++;
            }
            return loadCount;
        }

    }
}
