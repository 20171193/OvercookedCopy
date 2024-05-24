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
            int temp = 0;
            switch (ingredient.IngState)
            {
                case IngredientState.Original:
                    return Original(ingredient, ref temp);
                case IngredientState.Sliced:
                    return Sliced(ingredient, ref temp);
                case IngredientState.Paned:
                    return Paned(ingredient, ref temp);
                case IngredientState.Potted:
                    return Potted(ingredient, ref temp);
            }
            return null;
        }

        public IngredientsObject Find(int ingredientTypeNum, int ingredientNum)
        {
            switch (ingredientTypeNum)
            {
                case 0:
                    return OriginalingredientsList[ingredientNum];
                case 1:
                    return SlicedingredientsList[ingredientNum];
                case 2:
                    return PanedingredientsList[ingredientNum];
                case 3:
                    return PottedingredientsList[ingredientNum];
            }
            return null;
        }

        public IngredientsObject Find(IngredientsObject ingredient, ref int state, ref int num)
        {
            switch(ingredient.IngState)
            {
                case IngredientState.Original:
                    state = 0;
                    return Original(ingredient, ref num);
                case IngredientState.Sliced:
                    state = 1;
                    return Sliced(ingredient, ref num);
                case IngredientState.Paned:
                    state = 2;
                    return Paned(ingredient, ref num);
                case IngredientState.Potted:
                    state = 3;
                    return Potted(ingredient, ref num);
            }
            return null;
        }

        private IngredientsObject Original(IngredientsObject ingredient, ref int num)
        {
            for(int i = 0; i < OriginalingredientsList.Count; i++)
            {
                if (OriginalingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                {
                    num = i;
                    return OriginalingredientsList[i];
                }
            }
            return null;
        }

        private IngredientsObject Sliced(IngredientsObject ingredient, ref int num)
        {
            for (int i = 0; i < SlicedingredientsList.Count; i++)
            {
                if (SlicedingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                {
                    num = i;
                    return SlicedingredientsList[i];
                }
            }
            return null;
        }

        private IngredientsObject Paned(IngredientsObject ingredient, ref int num)
        {
            for (int i = 0; i < PanedingredientsList.Count; i++)
            {
                if (PanedingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                {
                    num = i;
                    return PanedingredientsList[i];
                }
            }
            return null;
        }

        private IngredientsObject Potted(IngredientsObject ingredient, ref int num)
        {
            for (int i = 0; i < PottedingredientsList.Count; i++)
            {
                if (PottedingredientsList[i].ingredientsData.id == ingredient.ingredientsData.id)
                {
                    num = i;
                    return PottedingredientsList[i];
                }
            }
            return null;
        }
    }
}