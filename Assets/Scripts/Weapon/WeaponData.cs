using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon Data", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("���� ����")]
    public float attackDamage = 1;
    public float attackRate = 1f;
    public GameObject bulletPrefab;

    [Header("ź ���� ����")]
    public float minSpread = 0f;    // �ּ� ���� ����(�� ����)
    public float maxSpread = 15f;    // �ִ� ���� ����
    public float maxDistance = 10f;  // �ִ� �Ÿ�(�� �̻��̸� maxSpread ����)
}
