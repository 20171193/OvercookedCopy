using UnityEngine;
using UnityEngine.UI;

namespace KJW
{
    public class SliderTimer : MonoBehaviour
    {
        Slider slTimer;
        float fSliderBarTime = 8f;
        Camera mainCamera;

        void Start()
        {
            slTimer = GetComponent<Slider>();
            slTimer.maxValue = fSliderBarTime;
            slTimer.minValue = 0f;
            slTimer.value = fSliderBarTime; // 슬라이더의 값을 초기화
        }

        void Update()
        {
            if (slTimer.value > 0f)
            {
                // 시간이 지남에 따라 슬라이더의 값을 감소시킴
                slTimer.value -= Time.deltaTime;
            }
            else
            {
                slTimer.value = 0f; // 슬라이더 값을 0으로 고정
            }

            if (mainCamera != null)
            {
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                                 mainCamera.transform.rotation * Vector3.up);
            }


        }
    }
}