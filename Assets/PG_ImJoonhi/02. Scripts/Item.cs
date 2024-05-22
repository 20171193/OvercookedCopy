using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour, IPickable, IHighlightable
{
    [Header("Components")]
    public Rigidbody rigid;
    public Collider collid;
    public MeshRenderer meshRenderer;
    public ItemType Type;

    private Material originMT;
    private Material changeMT;

    // IPickable
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

    // IHighlightable
    public void EnterPlayer()
    {
        meshRenderer.sharedMaterial = changeMT;
    }
    public void ExitPlayer()
    {
        meshRenderer.sharedMaterial = originMT;
    }

    protected void SetOriginMT()
    {
        originMT = meshRenderer.sharedMaterial;
    }
}

public enum ItemType { Ingredient, Plate, FoodDish, Pan, Pot}