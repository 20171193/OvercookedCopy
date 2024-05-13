using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    public class RecipeList : MonoBehaviour
    {
        public List<RecipeData> Recipe { get; private set; }
        public List<RecipeData> finishedRecipe { get; private set; }
    }

}