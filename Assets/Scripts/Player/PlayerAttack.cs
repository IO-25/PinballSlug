using System;
using System.Reflection;
using UnityEngine;

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
    // [SerializeField] private Transform laserPoint;
    [SerializeField] private int laserCount = 10;
    private int currentLaserCount = 10;

    private PlayerAnimationController animationController;

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
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(true);

        Initialize();
    }

    void OnDisable()
    {
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);
        animationController.SetBool("IsShooting", false);
    }

    void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();
        weaponSlots = new Weapon[weaponSlotSize];
    }

    void Update()
    {
        HandleLook();
        HandleInput();
    }

    private void Initialize()
    {
        if(UIManager.Instance.weaponUI)
            UIManager.Instance.weaponUI.Initialize();
        currentWeaponIndex = 0;
        currentLaserCount = laserCount;

        if(weaponSlots[0] == null)
            EquipWeapon(WeaponType.Pistol);

        for (int i = 1; i < weaponSlots.Length; i++)
            UnequipWeapon(i);

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
            UnequipWeapon(currentWeaponIndex);

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
        // laser.ShotLaser(laserPoint.position, dir);
        UIManager.Instance.UpdateBomb(currentLaserCount);
    }

    private void SwitchWeapon()
    {
        if (weaponSlots.Length <= 1) return;
        if (FindNextWeaponIndex() == currentWeaponIndex) return; // �ٸ� ���Ⱑ ������ ����

        if(CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);

        currentWeaponIndex = FindNextWeaponIndex();
        CurrentWeapon.gameObject.SetActive(true);

        animationController.SetAnimController(
            CurrentWeapon.WeaponData.upperAnimController,
            CurrentWeapon.WeaponData.lowerAnimController
        );

        UIManager.Instance.SelectWeaponSlot(currentWeaponIndex);
        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
        Debug.Log($"���� ��ü: {CurrentWeapon.WeaponData.weaponName}");
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        int index = currentWeaponIndex;

        if (index == 0 && weaponSlots[0])
        {
            if (FindEmptySlotIndex() == -1) return;
            else index = FindEmptySlotIndex();
        }

        Debug.Log($"EquipWeapon: {weaponType} at slot {index}");

        // ���� ���� �� �ʱ�ȭ
        GameObject weaponPrefab = Resources.Load<GameObject>($"Weapon/{weaponType}");
        Weapon newWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
        newWeapon.Initialize();

        if (weaponSlots[index] != null) // ���� ���� ����
            Destroy(weaponSlots[index].gameObject);

        weaponSlots[index] = newWeapon;
        weaponSlots[index].gameObject.SetActive(false);
        Debug.Log(weaponSlots[index].WeaponData.weaponIcon);
        UIManager.Instance.SetWeaponSlotSprite(GetWeaponIcon(index), index);
        UIManager.Instance.UpdateAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
        Debug.Log($"���� ȹ��: {weaponSlots[index].WeaponData.weaponName}");

        /*
        int index = FindEmptySlotIndex();

        // ���� ���� �� �ʱ�ȭ
        GameObject weaponPrefab = Resources.Load<GameObject>($"Weapon/{weaponType}");
        Weapon newWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
        newWeapon.Initialize();

        if (weaponSlots[index] != null) // ���� ���� ����
            Destroy(weaponSlots[index].gameObject);

        weaponSlots[index] = newWeapon;
        weaponSlots[index].gameObject.SetActive(false);
        Debug.Log(weaponSlots[index].WeaponData.weaponIcon);
        MapGameManager.Instance.SetWeaponSlotSprite(GetWeaponIcon(index), index);
        MapGameManager.Instance.DisplayAmmo(CurrentWeapon.CurrentAmmo, CurrentWeapon.WeaponData.useAmmo);
        Debug.Log($"���� ȹ��: {weaponSlots[index].WeaponData.weaponName}");
        */
    }

    public void UnequipWeapon(int index)
    {
        if (weaponSlots[index] == null) return;

        Debug.Log($"���� ����: {weaponSlots[index].WeaponData.weaponName}");
        Destroy(weaponSlots[index].gameObject);
        weaponSlots[index] = null;
        UIManager.Instance.SetWeaponSlotSprite(GetWeaponIcon(index), index);

        // ���� ����� �ڵ� ��ȯ
        SwitchWeapon();
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
