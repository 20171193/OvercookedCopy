using JH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    [Header("Cook State")]
    public bool OnCooker;
    public bool Cooking;
    public float CookSpeed;

    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float progress;
    private double startTime;

    [Header("Potints")]
    [SerializeField] GameObject PanPoint;

    [Header("Debug")]
    [SerializeField] IngredientsObject DebugCookingObject;

    private IngredientsObject CookingObject;

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
        CookingObject = null;
    }

    private void OnPan(IngredientsObject ingredient)
    {
        Cooking = true;
        CookingObject = ingredient;
        CookingObject.transform.position = PanPoint.transform.position;
        CookingObject.transform.SetParent(PanPoint.transform, true);
    }

    public void GoTo(GameObject GoPotint)
    {
        gameObject.transform.SetParent(GoPotint.transform, true);
    }
    public void Drop()
    {
        gameObject.transform.SetParent(null);
    }


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
