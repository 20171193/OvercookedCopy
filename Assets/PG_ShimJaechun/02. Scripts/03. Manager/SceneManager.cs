using Jc;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    public enum SceneType
    {
        Title = -1,
        Main,
        Campagin,
        InGame
    }
    [SerializeField]
    private LoadingPanel loadingPanel;
    private float loadDelayTime = 2f;
    public UnityAction OnDisconnected;

    // 로드 씬 (플레이어 별 씬 변경)
    /// <summary>
    /// Unity LoadScene
    /// </summary>
    /// <param name="type"></param>
    public void LoadScene(SceneType type)
    {
        if(type == SceneType.Title) // 타이틀 씬 (로그인 씬 이동)
        { 
            UnitySceneManager.LoadScene((int)type + 1);
            OnDisconnected?.Invoke();
        }
        else
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

    // 로드 딜레이 (씬 전환 페이드인/아웃)
    public void LoadLevelWithDelay(SceneType type, int ingameNumber = 0)
    {
        FadeIn();
        StartCoroutine(LoadLevelDelayRoutine(type, ingameNumber));
    }


    public void FadeIn()
    {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.FadeIn(loadDelayTime);
    }
    public void FadeOut()
    {
        loadingPanel.gameObject.SetActive(true);
        loadingPanel.FadeOut(loadDelayTime);
    }
    IEnumerator LoadLevelDelayRoutine(SceneType type, int ingameNumber = 0)
    {
        yield return new WaitForSeconds(loadDelayTime);
        PhotonNetwork.LoadLevel((int)type + ingameNumber);
    }
}