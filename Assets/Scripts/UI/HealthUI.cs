using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [Header("Health Images")]
    public GameObject[] healthIcons;

    void OnEnable()
    {

        PlayerHealth.OnHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int currentHealth)
    {
        // 목숨 이미지 배열의 개수만큼 반복
        for (int i = 0; i < healthIcons.Length; i++)
        {
            // 현재 목숨 개수보다 i가 작으면, 하트를 활성화
            if (i < currentHealth)
            {
                healthIcons[i].SetActive(true);
            }
            // 아니면 비활성화
            else
            {
                healthIcons[i].SetActive(false);
            }
        }
    }
}
