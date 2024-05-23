using JH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : Item
{
    [Header("Map Recipes")]
    [SerializeField] IngredientList ingredientPrefabs;

    private void Start()
    {
        ingredientPrefabs = Manager_TEMP.recipemanager.ingredientList;
        rigid = gameObject.GetComponent<Rigidbody>();
        collid = gameObject.GetComponent<BoxCollider>();
        meshRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        rigid.isKinematic = true;
        collid.enabled = false;
    }

}
