using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH {
    public class IngredientList : MonoBehaviour
    {
        [Header("Ingredients Object")]
        public List<IngredientsObject> OriginalingredientsList;
        public List<IngredientsObject> SlicedingredientsList;
        public List<IngredientsObject> PanedingredientsList;
        public List<IngredientsObject> PottedingredientsList;

        private void Start()
        {
            Debug.Log("Recipe Ready");
            if (Manager_TEMP.Inst != null)
                Manager_TEMP.recipemanager.ingredientList = this;
            // Manager.recipemanager.ingredientList = this;
        }

        public IngredientsObject Find(IngredientsObject ingredient)
        {
            switch(ingredient.IngState)
            {
                case IngredientState.Original:
                    return Original(ingredient);
                case IngredientState.Sliced:
                    return Sliced(ingredient);
                case IngredientState.Paned:
                    return Paned(ingredient);
                case IngredientState.Potted:
                    return Potted(ingredient);
            }
            return null;
        }

        private IngredientsObject Original(IngredientsObject ingredient)
        {
            for(int i = 0; i < OriginalingredientsList.Count; i++)
            {
                if (OriginalingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                    return OriginalingredientsList[i]; 
            }
            return null;
        }

        private IngredientsObject Sliced(IngredientsObject ingredient)
        {
            for (int i = 0; i < SlicedingredientsList.Count; i++)
            {
                if (SlicedingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                    return SlicedingredientsList[i];
            }
            return null;
        }

        private IngredientsObject Paned(IngredientsObject ingredient)
        {
            for (int i = 0; i < PanedingredientsList.Count; i++)
            {
                if (PanedingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                    return PanedingredientsList[i];
            }
            return null;
        }

        private IngredientsObject Potted(IngredientsObject ingredient)
        {
            for (int i = 0; i < PottedingredientsList.Count; i++)
            {
                if (PottedingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                    return PottedingredientsList[i];
            }
            return null;
        }
    }
}