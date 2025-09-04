using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [Header("���� ����")]
    [Range(0f, 1f)]
    [SerializeField] private float forwardAttackRange = 0.8f;
    [SerializeField] private Transform upFirePoint;
    [SerializeField] private Transform forwardFirePoint;
    [SerializeField] private Transform downFirePoint;

    [Header("���� ����")]
    [SerializeField] private Transform weaponParent;
    [SerializeField] private int weaponSlotSize = 2;
    private Weapon[] weaponSlots = null;
    private int currentWeaponIndex = 0;

    [Header("��ź ����")]
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private int laserCount = 10;
    [SerializeField] private AudioClip bombEquipSFX;
    private int currentLaserCount = 10;

    private PlayerAnimationController animationController;
    private AudioSource audioSource;

    public Weapon CurrentWeapon => weaponSlots[currentWeaponIndex];

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
        currentWeaponIndex = 0;
        currentLaserCount = laserCount;

        weaponSlots ??= new Weapon[weaponSlotSize];

        for (int i = 0; i < weaponSlots.Length; i++)
            UnequipWeapon(i);

        Debug.Log(currentWeaponIndex);
        EquipWeapon(WeaponType.Pistol);
        CurrentWeapon.SetActiveTrajectory(true);

        UIManager.Instance.SelectWeaponSlot(currentWeaponIndex);
        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
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
        {
            UnequipWeapon(currentWeaponIndex);
            SwitchWeapon();
        }

        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
    }

    public void UseBomb()
    {
        if (currentLaserCount <= 0) return;
        currentLaserCount--;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - CurrentFirePoint).normalized;

        Laser laser = Instantiate(laserPrefab, transform);
        laser.ShotLaser(CurrentFirePoint, dir);
        UIManager.Instance.UpdateBomb(currentLaserCount);
    }

    private void SwitchWeapon()
    {
        if (weaponSlots.Length <= 1) return;
        if (FindNextWeaponIndex() == currentWeaponIndex) return; // �ٸ� ���Ⱑ ������ ����

        if(CurrentWeapon != null)
            CurrentWeapon.SetActiveTrajectory(false);

        currentWeaponIndex = FindNextWeaponIndex();
        CurrentWeapon.SetActiveTrajectory(true);

        animationController.SetAnimController(
            CurrentWeapon.WeaponData.upperAnimController,
            CurrentWeapon.WeaponData.lowerAnimController
        );

        UIManager.Instance.SelectWeaponSlot(currentWeaponIndex);
        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        int index = currentWeaponIndex;

        if (index == 0 && weaponSlots[0])
        {
            if (FindEmptySlotIndex() == -1) return;
            else index = FindEmptySlotIndex();
        }

        // ���� ���� �� �ʱ�ȭ
        GameObject weaponPrefab = Resources.Load<GameObject>($"Weapon/{weaponType}");
        Weapon newWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
        newWeapon.Initialize();

        if (weaponSlots[index] != null) // ���� ���� ����
            Destroy(weaponSlots[index].gameObject);

        weaponSlots[index] = newWeapon;
        weaponSlots[index].SetActiveTrajectory(false);
        AudioClip equipSFX = weaponSlots[index].WeaponData.equipSFX;
        if (equipSFX != null)
            audioSource.PlayOneShot(equipSFX);
        UIManager.Instance.SetWeaponSlotSprite(GetWeaponIcon(index), index);
        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
    }

    public void UnequipWeapon(int index)
    {
        if (weaponSlots[index] == null) return;

        Debug.Log($"���� ����: {weaponSlots[index].WeaponData.weaponName}");
        Destroy(weaponSlots[index].gameObject);
        weaponSlots[index] = null;
        UIManager.Instance.SetWeaponSlotSprite(GetWeaponIcon(index), index);
    }

    public void EquipBomb()
    {
        currentLaserCount = laserCount;
        audioSource.PlayOneShot(bombEquipSFX);
        UIManager.Instance.UpdateBomb(currentLaserCount);
    }

    private int FindNextWeaponIndex()
    {
        int nextIndex = (currentWeaponIndex + 1) % weaponSlots.Length;
        while (nextIndex != currentWeaponIndex)
        {
            if (weaponSlots[nextIndex] != null) return nextIndex;
            nextIndex = (nextIndex + 1) % weaponSlots.Length;
        }
        return currentWeaponIndex; // �ٸ� ���Ⱑ ������ ���� �ε��� ��ȯ
    }


    private int FindEmptySlotIndex()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
            if (weaponSlots[i] == null) return i;

        return -1; // �� ������ ������ -1 ��ȯ
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

}
