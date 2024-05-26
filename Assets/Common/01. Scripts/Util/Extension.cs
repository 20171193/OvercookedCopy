using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public static class Extension
{
    // 레이어마스크가 해당 레이어를 포함하고 있는지 체크
    public static bool Contain(this LayerMask layerMask, int layer)
    {
        return ((1 << layer) & layerMask) != 0;
    }

    // 일정시간 딜레이 후 Action
    public static IEnumerator ActionDelay(float delayTime, UnityAction action)
    {
        yield return new WaitForSeconds(delayTime);
        action?.Invoke();
    }
}

