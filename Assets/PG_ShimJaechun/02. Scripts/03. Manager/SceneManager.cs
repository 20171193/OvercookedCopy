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

    // 로드 씬 (플레이어 별 씬 변경)
    /// <summary>
    /// Unity LoadScene
    /// </summary>
    /// <param name="type"></param>
    public void LoadScene(SceneType type)
    {
        UnitySceneManager.LoadScene((int)type);
    }


    // 포톤 로드씬 (방장과 함께 이동)
    /// <summary>
    /// Photon LoadLevel (AutomaticallySyncScene)
    /// </summary>
    /// <param name="type"></param>
    public void LoadLevel(SceneType type, int ingameNumber = 0)
    {
        PhotonNetwork.LoadLevel((int)type + ingameNumber);
    }
}