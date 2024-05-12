using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    [CreateAssetMenu(fileName = "Recipe Data", menuName = "Data/Recipe")]
    public class RecipeData : ScriptableObject
    {
        [Header("Status")]
        public string Name;
        public bool Wrong;
        public bool needDish;

        public List<IngredientsData> ingredients;
        public List<IngredientState> ingredientsState;

        public Object Model;
    }
}