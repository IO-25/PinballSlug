using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayManager : MonoBehaviour
{
    public List<Image> scoreImages;
    public List<Sprite> numberSprites;
    public Image infinityImage;
    public Sprite infinitySprite;
    public Material chromaKeyMaterial;
    private bool infinityMode = false;

    private int currentScore;

    public void UpdateScore(int score)
    {
        currentScore = score;
        DisplayScore();
    }
    public void SetInfinity(bool infinity)
    {
        infinityMode = infinity;
        DisplayScore();
    }

    private void DisplayScore()
    {
        if (infinityMode)
        {
            foreach (var scoreImage in scoreImages)
                scoreImage.enabled = false;

            infinityImage.enabled = true;
            infinityImage.sprite = infinitySprite;
        }
        else
        {
            infinityImage.enabled = false;

            // D3 포맷을 사용하여 항상 3자리, 부족한 자리는 0으로
            string scoreString = currentScore.ToString("D3");
            int scoreLength = scoreString.Length;

            // 점수 이미지들을 오른쪽(1의 자리)부터 순서대로 처리
            for (int i = 0; i < scoreImages.Count; i++)
            {
                // 점수 문자열의 인덱스 계산 (예: 123 -> i=0일때 '3', i=1일때 '2')
                int scoreIndex = scoreLength - 1 - i;

                if (scoreIndex >= 0)
                {
                    // 현재 자릿수의 숫자를 가져옴
                    int digit = int.Parse(scoreString[scoreIndex].ToString());

                    // UI 이미지를 해당 숫자로 변경
                    scoreImages[i].sprite = numberSprites[digit];
                    scoreImages[i].enabled = true;
                }
                else
                {
                    // 점수 자릿수가 부족하면 이미지를 비활성화
                    scoreImages[i].enabled = false;
                }
            }
        }
    }
}