using System;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithView : MonoBehaviour
{
    [SerializeField] private List<BlacksmithWeaponItemView> weaponItemViews;
    [SerializeField] private List<BlacksmithBuffItemView> buffItemViews;

    private Blacksmith blacksmithInstance;

    public void Show()
    {
        this.gameObject.SetActive(true);

        UpdateWeaponInventoryViews();
        UpdateBuffInventoryViews();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void RegisterBlacksmithInstance(Blacksmith instance)
    {
        if (blacksmithInstance != null)
        {
            Debug.LogWarning("Instance already registed! Deregister the instance first.");
            return;
        }
        blacksmithInstance = instance;
        blacksmithInstance.OnBuffInventoryChanged += UpdateBuffInventoryViews;
    }

    public void DeregisterBlacksmithInstance()
    {
        if (blacksmithInstance != null)
        {
            blacksmithInstance.OnBuffInventoryChanged -= UpdateBuffInventoryViews;
        }
        blacksmithInstance = null;
        weaponItemViews.ForEach(weaponItemView => weaponItemView.DeregisterWeaponItem());
        buffItemViews.ForEach(buffItemView => buffItemView.DeregisterBuffItem());
    }

    private void UpdateBuffInventoryViews()
    {
        // Register buff items
        for (int i = 0; i < buffItemViews.Count; i++)
        {
            BlacksmithBuffItem buffItem = blacksmithInstance.AvailableBuffItems[i];
            BlacksmithBuffItemView buffItemView = buffItemViews[i];

            buffItemView.DeregisterBuffItem();

            if (buffItem == null)
            {
                continue;
            }
            buffItemView.RegisterBuffItem(buffItem, blacksmithInstance);
        }
    }

    private void UpdateWeaponInventoryViews()
    {
        // Register weapon items
        for (int i = 0; i < weaponItemViews.Count; i++)
        {
            weaponItemViews[i].RegisterWeaponItem(blacksmithInstance.AvailableWeaponItems[i], blacksmithInstance);

            BlacksmithWeaponItem weaponItem = blacksmithInstance.AvailableWeaponItems[i];
            BlacksmithWeaponItemView weaponItemView = weaponItemViews[i];

            weaponItemView.DeregisterWeaponItem();

            if (weaponItem == null)
            {
                continue;
            }
            weaponItemView.RegisterWeaponItem(weaponItem, blacksmithInstance);
        }
    }
}
