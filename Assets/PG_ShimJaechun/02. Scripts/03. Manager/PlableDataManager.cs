using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jc;

public class PlableDataManager : Singleton<PlableDataManager>
{
    public List<ChefInfo> chefInfos;

    // 로컬유저 스테이지 클리어정보
    public bool[] userStageScore = new bool[3];

    // 파이어 베이스 연동 시 적용
    public bool[] LoadUserStageScore()
    {
        return userStageScore;
    }

    // 파이어 베이스 연동 시 적용
    public void SaveUserStageScore()
    {

    }
}
