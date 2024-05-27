using JH;
using Kyungmin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : Table
{
    [Header("Delivery")]

    // 현재까지의 총 점수
    [SerializeField] int finalScore;
    // 제출 시 얻은 점수
    [SerializeField] int addScore;

    // 새 접시 리스폰되는 곳
    [SerializeField] PlateReturn plateReturn;

    private void Start()
    {
        tableType = TableType.Delivery;
    }

    private void OnEnable()
    {
        // 시작 시 총 점수 0
        finalScore = 0;
    }


    public override bool PutDownItem(Item item)
    {
        // 1. 제출하는 아이템이 (접시 or 재료담긴접시)일 경우 제출
        if (item.Type == ItemType.Plate || item.Type == ItemType.FoodDish)
        {
            base.PutDownItem(item);  // 제출대에 내려놓기

            StartCoroutine(MenuDeliveryRoutine(item));

            return true;
        }

        // 2. 아닐 경우 '접시 필요' 메세지 -> 제출 실패
        else
        {
            Debug.Log("접시가 필요합니다.");
            return false;
        }
    }


    // 제출 코루틴 : 1초 뒤에 사라지면서 점수 더해줌
    IEnumerator MenuDeliveryRoutine(Item item)
    {
        yield return new WaitForSeconds(1.0f);
        RecipeOrder.Inst.SubmitItem((FoodDish)item);
        Destroy(item.gameObject);

        // 제출한 접시 사라지면 새 접시 리스폰 코루틴 시작됨
        plateReturn.PlateRespawn();
    }


    public override Item PickUpItem()
    {
        // 집어들기 불가능
        Debug.Log("집기 불가능");
        return null;
    }
}
