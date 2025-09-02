using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBasedGauge : MonoBehaviour
{
    public SmoothGauge smoothGauge;

    // 게이지가 모두 채워지는 데 걸리는 시간
    public float totalFillTime = 360f;

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        float progress = elapsedTime / totalFillTime;

        // 게이지 총 칸 수에 진행률을 곱하여 게이지 값 계산
        float gaugeValue = progress * smoothGauge.gaugeImages.Count;
        
        if (smoothGauge != null)
        {
            smoothGauge.SetGaugeValue(gaugeValue);
        }
    }
}
