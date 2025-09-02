using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private WeaponData defaultWeaponData;
    [SerializeField] private WeaponData subWeaponData;
    [SerializeField] private Weapon bombWeapon;
    [SerializeField] private int bombCount = 3;

    [SerializeField] private Transform upFirePoint;
    [SerializeField] private Transform forwardFirePoint;
    [SerializeField] private Transform downFirePoint;
    [SerializeField] private TrajectoryRenderer trajectoryRenderer;

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

        trajectoryRenderer.RenderTrajectory(GetFirePoint());
        weapon?.Look(GetFirePoint());
    }

    public void Fire()
    {
        weapon?.Fire(GetFirePoint());
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
        bombWeapon?.Fire(GetFirePoint());
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

    public Vector2 GetFirePoint()
    {
        float y = animationController.upperBodyAnimator.GetFloat("Y");
        if (y > 0.5f)
            return upFirePoint.position;
        else if (y < -0.5f)
            return downFirePoint.position;
        else
            return forwardFirePoint.position;
    }
}
