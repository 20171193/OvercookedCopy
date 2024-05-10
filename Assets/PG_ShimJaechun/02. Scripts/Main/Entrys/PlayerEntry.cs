using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

namespace Jc
{
    public class PlayerEntry : MonoBehaviour
    {
        [SerializeField]
        public RoomPanel roomPanel;

        [SerializeField]
        private Image chefImage;

        [SerializeField]
        private TMP_Text playerName;

        [SerializeField]
        private TMP_Text playerReady;

        private Player player;

        // 활성화된 엔트리의 소유자가 방장일 경우 Ready
        private void OnEnable()
        {
            if (PhotonNetwork.IsMasterClient)
                Ready();
            else
                roomPanel.ReadyButton.onClick.AddListener(Ready);
        }

        private void OnDisable()
        {
            if (!PhotonNetwork.IsMasterClient)
                roomPanel.ReadyButton.onClick.RemoveListener(Ready);

        }


        // 플레이어(유저) 세팅
        public void SetPlayer(Player player)
        {
            this.player = player;
            // 이름 할당
            playerName.text = player.NickName;
            // 레디 체크
            playerReady.text = player.GetReady() ? "Ready" : "";   
        }

        // 플레이어 커스텀 프로퍼티 갱신
        public void ChangeCustomProperty(PhotonHashtable property)
        {
            if (property.TryGetValue(CustomProperty.READY, out object value))
            {
                // 레디 갱신
                bool ready = (bool)value;
                playerReady.text = ready ? "Ready" : "";
            }
            else
            {
                playerReady.text = "";
            }
        }

        public void Ready()
        {
            player.SetReady(!player.GetReady());
        }
    }
}
