using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour, IPickable, IHighlightable
{
    public Rigidbody rigid;
    public Collider collid;
    public MeshRenderer meshRenderer;
    public ItemType Type;
    public void GoTo(GameObject GoPotint)
    {
        rigid.isKinematic = true;
        collid.enabled = false;
        gameObject.transform.position = GoPotint.transform.position;
        gameObject.transform.rotation = GoPotint.transform.rotation;
        gameObject.transform.SetParent(GoPotint.transform, true);
    }
    public void Drop()
    {
        rigid.isKinematic = false;
        collid.enabled = true;
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