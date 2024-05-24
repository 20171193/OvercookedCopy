using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : Table
{
    [SerializeField] int finalScore;
    [SerializeField] int addScore;
    [SerializeField] PlateReturn plateReturn;

    private void OnEnable()
    {
        finalScore = 0;
    }

    public override bool PutDownItem(Item item)
    {
        // 제출하는 아이템이 (접시 or 재료담긴접시)일 경우 제출
        if (item.Type == ItemType.Plate || item.Type == ItemType.FoodDish)
        {
            base.PutDownItem(item);  // 제출대에 내려놓기

            StartCoroutine(MenuDeliveryRoutine(item));

            return true;
        }
        
        // 아닐 경우 '접시 필요' 메세지 -> 제출 실패
        Debug.Log("접시가 필요합니다.");  // UI로 메세지 띄워주기
        return false;
    }

    // 제출 코루틴 : 1초 뒤에 사라지면서 점수 더해줌
    IEnumerator MenuDeliveryRoutine(Item item)
    {
        finalScore += addScore;  // UI로 받은 점수 띄워주기

        yield return new WaitForSeconds(1.0f);

        Destroy(item.gameObject);

        plateReturn.PlateRespawn();
    }

    public override Item PickUpItem()
    {
        // 집어들기 불가능
        return null;
    }
}
