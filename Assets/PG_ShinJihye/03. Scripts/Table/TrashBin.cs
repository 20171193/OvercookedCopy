using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBin : Table
{
    [SerializeField] float speed;
    [SerializeField] float destroyScale;
    
    public override void PutDownItem(Item item)
    {
        Debug.Log("TrashBin");
        Debug.Log(item.name);

        //StartCoroutine(DestroyItem(item));
    }

    IEnumerator DestroyItem(Item item)
    {
        //item.trashBinAnime.enabled = false;

        Vector3 cubeScale = item.transform.localScale;
        /*
        float reduceSpeed;
        bool isDestroyScale = itemScale.x < destroyScale;

        while (true)
        {
            reduceSpeed = Time.deltaTime * speed * -1;
            Debug.Log(reduceSpeed);

            itemScale = new Vector3(itemScale.x + reduceSpeed, itemScale.y + reduceSpeed, itemScale.z + reduceSpeed);

            if (isDestroyScale)
            {
                Destroy(item.gameObject);
            }

            yield return null;
        }*/
        yield return null;
    }
}
