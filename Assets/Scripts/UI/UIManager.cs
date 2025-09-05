using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // 숫자 스프라이트
    [Header("Number Sprites")]
    public List<Sprite> numberSprites;
    
    // 점수, 탄약, 폭탄 디스플레이
    [Header("Score Display Settings")]
    public List<Image> scoreImages;
    
    [Header("Ammo & Bomb Display Settings")]
    public List<Image> ammoImages;
    public Image ammoInfinityImage;
    public Sprite ammoInfinitySprite;
    public List<Image> bombImages;

    private int currentScore = 0;
    private int currentAmmo = 0;
    private int currentBomb = 0;

    // 무기 UI 관리
    [Header("Weapon UI")]
    public WeaponUI weaponUI;
    
    // 게이지 관리
    [Header("Gauge Settings")]
    public List<Image> gaugeImages;
    public Sprite emptySprite;
    public List<Sprite> filledSprites;
    public float totalFillTime = 360f;
    
    private float elapsedTime = 0f;
    
    [Header("Gauge Runner")]
    public RectTransform gaugeRunnerInstance;

    void Update()
    {
        UpdateGauge();
    }

    // 점수 관리
    public void UpdateScore(int score)
    {
        currentScore = score;
        DisplayScore(score, scoreImages, numberSprites, null, null, false);
    }
    
    // 탄약 관리
    public void UpdateAmmo(int ammo, bool useAmmo)
    {
        currentAmmo = ammo;
        DisplayScore(ammo, ammoImages, numberSprites, ammoInfinityImage, ammoInfinitySprite, !useAmmo);
    }

    // 폭탄 관리
    public void UpdateBomb(int bomb)
    {
        currentBomb = bomb;
        DisplayScore(bomb, bombImages, numberSprites, null, null, false);
    }
    
    private void DisplayScore(int score, List<Image> images, List<Sprite> sprites, Image infImage, Sprite infSprite, bool isInfinity)
    {
        if (isInfinity)
        {
            if (infImage != null)
            {
                foreach (var image in images)
                    image.enabled = false;
                infImage.enabled = true;
                infImage.sprite = infSprite;
            }
        }
        else
        {
            if (infImage != null) infImage.enabled = false;
            // D3 포맷을 사용하여 항상 3자리, 부족한 자리는 0으로
            string scoreString = score.ToString("D3");
            int scoreLength = scoreString.Length;
            
            // 점수 이미지들을 오른쪽(1의 자리)부터 순서대로 처리
            for (int i = 0; i < images.Count; i++)
            {
                // 점수 문자열의 인덱스 계산 (예: 123 -> i=0일때 '3', i=1일때 '2')
                int scoreIndex = scoreLength - 1 - i;

                if (scoreIndex >= 0)
                {
                    // 현재 자릿수의 숫자를 가져옴
                    int digit = int.Parse(scoreString[scoreIndex].ToString());
                    
                    // UI 이미지를 해당 숫자로 변경
                    images[i].sprite = sprites[digit];
                    images[i].enabled = true;
                }
                else
                {
                    images[i].enabled = false;
                }
            }
        }
    }

    // 무기 UI 관리
    public void SetWeaponSlotSprite(Sprite sprite, int index)
    {
        if (weaponUI == null) return;
        weaponUI.SetWeaponSlotSprite(sprite, index);
    }
    
    public void SelectWeaponSlot(int index)
    {
        if (weaponUI == null) return;
        weaponUI.SelectWeaponSlot(index);
    }
    
    // 게이지 관리
    private void UpdateGauge()
    {
        elapsedTime += Time.deltaTime;
        float progress = elapsedTime / totalFillTime;
        float gaugeValue = progress * gaugeImages.Count;
        SetGaugeValue(gaugeValue);

        // 게이지가 다 찼을 때 클리어 처리
        if (progress >= 1f) 
            SceneChanger.GoGameClearScene();
    }
    public void SetGaugeValue(float gaugeValue)
    {
        int filledSections = Mathf.FloorToInt(gaugeValue);
        float fillProgress = gaugeValue - filledSections;

        // 게이지 채우기
        for (int i = 0; i < gaugeImages.Count; i++)
        {
            if (i < filledSections)
            {
                gaugeImages[i].sprite = filledSprites[filledSprites.Count - 1];
            }
            else if (i == filledSections)
            {
                int spriteIndex = Mathf.FloorToInt(fillProgress * (filledSprites.Count - 1));
                gaugeImages[i].sprite = filledSprites[spriteIndex];
            }
            else
            {
                gaugeImages[i].sprite = emptySprite;
            }
        }

        // 게이지 러너 이동
        if (gaugeRunnerInstance != null && gaugeImages.Count > 0)
        {
            if (filledSections >= gaugeImages.Count) return;
            int next = Mathf.Min(filledSections + 1, gaugeImages.Count - 1);  // 다음 칸
            float t = gaugeValue - filledSections;                          // 0~1 보간값

            float currentX = Mathf.Lerp(
                gaugeImages[filledSections].rectTransform.anchoredPosition.x,
                gaugeImages[next].rectTransform.anchoredPosition.x,
                t
            );

            Vector2 runnerPos = gaugeRunnerInstance.anchoredPosition;
            runnerPos.x = currentX;
            gaugeRunnerInstance.anchoredPosition = runnerPos;
        }
    }

}