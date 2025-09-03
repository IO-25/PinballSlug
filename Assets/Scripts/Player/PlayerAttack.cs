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
    [SerializeField] private Transform laserPoint;
    [SerializeField] private int laserCount = 10;

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
    }

    void OnDisable()
    {
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);
        animationController.SetBool("IsShooting", false);
    }

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        HandleLook();
        HandleInput();
    }

    private void Initialize()
    {
        currentWeaponIndex = 0;
        weaponSlots = new Weapon[weaponSlotSize];
        animationController = GetComponent<PlayerAnimationController>();
        EquipWeapon(WeaponType.Pistol);
        EquipWeapon(WeaponType.Shotgun);
        CurrentWeapon.gameObject.SetActive(true);
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
            UnequipWeapon();
    }

    public void UseBomb()
    {
        if (laserCount <= 0) return;
        laserCount--;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - CurrentFirePoint).normalized;

        Laser laser = Instantiate(laserPrefab);
        laser.ShotLaser(laserPoint.position, dir);
        Debug.Log($"��ź ���! ���� ��ź: {laserCount}");
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

        Debug.Log($"���� ��ü: {CurrentWeapon.WeaponData.weaponName}");
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        // �� ���� ã��
        int index = FindEmptySlotIndex();

        // ���� ���� �� �ʱ�ȭ
        GameObject weaponPrefab = Resources.Load<GameObject>($"Weapon/{weaponType}");
        Weapon newWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
        newWeapon.Initialize();

        if (weaponSlots[index] != null) // ���� ���� ����
            Destroy(weaponSlots[index].gameObject);

        weaponSlots[index] = newWeapon;
        weaponSlots[index].gameObject.SetActive(false);
        Debug.Log($"���� ȹ��: {weaponSlots[index].WeaponData.weaponName}");
    }

    public void UnequipWeapon()
    {
        if (weaponSlots[currentWeaponIndex] == null) return;

        Debug.Log($"���� ����: {weaponSlots[currentWeaponIndex].WeaponData.weaponName}");
        Destroy(weaponSlots[currentWeaponIndex].gameObject);
        weaponSlots[currentWeaponIndex] = null;

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

        return weaponSlots.Length - 1; // �� ������ ������ ������ �ε��� ��ȯ
    }

    private void UpdateDirectionY(float dirY)
    {
        float value = dirY > forwardAttackRange ? 1f :
                      dirY < -forwardAttackRange ? -1f : 0f;

        animationController.SetFloat_Upper("Y", value);
    }

}
