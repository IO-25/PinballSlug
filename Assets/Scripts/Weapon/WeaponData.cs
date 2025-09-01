using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon Data", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("공격 관련")]
    public float attackDamage = 1;
    public float attackRate = 1f;
    public GameObject bulletPrefab;

    [Header("탄 퍼짐 설정")]
    public float minSpread = 0f;    // 최소 퍼짐 각도(도 단위)
    public float maxSpread = 15f;    // 최대 퍼짐 각도
    public float maxDistance = 10f;  // 최대 거리(이 이상이면 maxSpread 적용)
}
