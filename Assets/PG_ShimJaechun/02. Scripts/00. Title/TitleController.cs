using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Jc
{
    public class TitleController : MonoBehaviour
    {
        private bool isLoadScene;

        [SerializeField]
        private float loadTime;             // 씬 전환 로딩시간

        [SerializeField]
        private Image loadingFadeImage;     // 로딩 페이드인 이미지
        [SerializeField]
        private GameObject loadingGroup;    // 로딩 그룹

        private void OnEnable()
        {
            isLoadScene = false;
        }

        public void ActiveLoginPanel()
        {

        }

        private void Update()
        {
            if (!isLoadScene && Input.GetKey(KeyCode.Space))
            {
                isLoadScene = true;
                Manager.Scene.LoadScene(SceneManager.SceneType.Main);
            }
        }

        IEnumerator LoadSceneRoutine()
        {
            float time = loadTime;
            yield return null;

            while(time > 0)
            {
                time -= Time.deltaTime;

                yield return null;
            }
        }
    }
}
