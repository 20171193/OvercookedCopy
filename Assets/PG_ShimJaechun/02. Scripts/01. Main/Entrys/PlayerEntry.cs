using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using JH;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Jc
{
    public class PlayerEntry : MonoBehaviour
    {
        [SerializeField]
        public RoomPanel roomPanel;

        [SerializeField]
        private GameObject selectChefGroup;

        [SerializeField]
        private Image chefImage;
        public Image ChefImage { get { return chefImage; } }
        
        [SerializeField]
        private TMP_Text playerName;

        [SerializeField]
        private TMP_Text playerReady;

        private Player player;

        private void OnDisable()
        {
            if (player.IsLocal)
                roomPanel?.ReadyButton.onClick.AddListener(Ready);
        }

        // 플레이어(유저) 세팅
        public void SetPlayer(Player player)
        {
            this.player = player;
            // 이름 할당
            playerName.text = player.NickName;

            if (player.IsLocal)
                roomPanel.ReadyButton.onClick.AddListener(Ready);

            if (player.IsMasterClient)
            {
                OnMasterSetting();
            }
            else
            {
                // 레디 체크
                playerReady.text = player.GetReady() ? "Ready" : "";
            }
        }

        public void OnMasterSetting()
        {
            player.SetReady(true);
            playerReady.text = "Master";
            playerReady.color = Color.red;
        }

        // 플레이어 커스텀 프로퍼티 갱신
        public void ChangeCustomProperty(PhotonHashtable property)
        {
            if(player.IsMasterClient)
                return;
            

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

            if(property.TryGetValue(CustomProperty.CHEF, out object index))
            {
                // 요리사 인덱스 갱신
            }
            else
            {

            }
        }

        // 준비버튼 함수
        public void Ready()
        {
            player.SetReady(!player.GetReady());
        }

        public void OnClickChangeButton()
        {
            selectChefGroup.SetActive(true);
        }

        public void ChangeChef(int index)
        {
            this.chefImage.sprite = Manager.PlableData.chefInfos[index].sprite;
            player.SetChef(index);
            selectChefGroup.SetActive(false);
        }
    }
}
