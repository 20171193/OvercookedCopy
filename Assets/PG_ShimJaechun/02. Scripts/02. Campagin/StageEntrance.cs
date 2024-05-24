using UnityEngine;

namespace Jc
{
    public class StageEntrance : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        [SerializeField]
        private Animator slopeTileAnim;

        [Header("스테이지 넘버 (0~)")]
        [SerializeField]
        private int stageNumber;

        [SerializeField]
        private Transform stageInfo;   // 스테이지 정보

        private Transform mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main.transform;
        }

        public void ActiveEntrance()
        {
            anim.SetTrigger("OnActive");
            if(slopeTileAnim != null)
                slopeTileAnim.SetTrigger("OnActive");
        }

        public void EnterStage()
        {
            Manager.Scene.LoadLevel(SceneManager.SceneType.InGame + stageNumber);
        }

        private void Update()
        {
            // 빌보드 UI
            if (stageInfo.gameObject.activeSelf)
                stageInfo.LookAt(stageInfo.position + mainCamera.rotation * Vector3.forward,
                    mainCamera.rotation * Vector3.up);
        }

        private void OnTriggerEnter(Collider other)
        {
            stageInfo.gameObject.SetActive(true);
            other.GetComponent<BusController>().stageNumber = stageNumber;
        }

        private void OnTriggerExit(Collider other)
        {
            stageInfo.gameObject.SetActive(false);
            other.GetComponent<BusController>().stageNumber = -1;
        }
    }
}
