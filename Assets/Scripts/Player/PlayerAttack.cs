using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 관련")]
    [Range(0f, 1f)]
    [SerializeField] private float forwardAttackRange = 0.8f;
    [SerializeField] private Transform upFirePoint;
    [SerializeField] private Transform forwardFirePoint;
    [SerializeField] private Transform downFirePoint;

    [Header("무기 관련 ")]
    [SerializeField] private Transform weaponParent;
    [SerializeField] private int weaponSlotSize = 2;
    [SerializeField] private Weapon defaultWeapon;
    private Weapon[] weaponSlots = null;
    private int currentWeaponIndex = 0;

    [Header("폭탄 관련")]
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private int laserCount = 10;
    [SerializeField] private AudioClip bombEquipSFX;
    private int currentLaserCount = 10;

    private PlayerAnimationController animationController;
    private AudioSource audioSource;

    public int CurrentWeaponIndex
    {
        get => currentWeaponIndex;
        set
        {
            if(value < 0) value = weaponSlots.Length - 1;
            else if(value >= weaponSlots.Length) value = 0;

            CurrentWeapon.SetActiveTrajectory(false);
            currentWeaponIndex = value;
            CurrentWeapon.SetActiveTrajectory(true);

            UpdatePlayerAnimator();
            UIManager.Instance.SelectWeaponSlot(currentWeaponIndex);
            UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
        }
    }

    public Weapon CurrentWeapon
    {
        get
        {
            if(weaponSlots == null) return null;
            if (weaponSlots[currentWeaponIndex]) return weaponSlots[currentWeaponIndex];
            else return defaultWeapon;
        }
    }

    public Vector2 CurrentFirePoint
    {
        get
        {
            float y = animationController.upperBodyAnimator.GetFloat("Y");
            if (y > forwardAttackRange) return upFirePoint.position;
            if (y < -forwardAttackRange) return downFirePoint.position;
            return forwardFirePoint.position;
        }
    }

    void OnEnable()
    {
        Initialize();
    }

    private void Start()
    {
        UpdatePlayerAnimator();
    }

    void OnDisable()
    {
        if (CurrentWeapon != null)
            CurrentWeapon.SetActiveTrajectory(false);
        animationController.SetBool("IsShooting", false);
    }

    void Update()
    {
        HandleLook();
        HandleInput();
    }

    private void Initialize()
    {
        if(animationController == null)
            animationController = GetComponent<PlayerAnimationController>();
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if(UIManager.Instance.weaponUI)
            UIManager.Instance.weaponUI.Initialize();
        currentLaserCount = laserCount;

        weaponSlots ??= new Weapon[weaponSlotSize];

        for (int i = 0; i < weaponSlots.Length; i++)
            UnequipWeapon(i);

        CurrentWeaponIndex = 0;
        UIManager.Instance.UpdateBomb(currentLaserCount);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0)) Fire();
        else if (Input.GetMouseButtonUp(0)) animationController.SetBool("IsShooting", false);
        if (Input.GetKeyDown(KeyCode.Q)) SwitchWeapon();
        if (Input.GetMouseButtonDown(1)) UseBomb();
    }

    private void HandleLook()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

        UpdateDirectionY(dir.y);
        CurrentWeapon.Look(CurrentFirePoint);
    }

    public void Fire()
    {
        if (CurrentWeapon == null) return;

        CurrentWeapon.Fire(CurrentFirePoint);
        animationController.SetBool("IsShooting", true);

        if (CurrentWeapon.CurrentAmmo <= 0)
            UnequipWeapon(currentWeaponIndex);

        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
    }

    private void SwitchWeapon()
        => CurrentWeaponIndex++;

    public void EquipWeapon(WeaponType weaponType)
    {
        GameObject weaponPrefab = Resources.Load<GameObject>($"Weapon/{weaponType}");
        Weapon newWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
        newWeapon.Initialize();

        UnequipWeapon(CurrentWeaponIndex);
        weaponSlots[CurrentWeaponIndex] = newWeapon;
        weaponSlots[CurrentWeaponIndex].SetActiveTrajectory(true);
        AudioClip equipSFX = weaponSlots[CurrentWeaponIndex].WeaponData.equipSFX;
        if (equipSFX != null)
            audioSource.PlayOneShot(equipSFX);

        UpdatePlayerAnimator();
        UIManager.Instance.SetWeaponSlotSprite(GetWeaponIcon(CurrentWeaponIndex), CurrentWeaponIndex);
        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
    }

    public void UnequipWeapon(int index)
    {
        if (weaponSlots[index] == null)
        {
            CurrentWeapon.SetActiveTrajectory(false);
            return;
        }

        Destroy(weaponSlots[index].gameObject);
        weaponSlots[index] = null;
        UpdatePlayerAnimator();
        UIManager.Instance.SetWeaponSlotSprite(GetWeaponIcon(index), index);
    }

    public void EquipBomb()
    {
        currentLaserCount += laserCount;
        audioSource.PlayOneShot(bombEquipSFX);
        UIManager.Instance.UpdateBomb(currentLaserCount);
    }
    public void UseBomb()
    {
        if (currentLaserCount <= 0) return;
        currentLaserCount--;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - CurrentFirePoint).normalized;

        Laser laser = ObjectPoolingManager.Instance.Get(laserPrefab.gameObject, transform.position).GetComponent<Laser>();
        laser.transform.SetParent(transform);

        laser.ShotLaser(CurrentFirePoint, dir);
        UIManager.Instance.UpdateBomb(currentLaserCount);
    }


    private void UpdateDirectionY(float dirY)
    {
        float value = dirY > forwardAttackRange ? 1f :
                      dirY < -forwardAttackRange ? -1f : 0f;

        animationController.SetFloat_Upper("Y", value);
    }

    private Sprite GetWeaponIcon(int index)
    {
        if (weaponSlots[index] == null) return null;
        return weaponSlots[index].WeaponData.weaponIcon;
    }

    private void UpdatePlayerAnimator()
    {
        animationController.SetAnimController(
            CurrentWeapon.WeaponData.upperAnimController,
            CurrentWeapon.WeaponData.lowerAnimController
        );

        float speed = CurrentWeapon.WeaponData.attackRate;
        animationController.SetFloat("ShootSpeed", speed);
    }
}
