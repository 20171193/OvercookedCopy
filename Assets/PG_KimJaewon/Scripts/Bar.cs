using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    Slider slTimer;
    float fSliderBarTime = 20f; // 슬라이더의 초기 값을 20으로 설정

    void Start()
    {
        slTimer = GetComponent<Slider>();
        slTimer.maxValue = fSliderBarTime;
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
            Debug.Log("Time is Zero.");
        }
    }
}
