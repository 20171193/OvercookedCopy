using UnityEngine;
using UnityEngine.UI;

public class BillboardUI : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // 메인 카메라 참조
    }

    void Update()
    {
        // 빌보드 효과를 적용하여 UI가 카메라를 향하도록 만듦
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }
    }
}
