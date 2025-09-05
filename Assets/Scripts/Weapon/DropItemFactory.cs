using System.Collections.Generic;
using UnityEngine;

public static class DropItemFactory
{
    public const string DropItemPrefabPath = "Item/DropItem";

    private static readonly Dictionary<WeaponType, string> dropItemDataNameMapping = new()
    {
        { WeaponType.MachineGun, "DropItem_MachineGun" },
        { WeaponType.Shotgun, "DropItem_Shotgun" },
        { WeaponType.Bomb, "DropItem_Bomb" },
    };

    public static DropItem CreateRandomDropItem(Vector3 position)
    {
        List<WeaponType> possibleWeapons = new()
        {
            WeaponType.MachineGun,
            WeaponType.Shotgun,
            WeaponType.Bomb,
        };

        WeaponType randomWeapon = possibleWeapons[Random.Range(0, possibleWeapons.Count)];
        return CreateDropItem(randomWeapon, position);
    }

    public static DropItem CreateDropItem(WeaponType weaponType, Vector3 position)
    {
        GameObject dropItemPrefab = Resources.Load<GameObject>(DropItemPrefabPath);
        GameObject dropItemObject = Object.Instantiate(dropItemPrefab, position, Quaternion.identity);
        DropItem dropItem = dropItemObject.GetComponent<DropItem>();
        dropItem.Initialize(GetDropItemData(weaponType));
        return dropItem;
    }

    public static DropItemData GetDropItemData(WeaponType weaponType)
    {
        if (dropItemDataNameMapping.TryGetValue(weaponType, out string name))
            return Resources.Load<DropItemData>($"Item/Data/{name}");

        return null;
    }

}
