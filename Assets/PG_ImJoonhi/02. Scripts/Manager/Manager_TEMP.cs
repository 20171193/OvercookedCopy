using UnityEngine;

namespace JH
{
    public class Manager_TEMP : MonoBehaviour
    {
        private static Manager_TEMP instance;
        public static Manager_TEMP Inst { get { return instance; } }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Function()
        {

        }

        [Header("Managers")]
        [SerializeField] RecipeManager recipeManager;

        public static RecipeManager recipemanager { get { return instance.recipeManager; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
