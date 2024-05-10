using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jc
{
    public class CampaginPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject createRoomPanel;     // 룸 생성 패널

        [SerializeField]
        private TMP_InputField roomNameInput;  // 룸 이름 입력패널

        [SerializeField]
        private List<string> defaultRoomNames;  // 기본 룸 이름

        private void Awake()
        {
        }

        private void OnEnable()
        {
            createRoomPanel.SetActive(false);
        }

        // 룸 생성메뉴 오픈
        public void CreateRoomMenu()
        {
            createRoomPanel.SetActive(true);
        }

        // 룸 생성
        public void CreateRoomConfirm()
        {
            string _roomName = roomNameInput.text.Length < 1 ?
                defaultRoomNames[Random.Range(0, defaultRoomNames.Count)] :
                roomNameInput.text;

            // 최대 인원 수는 4명
            RoomOptions options = new RoomOptions() { MaxPlayers = 4 };

            PhotonNetwork.CreateRoom(roomName: _roomName, roomOptions: options);
        }

        // 룸 생성 취소
        public void CreateRoomCancel()
        {
            roomNameInput.text = "";
            createRoomPanel.SetActive(false);
        }

        // 랜덤매칭
        public void RandomMatching()
        {
            string _roomName = defaultRoomNames[Random.Range(0, defaultRoomNames.Count)];
            RoomOptions options = new RoomOptions() { MaxPlayers = 4 };

            PhotonNetwork.JoinRandomOrCreateRoom(roomName: _roomName, roomOptions: options);
        }

        // 로비로 이동
        public void JoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }
    }
}
