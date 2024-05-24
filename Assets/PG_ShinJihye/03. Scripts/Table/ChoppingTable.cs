using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingTable : Table
{
    [SerializeField] float choppingTime; // 6번
    [SerializeField] float currentTime;
    [SerializeField] float choppingSpeed;

    [SerializeField] BillBoard choppingBar;

    private void OnEnable()
    {
        if (placedItem != null)
        {
            choppingBar = placedItem.GetComponentInChildren<BillBoard>();
        }
    }

    public override bool PutDownItem(Item item)
    {
        return base.PutDownItem(item);
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
