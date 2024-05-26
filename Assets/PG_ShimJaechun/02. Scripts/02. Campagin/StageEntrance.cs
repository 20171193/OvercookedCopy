using Cinemachine;
using UnityEngine;

namespace Jc
{
    public class StageEntrance : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private Animator anim;

        [SerializeField]
        private Animator slopeTileAnim;

        [SerializeField]
        private CinemachineVirtualCamera mainCam;
        [SerializeField]
        private CinemachineVirtualCamera subCam;

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
            // 효과음 출력
            if(!audioSource.isPlaying)
                audioSource.Play();

            // 카메라 연출
            Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0.1f;
            int temp = mainCam.Priority;
            mainCam.Priority = subCam.Priority;
            subCam.Priority = temp;

            stageInfo.gameObject.SetActive(true);
            other.GetComponent<BusController>().stageNumber = stageNumber;
        }

        private void OnTriggerExit(Collider other)
        {
            // 카메라 연출
            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
            brain.m_DefaultBlend.m_Time = 0.75f;
            brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
            StartCoroutine(Extension.ActionDelay(0.75f,
                () => brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.HardIn));

            int temp = subCam.Priority;
            subCam.Priority = mainCam.Priority;
            mainCam.Priority = temp;

            stageInfo.gameObject.SetActive(false);
            other.GetComponent<BusController>().stageNumber = -1;
        }
    }
}
