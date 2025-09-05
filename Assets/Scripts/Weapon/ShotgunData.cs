using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "ScriptableObjects/Shotgun Data")]
public class ShotgunData : WeaponData
{
    [Header("샷건 특성")]
    public int bulletCount = 10; // 한 발당 발사되는 총알 수
}
