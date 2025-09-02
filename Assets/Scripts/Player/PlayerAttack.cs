using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponData defaultWeaponData;
    [SerializeField] private WeaponData subWeaponData;
    [SerializeField] private Weapon bombWeapon;
    [SerializeField] private int bombCount = 3;

    private bool isUsingDefaultWeapon = true;
    private PlayerAnimationController animationController;


    private void Start()
    {
        animationController = GetComponent<PlayerAnimationController>();

        Equip(defaultWeaponData);
    }

    void Update()
    {
        Look();

        if (Input.GetMouseButton(0)) 
            Fire();
        else if (Input.GetMouseButtonUp(0))
            animationController.SetBool("IsShooting", false);

        if (Input.GetKeyDown(KeyCode.Q))
            SwitchWeapon();

        if (Input.GetMouseButtonDown(1))
            UseBomb();
    }

    public void Look()
    {
        Vector2 start = transform.position;
        Vector2 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (end - start).normalized;

        animationController.SetFloat_Upper("Y", dir.y);
    }

    public void Fire()
    {
        weapon?.Fire();
        animationController.SetBool("IsShooting", true);

        if (!isUsingDefaultWeapon)
        {
            if (weapon.CurrentAmmo <= 0)
            {
                SwitchWeapon();
                subWeaponData = null;
            }
        }
    }

    public void UseBomb()
    {
        if (bombCount <= 0) return;

        bombCount--;
        bombWeapon?.Fire();
        Debug.Log("ÆøÅº »ç¿ë!!! ³²Àº ÆøÅº: " + bombCount);
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
        animationController.SetAnimController(weaponData.upperAnimController, weaponData.lowerAnimController);
        Debug.Log($"Equipped {weaponData.weaponName}");
    }
}
