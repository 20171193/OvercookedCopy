using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class TileMapController : MonoBehaviour
    {
        [SerializeField]
        private Transform[] stageTileMaps;

        [SerializeField]
        private StageEntrance[] stageEntrances;


        [SerializeField]
        private List<StageTileInfo> stageTileInfos = new List<StageTileInfo>();

        [SerializeField]
        private float cycleTime;

        private Coroutine openStageRoutine;

        private int changedTileCnt = 0;
        private int tileCnt = 0;
        private int curStage = 0;

        [SerializeField]
        private int clearStage = 0;

        private void Start()
        {
            TileSetUp();
            LoadMasterData();
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
                OpenedStage(i);
            }
            //OpenStage(0);
        }

        private void OpenedStage(int stageNumber)
        {
            // 스테이지 타일 세팅
            for(int i =0; i<3; i++)
            {
                List<ChangeableTile> tileList = stageTileInfos[stageNumber].depthList[i].tileList;
                foreach(ChangeableTile tile in tileList)
                {
                    tile.OnChangedSetting();
                }
            }

            // 스테이지 입구 세팅
            stageEntrances[stageNumber].gameObject.SetActive(true);
            stageEntrances[stageNumber].ActiveEntrance();
        }

        private void OpenStage(int stageNumber)
        {
            curStage = stageNumber;
            // 카메라 액션
            // 타일 오픈
            openStageRoutine = StartCoroutine(OpenStageRoutine(stageNumber));
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
            if(changedTileCnt >= tileCnt)
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
    }
}
