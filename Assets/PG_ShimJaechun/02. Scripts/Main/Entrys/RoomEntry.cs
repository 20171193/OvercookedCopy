using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jc
{
    public class RoomEntry : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI roomName;           // 방 정보
        [SerializeField]
        private TextMeshProUGUI currentPlayer;      // 플레이어 정보
        [SerializeField]
        private Button joinRoomButton;              // 입장 버튼

        private RoomInfo roomInfo;

        private void OnEnable()
        {

        }

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            // RoomEntry의 RoomInfo를 할당
            this.roomInfo = roomInfo;
            // 방 이름
            roomName.text = roomInfo.Name;
            // 방에 참여한 플레이어 수
            currentPlayer.text = $"{roomInfo.PlayerCount} / {roomInfo.MaxPlayers}";

            SetRoomEnterable(roomInfo.PlayerCount < roomInfo.MaxPlayers);
        }

        // 방 입장세팅
        private void SetRoomEnterable(bool isOpen)
        {
            joinRoomButton.interactable = isOpen;
            currentPlayer.color = isOpen ? Color.white : Color.red;
        }

        public void JoinRoom()
        {
            // 룸 입장
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
    }
}