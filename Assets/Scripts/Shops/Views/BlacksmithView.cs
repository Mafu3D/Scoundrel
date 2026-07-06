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
        // Register weapon items
        for (int i = 0; i < weaponItemViews.Count; i++)
        {
            weaponItemViews[i].RegisterWeaponItem(blacksmithInstance.AvailableWeaponItems[i], blacksmithInstance);
        }

        // Register buff items
        for (int i = 0; i < buffItemViews.Count; i++)
        {
            buffItemViews[i].RegisterBuffItem(blacksmithInstance.AvailableBuffItems[i], blacksmithInstance);
        }
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

    }

    public void DeregisterBlacksmithInstance()
    {
        blacksmithInstance = null;
        weaponItemViews.ForEach(weaponItemView => weaponItemView.DeregisterWeaponItem());
        buffItemViews.ForEach(buffItemView => buffItemView.DeregisterBuffItem());
    }
}
