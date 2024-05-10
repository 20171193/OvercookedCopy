using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPanelController : MonoBehaviour
{
    public enum PanelType { Login, Main, Campagin, ChefSelect, Option}

    [SerializeField]
    private TextMeshProUGUI debbugingText;      // 디버깅용 텍스트

    [SerializeField]
    private GameObject mainPanel;               // 메인 선택 패널

    [SerializeField]
    private GameObject campaginSelectPanel;     // 캠페인 선택 패널

    [SerializeField]
    private GameObject chefSelectPanel;         // 요리사 선택 패널

    private void Start()
    {
       
    }

    private void Update()
    {
        
    }

    public void OnClickCampaginButton()
    {

    }
    public void OnClick


    public void OnActivePanel()
    {

    }
}
