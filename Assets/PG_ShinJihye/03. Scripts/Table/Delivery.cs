using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : Table
{

    public override void PutDownItem(Item item)
    {
        base.PutDownItem(item);  // 제출대에 내려놓고

        Debug.Log(item.name);
        Destroy(item.gameObject);
    }
}
