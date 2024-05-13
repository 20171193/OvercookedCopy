using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    public enum SceneType
    {
        Title = 0,
        Main,
        Campagin,
        Loading,
        InGame
    }

    // Unity LoadScene
    public void LoadScene(SceneType type)
    {
        UnitySceneManager.LoadScene((int)type);
    }

    // Photon LoadLevel
    // AutomaticallySyncScene
    public void LoadLevel(SceneType type)
    {
        PhotonNetwork.LoadLevel((int)type);
    }
}