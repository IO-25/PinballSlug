using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon Data", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("�⺻ ����")]
    public string weaponName = "New Weapon";
    public Sprite weaponIcon;
    public RuntimeAnimatorController upperAnimController;
    public RuntimeAnimatorController lowerAnimController;

    [Header("���� ����")]
    public GameObject bulletPrefab; // �Ѿ� ������
    public AudioClip[] fireSFX; // �߻� �Ҹ�
    public float attackDamage = 1; // ���ݷ�
    public float attackRate = 1f; // �ʴ� ���� Ƚ��
    public int maxAmmo = 30;
    public bool useAmmo = true;

    [Header("ź ���� ����")]
    public float minSpread = 0f;    // �ּ� ���� ����(�� ����)
    public float maxSpread = 15f;    // �ִ� ���� ����
    public float maxDistance = 10f;  // �ִ� �Ÿ�(�� �̻��̸� maxSpread ����)
}
