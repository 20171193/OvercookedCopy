using JH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pan : Item
{
    [Header("Cook State")]
    public bool OnCooker;
    public bool Cooking;
    public float CookSpeed;

    [SerializeField]
    [Range(0.0f, 15.0f)]
    private float progress;
    private double startTime;

    [Header("Potints")]
    [SerializeField] GameObject PanPoint;

    [Header("Map Recipes")]
    [SerializeField] IngredientList ingredientPrefabs;

    [Header("Debug")]
    [SerializeField] IngredientsObject DebugCookingObject;

    public IngredientsObject CookingObject { get; private set; }

    private void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        collid = gameObject.GetComponent<BoxCollider>();
        rigid.isKinematic = true;
        collid.enabled = false;
    }

    private void Start()
    {
        ingredientPrefabs = Manager_TEMP.recipemanager.ingredientList;
        meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        SetOriginMT();
    }

    public bool IngredientIN(IngredientsObject ingredient)
    {
        if(!Cooking && ingredient.ingredientsData.Paned != null && ingredient.IngState != IngredientState.Paned && CookingObject == null)
        {
            // OnPan(ingredient);
            photonView.RPC("OnPan", RpcTarget.All, ingredient.photonView.ViewID);
            return true;
        }
        return false;
    }

    public void TakeOut()
    {
        progress = 0f;
        // Destroy(CookingObject);
        CookingObject.gameObject.GetPhotonView().RPC("DestroyItem", RpcTarget.MasterClient);
        CookingObject = null;
    }

    public bool isWellDone()
    {
        if (progress >= 10f && progress <= 15f)
            return true;
        return false;
    }

    public bool isEmpty()
    {
        if (CookingObject == null)
            return true;
        return false;
    }

    [PunRPC]
    private void OnPan(int ingredient)
    {
        Cooking = true;
        CookingObject = PhotonView.Find(ingredient).gameObject.GetComponent<IngredientsObject>();
        CookingObject.GoTo(PanPoint);
        // CookingObject.transform.position = PanPoint.transform.position;
        // CookingObject.transform.SetParent(PanPoint.transform, true);

        // 임시
        CookingObject.PanHeated();
        Cooking = false;
        progress = 10f;
    }

    /*
    public void GoTo(GameObject GoPotint)
    {
        gameObject.transform.SetParent(GoPotint.transform, true);
    }
    public void Drop()
    {
        gameObject.transform.SetParent(null);
    }
    */


    #region Debug
#if UNITY_EDITOR
    [ContextMenu("[Debug]DebugIngredient Cook")]
    public void DebugCook()
    {
        IngredientIN(DebugCookingObject);
    }

    [ContextMenu("[Debug]Done")]
    public void DebugDone()
    {
        CookingObject.PanHeated();
    }

    [ContextMenu("[Debug]TIme")]
    public void DebugTime()
    {
        startTime = PhotonNetwork.Time;
        Debug.Log(startTime);
    }
#endif
    #endregion
}
