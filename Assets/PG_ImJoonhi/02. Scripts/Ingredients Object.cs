using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsObject : MonoBehaviour
{
    [SerializeField] Ingredients ingredientsData;

    private Object CurrentObject;


    void Start()
    {
        CurrentObject = Instantiate(ingredientsData.Original, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Slice()
    {
        Destroy(CurrentObject);
        Instantiate(ingredientsData.Sliced, gameObject.transform);
    }
}
