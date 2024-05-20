using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPickable
{
    // 물건을 특정 게임오브젝트 자식으로 보낼떄 (놓기, 이동시키기 등)
    public void GoTo(GameObject GoPotint);
    // 물건을 바닥에 떨어뜨릴때
    public void Drop();
}
