using UnityEngine;
using Photon.Pun;

public class Item : MonoBehaviourPun, IPickable, IHighlightable
{
    [Header("Components")]
    public Rigidbody rigid;
    public Collider collid;
    public MeshRenderer meshRenderer;
    public ItemType Type;

    private Material originMT;
    private Material changeMT;
    [PunRPC]
    // photonView.RPC("DestroyItem", RpcTarget.MasterClient);
    public void DestroyItem()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    // IPickable
    [PunRPC]
    public void Hold(int HoldPointPhotonID)
    {
        Debug.Log($"Finding {HoldPointPhotonID}");
        Transform HoldPoint = PhotonView.Find(HoldPointPhotonID).gameObject.transform;
        Debug.Log($"Found {HoldPoint.gameObject.name}");
        rigid.isKinematic = true;
        collid.enabled = false;
        gameObject.layer = 7;
        gameObject.transform.position = HoldPoint.position;
        gameObject.transform.rotation = HoldPoint.rotation;
        gameObject.transform.SetParent(HoldPoint, true);
        Debug.Log($"Hold {HoldPoint.gameObject.name}");
    }

    public void GoTo(GameObject GoPoint)
    {
        /*
        rigid.isKinematic = true;
        collid.enabled = false;
        gameObject.layer = 7;
        gameObject.transform.position = GoPoint.transform.position;
        gameObject.transform.rotation = GoPoint.transform.rotation;
        gameObject.transform.SetParent(GoPoint.transform, true);
        */
        Debug.Log($"GOTO {GoPoint.name}");
        Debug.Log($"GOTO ID {GoPoint.GetPhotonView().ViewID}");
        photonView.RPC("Hold", RpcTarget.All, GoPoint.GetPhotonView().ViewID);
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