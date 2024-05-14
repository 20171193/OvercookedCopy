using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jc
{
    public class TitleController : MonoBehaviour
    {
        private bool isLoadScene;

        private void OnEnable()
        {
            isLoadScene = false;
        }

        private void Update()
        {
            if (!isLoadScene && Input.GetKey(KeyCode.Space))
            {
                isLoadScene = true;
                Manager.Scene.LoadScene(SceneManager.SceneType.Main);
            }
        }
    }
}
