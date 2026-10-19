using System;
using TMPro;
using UnityEngine;

public class WeaponHUD : MonoBehaviour
{
    public static event Action<string> OnWeaponDisplayRequested;

    [SerializeField] private TMP_Text weaponText;

    private void Awake()
    {
        if (weaponText == null)
            weaponText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        OnWeaponDisplayRequested += UpdateWeaponText;
    }

    private void OnDisable()
    {
        OnWeaponDisplayRequested -= UpdateWeaponText;
    }

    private void Start()
    {
        UpdateWeaponText("Survivor Sword");
    }

    private void UpdateWeaponText(string weaponName)
    {
        if (weaponText != null)
            weaponText.text = "Weapon: " + weaponName;
    }
}
