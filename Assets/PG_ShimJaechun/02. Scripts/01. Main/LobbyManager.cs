using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

namespace Jc
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private static LobbyManager inst;
        public static LobbyManager Inst { get { return inst; } }

        public enum Panel { Title, Main, Campagin, Room }

        [SerializeField]
        private TitleController titleController;    // 타이틀 패널

        [SerializeField]
        private MainPanel mainPanel;                // 메인 패널

        [SerializeField]
        private CampaginPanel campaignPanel;        // 캠페인 패널

        [SerializeField]
        private LobbyPanel lobbyPanel;              // 로비 패널

        [SerializeField]
        private RoomPanel roomPanel;                // 방 패널

        [SerializeField]
        private ClientState curState = ClientState.JoiningLobby;

        // 방을 나갔을 경우 바로 로비로 이동하기위해 사용
        bool isExitRoom = false;

        public LoadingPanel loadingPanel;        // 로딩 패널

        [Header("타이틀 씬 이벤트 카메라")]
        [SerializeField]
        private CinemachineVirtualCamera mainVC;    // 타이틀 씬 카메라
        [SerializeField]
        private Vector3 startVCpos;            // 카메라 시작 위치
        [SerializeField]
        private Vector3 startVCrot;            // 카메라 시작 회전값
        [SerializeField]
        private Vector3 endVCpos;              // 카메라 종료 위치
        [SerializeField]
        private Vector3 endVCrot;               // 카메라 종료 회전값
        [SerializeField]
        private float loadingTime;               // 타이틀 -> 메인 전환시간
        
        [Header("메뉴 타이틀 이미지 그룹")]
        [SerializeField]
        private GameObject titleImage;      // 메뉴 타이틀 이미지
        [Header("메뉴 페널")]
        [SerializeField]
        private GameObject menuPanel;

        private void Awake()
        {
            inst = this;
            titleImage.SetActive(false);
            menuPanel.SetActive(false);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            Manager.Scene.OnDisconnected += DisconnectSetting;
        }
        public override void OnDisable()
        {
            base.OnDisable();
            Manager.Scene.OnDisconnected -= DisconnectSetting;
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

        // 로딩 시작
        public void ActiveLoading()
        {
            loadingPanel.gameObject.SetActive(true);
        }
        // 로딩 종료 (페이드 아웃)
        public void DisActiveLoading(float fadeTime = 0f)
        {
            // 이미 비활성화된 상태라면 return
            if (!loadingPanel.gameObject.activeSelf) return;

            loadingPanel.FadeOut(fadeTime);
        }

        // 로그인 성공
        public override void OnConnectedToMaster()
        {
            Debug.Log("마스터서버 연결 성공");

            // 최초 한 번 실행 (로그인에 성공한 경우)
            if (!isExitRoom)
            { 
                StartCoroutine(FirstJoinLobbyRoutine());
            }
            else
            {
                isExitRoom = false;
                PhotonNetwork.JoinLobby();
            }
        }
        // 최초 로비 입장루틴
        IEnumerator FirstJoinLobbyRoutine()
        {
            float rate = 0f;
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.FadeOut(2f);
            titleController.gameObject.SetActive(false);
            yield return null;

            while(rate < 1f)
            {
                rate += Time.deltaTime / loadingTime;
                // 메인 카메라 위칫값 적용
                mainVC.transform.position = Vector3.Lerp(startVCpos, endVCpos, rate);
                mainVC.transform.rotation = Quaternion.Euler(Vector3.Lerp(startVCrot, endVCrot, rate));
                yield return null;
            }

            mainVC.transform.position = endVCpos;
            mainVC.transform.rotation = Quaternion.Euler(endVCrot);
            titleImage.SetActive(true);
            Manager.Sound.PlaySFX(SoundManager.SFXType.PopUp);
            menuPanel.SetActive(true);
            DisActiveLoading(1.2f);
            yield return null;

            yield return new WaitForSeconds(1.2f);
            SetActivePanel(Panel.Main);
        }

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

            SetActivePanel(Panel.Title);
            DisconnectSetting();
        }
        
        // 서버연결 해제 시 패널 UI 기본세팅
        private void DisconnectSetting()
        {
            titleImage.SetActive(false);
            menuPanel.SetActive(false);
            isExitRoom = false;
            titleController.gameObject.SetActive(true);
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
            DisActiveLoading(0.5f);
            SetActivePanel(Panel.Room);
        }

        // 랜덤참가 실패 
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log($"Join random room failed with error : {message}({returnCode})");
        }

        public override void OnLeftRoom()
        {
            isExitRoom = true;
            Debug.Log("Left Room");
            DisActiveLoading(0.5f);
            SetActivePanel(Panel.Campagin);
        }

        // 로비로 이동 == OnLeftRoom()
        public override void OnJoinedLobby()
        {
            Debug.Log("Join Lobby");
            DisActiveLoading(0.5f);
            SetActivePanel(Panel.Campagin);
        }

        // 로비에서 나가기 
        public override void OnLeftLobby()
        {
            Debug.Log("Left Lobby");
            DisActiveLoading(0.5f);
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

        // 패널 UI관리 메서드
        public void SetActivePanel(Panel panel)
        {
            titleController.gameObject.SetActive(panel == Panel.Title);
            mainPanel.gameObject.SetActive(panel == Panel.Main);
            campaignPanel.gameObject.SetActive(panel == Panel.Campagin);
            roomPanel.gameObject.SetActive(panel == Panel.Room);
        }

        // 현재 네트워크 상태 체크
        private void CheckCurrentState()
        {
            if (curState == PhotonNetwork.NetworkClientState) return;

            curState = PhotonNetwork.NetworkClientState;
            Debug.Log(curState);
        }


    }
}