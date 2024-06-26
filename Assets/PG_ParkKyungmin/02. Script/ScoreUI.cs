
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace Kyungmin
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] Slider gauge;        // 게이지바
        [SerializeField] TMP_Text totalscoreText;
        [SerializeField] TMP_Text tipText;
        [SerializeField] Animator coinAnimator;     // Score가 올랐을때 Coin이 돌아가는 애니메이션
        [SerializeField] Animator fireAnimator;     // 팁X4가 되었을때 불 애니메이션
        [SerializeField] Animator textAnimator;     // Score가 올랐을때 text 애니메이션



        private void OnEnable()
        {
            // score와 tip의 초기값
            UpdateUI(0, 0);
        }

        public void UpdateUI(int score, int tip)
        {

            totalscoreText.text = score.ToString();

            // 게이지의 value가 tip의 수치에 맞게 오를 수 있게함
            gauge.value = tip;

            if (tip == 0)
            {
                // tip이 0일때는 text가 보이지 않게
                tipText.text = "";
            }
            else
            {
                // tip이 0이 아닐때는 tip의 수치를 나타내줌
                tipText.text = $"팁 X {tip}";
            }
        }

        public void GetCoin()
        {
            // 코인을 먹었을때 애니메이션 재생
            coinAnimator.SetTrigger("OnGetCoin");
            textAnimator.SetTrigger("OnGetScore");
        }

        public void EnableFire()
        {
            // fireUI 키고, 애니메이션 재생      
            fireAnimator.SetBool("IsActiveFire", true);
        }

        public void DisableFire()
        {
            // fireUI, 애니메이션 끄기 
            fireAnimator.SetBool("IsActiveFire", false);
        }

    }
}