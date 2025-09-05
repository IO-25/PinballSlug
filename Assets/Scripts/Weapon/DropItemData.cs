using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItemData", menuName = "ScriptableObjects/DropItemData")]
public class DropItemData : ScriptableObject
{
    public WeaponType weaponType; // ���� Ÿ��
    public float floatStrength = 1f; // ���ٴϴ� ��
    public Sprite dropItemSprite; // ������ ��������Ʈ
}
