using JH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateReturn : Table
{
    [Header("PlateReturn")]

    // 제출 후 새 접시 리스폰 되기까지 걸리는 시간
    [SerializeField] float plateRespawnTime;
    // 새 접시 스폰 위치
    [SerializeField] GameObject spawnPoint;

    // 스폰된 새 접시
    [SerializeField] GameObject spawnPlate;
    // 쌓여있는 새 접시 리스트
    [SerializeField] List<Item> placedPlateList;

    private void Start()
    {
        tableType = TableType.PlateReturn;
    }

    public void PlateRespawn()
    {
        // spawnPoint 있는지 null 체크 (에러 방지)
        Transform temp = transform.GetChild(childIndex);
        if (temp != null)
        {
            spawnPoint = temp.gameObject;
            generatePoint = null;
        }

        StartCoroutine(PlateRespawnRoutine(spawnPoint));
    }


    // 새 접시 리스폰 코루틴
    IEnumerator PlateRespawnRoutine(GameObject spawnPoint)
    {
        yield return new WaitForSeconds(plateRespawnTime);

        spawnPlate = PhotonNetwork.Instantiate("Plate_burger", spawnPoint.transform.position, spawnPoint.transform.rotation);
        spawnPlate.transform.SetParent(spawnPoint.transform, true);

        Item placedPlate = spawnPlate.GetComponent<Item>();

        // 새 접시 리스트에 추가
        placedPlateList.Add(placedPlate);
    }


    public override Item PickUpItem()
    {
        // 새 접시 리스트 마지막 요소 가져온 뒤 리스트에서 삭제
        int listCnt = placedPlateList.Count;
        Item returnPlate = placedPlateList[listCnt - 1];
        placedPlateList.RemoveAt(listCnt - 1);

        return returnPlate;
    }


    public override bool PutDownItem(Item item)
    {
        // 내려놓기 불가능
        Debug.Log("내려놓기 불가능");
        return false;
    }
}
