using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

namespace Jc
{
    public class RoomPanel : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform playerContent;    // 엔트리 할당 트랜스폼
        [SerializeField] 
        private PlayerEntry playerEntryPrefab;  // 플레이어 엔트리 프리팹
        [SerializeField] 
        private Button startButton;             // 게임시작 버튼 

        [SerializeField]
        private Button readyButton;             // 게임레디 버튼
        public Button ReadyButton {get { return readyButton; }}

        // 플레이어 엔트리 딕셔너리
        private Dictionary<int, PlayerEntry> playerDictionary;

        private void Awake()
        {
            playerDictionary = new Dictionary<int, PlayerEntry>();
        }

        private void OnEnable()
        {
            PhotonNetwork.LocalPlayer.SetReady(false);
            PhotonNetwork.LocalPlayer.SetLoad(false);

            // 플레이어 리스트 갱신
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
                entry.roomPanel = this;
                playerDictionary.Add(player.ActorNumber, entry);
                if (player.IsMasterClient)
                    player.SetReady(true);
                entry.SetPlayer(player);
            }

            ButtonSetting();
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void OnDisable()
        {
            // 딕셔너리에 할당된 엔트리 제거
            foreach (int actorNumber in playerDictionary.Keys)
            {
                Destroy(playerDictionary[actorNumber].gameObject);
            }

            playerDictionary.Clear();
            
            // 씬 자동 변경해제 (마스터 팔로잉 해제)
            PhotonNetwork.AutomaticallySyncScene = false;
        }
        private void ButtonSetting()
        {
            // 버튼 세팅
            // 방장
            //  - 기본상태 : 준비 o
            //  - 시작버튼 : 활성화
            // 플레이어
            //  - 기본상태 : 준비 x
            //  - 시작버튼 : 비활성화
            bool isMaster = PhotonNetwork.IsMasterClient;
            startButton.gameObject.SetActive(isMaster);
            readyButton.gameObject.SetActive(!isMaster);
        }

        // 플레이어 방 입장
        public void PlayerEnterRoom(Player newPlayer)
        {
            Debug.Log($"Enter Player : {newPlayer.ActorNumber}");
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
            entry.roomPanel = this;
            entry.SetPlayer(newPlayer);
            // 룸 패널 할당
            // 액터넘버로 딕셔너리 키 할당
            playerDictionary.Add(newPlayer.ActorNumber, entry);
            AllPlayerReadyCheck();
        }

        // 플레이어 퇴장
        public void PlayerLeftRoom(Player otherPlayer)
        {
            // 해당 플레이어 할당 해제 및 제거
            Destroy(playerDictionary[otherPlayer.ActorNumber].gameObject);
            playerDictionary.Remove(otherPlayer.ActorNumber);
            AllPlayerReadyCheck();
        }

        // 방 내부에서 플레이어의 프로퍼티가 갱신된 경우 호출
        public void PlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
        {
            playerDictionary[targetPlayer.ActorNumber].ChangeCustomProperty(changedProps);

            AllPlayerReadyCheck();
        }

        // 방장이 변경된 경우 호출
        public void MasterClientSwitched(Player newMasterClient)
        {
            ButtonSetting();
            playerDictionary[newMasterClient.ActorNumber].OnMasterSetting();

            AllPlayerReadyCheck();
        }

        public void StartGame()
        {
            // 현재 방 닫기
            PhotonNetwork.CurrentRoom.IsOpen = false;
            // 로비에서 방 비활성화
            PhotonNetwork.CurrentRoom.IsVisible = false;

            // 게임씬 로드
            PhotonNetwork.LoadLevel("GameScene");
        }

        // 방 나가기 
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        // 방 내부에 존재하는 모든 플레이어를 대상으로 준비상태체크
        public void AllPlayerReadyCheck()
        {
            // 방장을 제외한 모든 플레이어 스타트버튼 비활성화
            if (!PhotonNetwork.IsMasterClient)
            {
                startButton.gameObject.SetActive(false);
                return;
            }

            int readyCount = 0;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.GetReady())
                    readyCount++;
            }

            // 준비한 플레이어 수
            // 게임시작 버튼 활성화/비활성화
            startButton.interactable = readyCount == PhotonNetwork.PlayerList.Length;
            startButton.GetComponent<Image>().color =
                readyCount == PhotonNetwork.PlayerList.Length ? startButton.colors.normalColor : startButton.colors.disabledColor;
        }
    }
}
