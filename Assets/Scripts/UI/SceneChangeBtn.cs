using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeBtn : MonoBehaviour
{
    public void ToGameScene()
    {
        SceneChanger.GoGameScene();
    }

    public void ToMainScene()
    {
        SceneChanger.GoStartScene();
    }

    public void ToGameOverScene()
    {
        SceneChanger.GoGameOverScene();
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
