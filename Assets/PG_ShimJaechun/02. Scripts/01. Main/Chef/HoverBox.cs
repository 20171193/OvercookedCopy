using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBox : MonoBehaviour
{
    public GameObject changeButton;

    public void OnClickHoverBox()
    {
        Manager.Sound.PlaySFX(SoundManager.SFXType.PopUp);
        changeButton.SetActive(!changeButton.activeSelf);
    }
}
