using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public WeaponSlot[] weaponSlots;

    public void Awake()
    {
        foreach (var weaponSlot in weaponSlots)
            weaponSlot.Initialize();
    }

    public void SetWeaponSlotSprite(Sprite sprite, int index)
    {
        if (index < 0 || index >= weaponSlots.Length) return;
        weaponSlots[index].SetWeaponSprite(sprite);
    }

    public void SelectWeaponSlot(int index)
    {
        if (index < 0 || index >= weaponSlots.Length) return;
        Debug.Log($"SelectWeaponSlot: {index}");

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i == index)
                weaponSlots[i].Select();
            else
                weaponSlots[i].Deselect();
        }
    }
}
