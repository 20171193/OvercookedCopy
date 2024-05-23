using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : Table
{
    [SerializeField] int finalScore;
    [SerializeField] int addScore;

    private void OnEnable()
    {
        finalScore = 0;
        addScore = 5;
    }

    public override bool PutDownItem(Item item)
    {
        if (item.Type != ItemType.Plate)
        {
            Debug.Log("접시가 필요합니다.");
            return false;
        }
        Debug.Log("1");
        base.PutDownItem(item);
        Debug.Log("1");

        Debug.Log(item.name);
        StartCoroutine(DeliveryMenu(item));
        return true;
    }

    IEnumerator DeliveryMenu(Item item)
    {
        finalScore += addScore;
        // UI로 받은 점수 띄워주기

        yield return new WaitForSeconds(1.0f);
        Destroy(item.gameObject);
        Debug.Log("제출완료");
    }
}
