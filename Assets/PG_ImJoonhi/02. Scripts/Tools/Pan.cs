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

    private void Start()
    {
        ingredientPrefabs = Manager_TEMP.recipemanager.ingredientList;
        rigid = gameObject.GetComponent<Rigidbody>();
        collid = gameObject.GetComponent<BoxCollider>();
        meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        rigid.isKinematic = true;
        collid.enabled = false;
    }

    public bool IngredientIN(IngredientsObject ingredient)
    {
        if(!Cooking && ingredient.ingredientsData.Paned != null && CookingObject == null)
        {
            OnPan(ingredient);
            return true;
        }
        return false;
    }

    public void TakeOut()
    {
        progress = 0f;
        Destroy(CookingObject);
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


    private void OnPan(IngredientsObject ingredient)
    {
        Cooking = true;
        ingredient.GoTo(PanPoint);
        CookingObject = ingredient;
        CookingObject.transform.position = PanPoint.transform.position;
        CookingObject.transform.SetParent(PanPoint.transform, true);
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
