using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothGauge : MonoBehaviour
{
    public List<Image> gaugeImages;
    public Sprite emptySprite;
    public List<Sprite> filledSprites;
    
    public void SetGaugeValue(float value)
    {
        int filledSections = Mathf.FloorToInt(value);
        float fillProgress = value - filledSections;

        for (int i = 0; i < gaugeImages.Count; i++)
        {
            if (i < filledSections)
            {
                // 이미 완전히 채워진 칸
                gaugeImages[i].sprite = filledSprites[filledSprites.Count - 1];
            }
            else if (i == filledSections)
            {
                // 현재 채워지는 중인 칸
                int spriteIndex = Mathf.FloorToInt(fillProgress * (filledSprites.Count - 1));
                gaugeImages[i].sprite = filledSprites[spriteIndex];
            }
            else
            {
                // 아직 채워지지 않은 빈 칸
                gaugeImages[i].sprite = emptySprite;
            }
        }
    }
}