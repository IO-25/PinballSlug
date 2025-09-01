using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon Data", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string weaponName = "New Weapon";
    public Sprite weaponIcon;

    [Header("���� ����")]
    public GameObject bulletPrefab;
    public float attackDamage = 1;
    public float attackRate = 1f;
    public int maxAmmo = 30;
    public bool useAmmo = true;

    [Header("ź ���� ����")]
    public float minSpread = 0f;    // �ּ� ���� ����(�� ����)
    public float maxSpread = 15f;    // �ִ� ���� ����
    public float maxDistance = 10f;  // �ִ� �Ÿ�(�� �̻��̸� maxSpread ����)
}
