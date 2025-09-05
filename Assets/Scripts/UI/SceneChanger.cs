using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void GoStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    
    public static void GoGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public static void GoGameOverScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    public static void GoGameClearScene()
    {
        SceneManager.LoadScene("ClearScene");
    }
}  

