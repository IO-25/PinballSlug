using System.Collections.Generic;
using UnityEngine;

public static class DropItemFactory
{
    public const string DropItemPrefabPath = "Item/DropItem";

    private static Dictionary<WeaponType, string> spriteNameMapping = new()
    {
        { WeaponType.MachineGun, "HM_0" },
        { WeaponType.Shotgun, "ShotGun_0" },
    };

    public static DropItem CreateRandomDropItem(Vector3 position)
    {
        List<WeaponType> possibleWeapons = new()
        {
            WeaponType.MachineGun,
            WeaponType.Shotgun,
        };

        WeaponType randomWeapon = possibleWeapons[Random.Range(0, possibleWeapons.Count)];
        return CreateDropItem(randomWeapon, position);
    }

    public static DropItem CreateDropItem(WeaponType weaponType, Vector3 position)
    {
        GameObject dropItemPrefab = Resources.Load<GameObject>(DropItemPrefabPath);
        GameObject dropItemObject = Object.Instantiate(dropItemPrefab, position, Quaternion.identity);
        DropItem dropItem = dropItemObject.GetComponent<DropItem>();
        dropItem.Initialize(weaponType, GetDropItemSprite(weaponType));
        return dropItem;
    }

    public static Sprite GetDropItemSprite(WeaponType weaponType)
    {
        string spriteName = string.Empty;

        if (spriteNameMapping.TryGetValue(weaponType, out spriteName))
            return LoadSpriteFromSheet("Item/Items", spriteName);

        return null;
    }

    private static Sprite LoadSpriteFromSheet(string sheetPath, string spriteName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(sheetPath);
        return System.Array.Find(sprites, s => s.name == spriteName);
    }
}
