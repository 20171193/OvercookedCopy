using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingTable : Table
{
    // 다지기 진행 과정 표시 바
    [SerializeField] BillBoard choppingBar;

    // 칼
    [SerializeField] GameObject knife;

    // 다지기 끝내는데에 걸리는 총 시간(6)
    [SerializeField] float choppingTime;

    // 다지기 현재 진행 시간
    [SerializeField] float currentTime;

    // 다지기 속도
    [SerializeField] float choppingSpeed;

    private void OnEnable()
    {
        if (placedItem != null)
        {
            choppingBar = placedItem.GetComponentInChildren<BillBoard>();
        }
    }

    public override bool PutDownItem(Item item)
    {
        base.PutDownItem(item);
        knife.SetActive(false);

        return true;
    }

    public override Item PickUpItem()
    {
        return base.PickUpItem();
    }

    public override void Interactable()
    {
        if (!PutDownItem())
            return;

        //애니메이션 실행하고
        choppingBar.gameObject.SetActive(true);

    }

    public void ChopIngredient()
    {
        
    }

    
}
