using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Jc
{
    public class MainPanel : MonoBehaviour
    {

        // 캠페인 버튼 클릭
        public void OnClickCampaignButton()
        {
            Manager.Sound.PlaySFX(SoundManager.SFXType.Click);
            PhotonNetwork.JoinLobby();
            LobbyManager.Inst.ActiveLoading();
        }
        
        // 옵션 버튼 클릭
        public void OnClickOptionButton()
        {
            Manager.Sound.PlaySFX(SoundManager.SFXType.Click);

        }

        public void OnClickExitButton()
        {
            Manager.Sound.PlaySFX(SoundManager.SFXType.Click);
            // 네트워크 연결해제 (로그아웃)
            PhotonNetwork.Disconnect();
        }
    }
}
