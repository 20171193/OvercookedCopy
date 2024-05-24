using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Jc
{
    public class LoginPanel : MonoBehaviour
    {
        [SerializeField] TMP_InputField idInputField;

        // 플레이어 기본 아이디 지정 (아이디를 지정하지 않은 경우)
        // 파이어 베이스 연동 후 수정
        private void Start()
        {
            idInputField.text = $"Player {Random.Range(1000, 10000)}";
        }

        // 로그인 버튼 콜백
        // 파이어베이스 연동 후 수정 : 로그인 체크
        public void Login()
        {
            // 로그인 실패 처리
            if (idInputField.text == "")
            {
                Debug.LogError("Empty nickname : Please input name");
                return;
            }

            // 로그인 성공 처리
            PhotonNetwork.LocalPlayer.NickName = idInputField.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
