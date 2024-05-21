using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IPickable, IHighlightable
{
    public Rigidbody rigid;
    public ItemType Type;
    public void GoTo(GameObject GoPotint)
    {
        rigid.isKinematic = true;
        gameObject.transform.position = GoPotint.transform.position;
        gameObject.transform.rotation = GoPotint.transform.rotation;
        gameObject.transform.SetParent(GoPotint.transform, true);
    }
    public void Drop()
    {
        rigid.isKinematic = false;
        gameObject.transform.SetParent(null);
    }

    public void EnterPlayer()
    {
    }

    public void ExitPlayer()
    {
    }
}

public enum ItemType { Ingredient, Plate, FoodDish, Pan, Pot}