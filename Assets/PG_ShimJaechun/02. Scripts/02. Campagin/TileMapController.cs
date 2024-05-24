using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

namespace Jc
{
    public class TileMapController : MonoBehaviourPun, IPunObservable
    { 
        // 카메라 컨트롤용
        [SerializeField]
        private CinemachineVirtualCamera mainCam;
        [SerializeField]
        private CinemachineVirtualCamera[] stageCams;

        [SerializeField]
        private float cameraActionTime;
        private const int currentPriority = 4;  // 현재 카메라 우선순위

        // 스테이지 별 타일맵
        [SerializeField]
        private Transform[] stageTileMaps;

        // 스테이지 별 입구
        [SerializeField]
        private StageEntrance[] stageEntrances;


        [SerializeField]
        private List<StageTileInfo> stageTileInfos = new List<StageTileInfo>();

        [SerializeField]
        private float cycleTime;

        private Coroutine openStageRoutine;

        private int changedTileCnt = 0;
        private int tileCnt = 0;
        [SerializeField]
        private int curStage = 0;

        [SerializeField]
        private int clearStage = 0;

        private void Start()
        {
            TileSetUp();
            LoadMasterData();
        }

        private void Update()
        {
            // 디버그 전용
            if(PhotonNetwork.IsMasterClient && Input.GetKey(KeyCode.O))
            {
                RequestOpenStage(0);
            }
            if (PhotonNetwork.IsMasterClient && Input.GetKey(KeyCode.P))
            {
                RequestOpenStage(1);
            }
        }

        // 타일맵 세팅
        private void TileSetUp()
        {
            foreach (Transform tileMap in stageTileMaps)
            {
                StageTileInfo tileInfo = new StageTileInfo();
                tileInfo.depthList = new List<TileDepthInfo>();

                if (tileMap.childCount < 1) return;

                for (int i = 0; i < 3; i++)
                {
                    // 스테이지 타일맵
                    //  -> depth 별 타일맵
                    Transform depthTileMap = tileMap.GetChild(i);
                    TileDepthInfo depthInfo = new TileDepthInfo(new List<ChangeableTile>());

                    for (int j = 0; j < depthTileMap.childCount; j++)
                    {
                        depthInfo.tileList.Add(depthTileMap.GetChild(j).GetComponent<ChangeableTile>());
                    }

                    tileInfo.depthList.Add(depthInfo);
                }

                stageTileInfos.Add(tileInfo);
            }
        }

        // 마스터의 스테이지 클리어정보 받아오기
        [ContextMenu("SetTile")]
        private void LoadMasterData()
        {
            // 로드한 데이터를 기반으로 클리어한 스테이지 미리 오픈
            for (int i = 0; i < clearStage; i++)
            {
                RequestOpenedStage(i);
            }
        }

        [PunRPC]
        private void RequestOpenStage(int stageNumber)
        {
            // 스테이지 오픈 요청
            photonView.RPC("OpenedStage", RpcTarget.AllViaServer, stageNumber);
        }
        [PunRPC]
        private void RequestOpenedStage(int stageNumber)
        {
            // 로드된 스테이지 미리 오픈 요청
            photonView.RPC("OpenedStage", RpcTarget.AllViaServer, stageNumber);
        }

        // 스테이지 오픈
        [PunRPC]
        private void OpenStage(int stageNumber)
        {
            curStage = stageNumber;
            // 카메라 액션
            ChangeCamera(stageNumber);
            // 타일 오픈
            openStageRoutine = StartCoroutine(OpenStageRoutine(stageNumber));
        }
        // 로드된 스테이지 미리 오픈
        [PunRPC]
        private void OpenedStage(int stageNumber)
        {
            // 스테이지 타일 세팅
            for (int i = 0; i < 3; i++)
            {
                List<ChangeableTile> tileList = stageTileInfos[stageNumber].depthList[i].tileList;
                foreach (ChangeableTile tile in tileList)
                {
                    tile.OnChangedSetting();
                }
            }

            // 스테이지 입구 세팅
            stageEntrances[stageNumber].gameObject.SetActive(true);
            stageEntrances[stageNumber].ActiveEntrance();
        }

        IEnumerator OpenStageRoutine(int stageNumber)
        {
            // depth 별 뒤집기
            int curDepth = 0;
            while (curDepth < 3)
            {
                yield return new WaitForSeconds(cycleTime);
                ChangeTile(stageNumber, curDepth++);
            }
        }
        private void ChangeTile(int stageNumber, int depth)
        {
            List<ChangeableTile> tileList = stageTileInfos[stageNumber].depthList[depth].tileList;
            for (int i = 0; i < tileList.Count; i++)
            {
                tileCnt++;
                tileList[i].OnChangeTile();
                tileList[i].OnChangedTile += CountingChangedTile;
            }
        }
        public void CountingChangedTile()
        {
            changedTileCnt++;
            // 모든 타일이 뒤집힌경우
            if (changedTileCnt >= tileCnt)
            {
                changedTileCnt = 0;
                tileCnt = 0;
                OnStageEntrance();
            }
        }
        private void OnStageEntrance()
        {
            stageEntrances[curStage].gameObject.SetActive(true);
            stageEntrances[curStage].ActiveEntrance();
        }

        [ContextMenu("ResetTile")]
        private void ResetTile()
        {
            for (int i = 0; i < 3; i++)
            {
                List<ChangeableTile> tileList = stageTileInfos[0].depthList[i].tileList;
                for (int j = 0; j < tileList.Count; j++)
                {
                    tileList[j].ResetTile();
                }
            }
        }

        // 카메라 우선순위 초기세팅
        private void CameraInitSetting()
        {
            mainCam.Priority = currentPriority;
            foreach(CinemachineVirtualCamera cam in stageCams)
            {
                cam.Priority = 0;
            }
        }

        // 카메라 액션
        private void ChangeCamera(int stageNumber = 0)
        {
            mainCam.Priority = 0;
            for(int i =0; i<stageCams.Length; i++)
            {
                if (i == stageNumber)
                    stageCams[i].Priority = currentPriority;
            }
            StartCoroutine(CameraActionRoutine());
        }

        IEnumerator CameraActionRoutine()
        {
            yield return new WaitForSeconds(cameraActionTime);
            CameraInitSetting();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }
    }
}
