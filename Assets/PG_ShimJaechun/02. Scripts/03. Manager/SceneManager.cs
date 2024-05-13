using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    public enum SceneType
    {
        Title,
        Main,
        Campagin,
        Loading,
        InGame
    }

    public void LoadLevel(SceneType type)
    {
        PhotonNetwork.LoadLevel((int)type);
    }
}