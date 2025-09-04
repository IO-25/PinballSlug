using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    public Image weaponImage;
    public Image selectionImage;

    public void SetWeaponSprite(Sprite sprite)
    {
        weaponImage.sprite = sprite;
        weaponImage.enabled = weaponImage.sprite != null;
    }

    public void Initialize()
    {
        weaponImage.enabled = false;
        selectionImage.enabled = false;
    }

    public void Select()
        => selectionImage.enabled = true;

    public void Deselect()
        => selectionImage.enabled = false;
}
