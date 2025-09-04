using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItemData", menuName = "ScriptableObjects/DropItemData")]
public class DropItemData : ScriptableObject
{
    public WeaponType weaponType; // 무기 타입
    public float floatStrength = 1f; // 떠다니는 힘
    public Sprite dropItemSprite; // 아이템 스프라이트
}
