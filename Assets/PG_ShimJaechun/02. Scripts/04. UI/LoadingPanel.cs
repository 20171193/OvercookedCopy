using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jc
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField]
        private Image fadeInPanel;
        public Image FadeInPanel { get { return fadeInPanel; } }

        [SerializeField]
        private Color fadeInColor;         // 페이드인 컬러
        public Color FadeInColor {get { return fadeInColor; } }
        [SerializeField]
        private Color fadeOutColor;        // 페이드아웃 컬러
        public Color FadeOutColor {get { return fadeOutColor; } }
        private void Awake()
        {
            fadeInColor = fadeInPanel.color;
            fadeOutColor = new Color(0, 0, 0, 0);
        }

        private void OnEnable()
        {
            fadeInPanel.color = fadeInColor;
        }

        public void FadeIn(float fadeTime = 0f)
        {
            fadeInPanel.color = fadeOutColor;
            StartCoroutine(FadeInRoutine(fadeTime));
        }

        public void FadeOut(float fadeTime = 0f)
        {
            fadeInPanel.color = fadeInColor;
            StartCoroutine(FadeOutRoutine(fadeTime));
        }

        // 로딩패널 페이드인 
        private IEnumerator FadeInRoutine(float fadeTime)
        {
            float rate = 0f;
            yield return null;

            while (rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeInPanel.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
                yield return null;
            }

            fadeInPanel.color = fadeInColor;
            yield return null;
        }

        // 로딩 패널 페이드아웃
        private IEnumerator FadeOutRoutine(float fadeTime)
        {
            float rate = 0f;
            yield return null;

            while(rate < 1f)
            {
                rate += Time.deltaTime / fadeTime;
                fadeInPanel.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
                yield return null;
            }

            // 코루틴 종료 시 비활성화
            gameObject.SetActive(false);
            fadeInPanel.color = fadeInColor;
            yield return null;
        }
    }
}
