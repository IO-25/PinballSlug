using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponData defaultWeaponData;
    [SerializeField] private WeaponData subWeaponData;
    private bool isUsingDefaultWeapon = true;
    private PlayerAnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<PlayerAnimationController>();

        Equip(defaultWeaponData);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            weapon?.Fire();
            animationController.SetBool_Upper("IsShooting", true);

            if (!isUsingDefaultWeapon)
            {
                if (weapon.CurrentAmmo <= 0)
                {
                    SwitchWeapon();
                    subWeaponData = null;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            animationController.SetBool_Upper("IsShooting", false);
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
