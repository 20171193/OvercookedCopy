using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Jc
{
    // 메인 타이틀
    public class TitleController : MonoBehaviour
    {
        [SerializeField]
        private GameObject titleText;   // 타이틀 텍스트
        [SerializeField]
        private GameObject loginPanel;  // 로그인 패널
        [SerializeField]
        private bool isActiveLogin;     // 로그인 패널 활성화 여부

        private void OnEnable()
        {
            Manager.Sound.PlayBGM(SoundManager.BGMType.Title);

            // 로그인창 초기세팅
            // 타이틀 텍스트 활성화
            titleText.SetActive(true);
            // 로그인 패널 비활성화
            loginPanel.SetActive(false);

        }

        private void OnDisable()
        {
            isActiveLogin = false;
        }

        private void Update()
        {
            if(!isActiveLogin && Input.GetKey(KeyCode.Space))
            {
                isActiveLogin = true;
                // 타이틀 텍스트 비활성화
                titleText.SetActive(false);
                // 로그인 패널 활성화
                loginPanel.SetActive(true);

                Manager.Sound.PlaySFX(SoundManager.SFXType.PopUp);
            }
        }
    }
}
