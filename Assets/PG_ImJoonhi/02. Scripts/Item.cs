using UnityEngine;
using Photon.Pun;

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
    [PunRPC]
    public void GoTo2(Transform GoPotint)
    {
        rigid.isKinematic = true;
        collid.enabled = false;
        gameObject.layer = 7;
        gameObject.transform.position = GoPotint.position;
        gameObject.transform.rotation = GoPotint.rotation;
        gameObject.transform.SetParent(GoPotint, true);
    }
    public void GoTo(GameObject GoPotint)
    {
        rigid.isKinematic = true;
        collid.enabled = false;
        gameObject.layer = 7;
        gameObject.transform.position = GoPotint.transform.position;
        gameObject.transform.rotation = GoPotint.transform.rotation;
        gameObject.transform.SetParent(GoPotint.transform, true);
    }

    public void Drop()
    {
        rigid.isKinematic = false;
        collid.enabled = true;
        gameObject.layer = 8;
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

public enum ItemType { Ingredient, Plate, FoodDish, Pan, Pot, Extinguisher }