using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "ScriptableObjects/Shotgun Data")]
public class ShotgunData : WeaponData
{
    [Header("���� Ư��")]
    public int bulletCount = 10; // �� �ߴ� �߻�Ǵ� �Ѿ� ��
}
