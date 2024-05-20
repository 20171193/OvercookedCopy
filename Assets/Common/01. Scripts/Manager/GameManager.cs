using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Jc;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 인게임 모드
        if(PhotonNetwork.InRoom)
        {
            PhotonNetwork.LocalPlayer.SetLoad(true);
        }
        // 디버깅 모드
        else
        {
            Debug.Log("디버깅 모드 시작.");
            PhotonNetwork.LocalPlayer.NickName = $"DebugPlayer {Random.Range(1000, 10000)}";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions() { IsVisible = false };
        PhotonNetwork.JoinOrCreateRoom("DebugRoom", options, TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected : {cause}");
        Manager.Scene.LoadScene(SceneManager.SceneType.Title);
    }

    public override void OnLeftRoom()
    {
        Manager.Scene.LoadLevel(SceneManager.SceneType.Campagin);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }



    void Update()
    {
        
    }
}
