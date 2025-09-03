using System;
using System.Reflection;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 관련")]
    [Range(0f, 1f)]
    [SerializeField] private float forwardAttackRange = 0.8f;
    [SerializeField] private Transform upFirePoint;
    [SerializeField] private Transform forwardFirePoint;
    [SerializeField] private Transform downFirePoint;

    [Header("무기 관련")]
    [SerializeField] private Transform weaponParent;
    [SerializeField] private int weaponSlotSize = 2;
    private Weapon[] weaponSlots = null;
    private int currentWeaponIndex = 0;

    [Header("폭탄 관련")]
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
        Debug.Log($"폭탄 사용! 남은 폭탄: {laserCount}");
    }

    private void SwitchWeapon()
    {
        if (weaponSlots.Length <= 1) return;
        if (FindNextWeaponIndex() == currentWeaponIndex) return; // 다른 무기가 없으면 종료

        if(CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);

        currentWeaponIndex = FindNextWeaponIndex();
        CurrentWeapon.gameObject.SetActive(true);

        animationController.SetAnimController(
            CurrentWeapon.WeaponData.upperAnimController,
            CurrentWeapon.WeaponData.lowerAnimController
        );

        Debug.Log($"무기 교체: {CurrentWeapon.WeaponData.weaponName}");
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        // 빈 슬롯 찾기
        int index = FindEmptySlotIndex();

        // 무기 생성 및 초기화
        GameObject weaponPrefab = Resources.Load<GameObject>($"Weapon/{weaponType}");
        Weapon newWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
        newWeapon.Initialize();

        if (weaponSlots[index] != null) // 기존 무기 제거
            Destroy(weaponSlots[index].gameObject);

        weaponSlots[index] = newWeapon;
        weaponSlots[index].gameObject.SetActive(false);
        Debug.Log($"무기 획득: {weaponSlots[index].WeaponData.weaponName}");
    }

    public void UnequipWeapon()
    {
        if (weaponSlots[currentWeaponIndex] == null) return;

        Debug.Log($"무기 해제: {weaponSlots[currentWeaponIndex].WeaponData.weaponName}");
        Destroy(weaponSlots[currentWeaponIndex].gameObject);
        weaponSlots[currentWeaponIndex] = null;

        // 다음 무기로 자동 전환
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
        return currentWeaponIndex; // 다른 무기가 없으면 현재 인덱스 반환
    }


    private int FindEmptySlotIndex()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
            if (weaponSlots[i] == null) return i;

        return weaponSlots.Length - 1; // 빈 슬롯이 없으면 마지막 인덱스 반환
    }

    private void UpdateDirectionY(float dirY)
    {
        float value = dirY > forwardAttackRange ? 1f :
                      dirY < -forwardAttackRange ? -1f : 0f;

        animationController.SetFloat_Upper("Y", value);
    }

}
