using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButtonToGame : AudioButton
{
    protected override void LoadNextScene()
    {
        SceneChanger.GoGameScene();
    }
}
