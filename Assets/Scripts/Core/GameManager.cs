using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] StageData[] stageDatabase;
    public StageData[] StageDatabase { get { return stageDatabase; }}
}
