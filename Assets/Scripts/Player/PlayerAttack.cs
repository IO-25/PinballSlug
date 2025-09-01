using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponData defaultWeaponData;
    [SerializeField] private WeaponData subWeaponData;
    private bool isUsingDefaultWeapon = true;

    private void Start()
    {
        Equip(defaultWeaponData);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            weapon?.Fire();

            if (!isUsingDefaultWeapon)
            {
                if (weapon.CurrentAmmo <= 0)
                {
                    SwitchWeapon();
                    subWeaponData = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    public void SwitchWeapon()
    {
        if(subWeaponData == null) return;

        isUsingDefaultWeapon = !isUsingDefaultWeapon;
        Equip(isUsingDefaultWeapon ? defaultWeaponData : subWeaponData);
    }

    public void Equip(WeaponData weaponData)
    {
        weapon.Initialize(weaponData);
        Debug.Log($"Equipped {weaponData.weaponName}");
    }
}
