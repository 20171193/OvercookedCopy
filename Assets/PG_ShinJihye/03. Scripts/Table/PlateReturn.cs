using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturn : Table
{
    // 제출 후 새 접시 리스폰 되기까지 걸리는 시간
    [SerializeField] float plateRespawnTime;

    // 새 접시 프리팹
    [SerializeField] Item newPlatePrefab;

    // 새 접시 스폰 위치
    [SerializeField] GameObject spawnPoint;

    public void PlateRespawn()
    {
        // spawnPoint 있는지 null 체크 (에러 방지)
        Transform temp = transform.GetChild(childIndex);
        if (temp != null)
        {
            spawnPoint = temp.gameObject;
        }

        StartCoroutine(PlateRespawnRoutine(spawnPoint));
    }

    // 새 접시 리스폰 코루틴
    IEnumerator PlateRespawnRoutine(GameObject spawnPoint)
    {
        yield return new WaitForSeconds(plateRespawnTime);

        Instantiate(newPlatePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public override Item PickUpItem()
    {
        // 디버그 확인
        Item pickedItem = base.PickUpItem();
        return pickedItem;
    }

    public override bool PutDownItem(Item item)
    {
        // 내려놓기 불가능
        return false;
    }
}
