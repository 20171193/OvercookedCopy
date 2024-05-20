using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH {
    public class IngredientList : MonoBehaviour
    {
        [Header("Ingredients Object")]
        public List<IngredientsObject> ingredientsObjectsList;

        private void Start()
        {
            Debug.Log("Recipe Ready");
            if (Manager_TEMP.Inst != null)
                Manager_TEMP.recipemanager.ingredientList = this;
            // Manager.recipemanager.ingredientList = this;
        }
    }
}