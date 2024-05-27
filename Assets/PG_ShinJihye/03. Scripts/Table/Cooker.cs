using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : Table
{
    [SerializeField] Pan pan;

    private void Start()
    {
        tableType = TableType.Cooker;
        if (placedItem != null)
        {
            Pan pan = placedItem as Pan;
            if (pan != null)
            {
                pan.OnCooker = true;
                pan.stateChanged();
            }
        }
    }

    public override Item PickUpItem()
    {
        Debug.Log("table.PickUpItem");

        Item returnItem = placedItem;

        Pan pan = placedItem as Pan;
        if (pan != null)
        {
            pan.OnCooker = false;
            pan.stateChanged();
        }

        gameObject.GetPhotonView().RPC("EmptyPlacedItem", RpcTarget.All);

        return returnItem;
    }

    public override bool PutDownItem(Item item)
    {
        // 팬이 놓여있는 경우
        if (placedItem != null)
        {
            pan = placedItem as Pan;
            
            if (pan == null)
                return false;

            pan.OnCooker = true;
            bool temp = base.PutDownItem(item);
            pan.stateChanged();
            return temp;
        }

        // 팬이 놓여있지 않는경우
        else
        {
            // 놓으려는게 팬이 아닐 때 (안됨)
            if (item.Type != ItemType.Pan)
                return false;

            // 놓으려는게 팬일 때
            bool temp = base.PutDownItem(item);

            pan = placedItem as Pan;

            if (pan == null)
                return false;

            pan.OnCooker = true;
            pan.stateChanged();
            return temp;
        }
        //pan.IngredientIN();


        return true;
    }
}