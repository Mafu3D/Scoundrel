using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

public abstract class BuffDefinition : ScriptableObject
{
    [Header("Buff Meta")]
    [SerializeField] public string Name;
    [SerializeField] public string Description;

    [Header("Base Buff Parameters")]
    [SerializeField] public bool RemoveOnDeath;
    [SerializeField] public bool RemoveOnRun;
    [SerializeField] public List<BuffDefinition> childBuffs = new();

    protected GameManager gameManager;
    protected CardModel owner;

    public BuffDefinition GetChildBuffByName(string name)
    {
        foreach (BuffDefinition buff in childBuffs)
        {
            if (buff.Name == name)
            {
                return buff;
            }
        }
        return default;
    }

    public abstract void OnBuffInitialized(GameManager gameManager, CardModel owner);
    public abstract void OnBuffApplied(GameManager gameManager, CardModel owner);
    public abstract void OnBuffRemoved(GameManager gameManager, CardModel owner);
    public abstract void OnDraw(GameManager gameManager, CardModel owner);
    public abstract void OnOpenNewRoom(GameManager gameManager, CardModel owner);
    public abstract void OnEnterNewRoom(GameManager gameManager, CardModel owner);
    public abstract void OnRun(GameManager gameManager, CardModel owner);
    public abstract void OnMonsterDie(GameManager gameManager, CardModel owner);
    public abstract void OnEquipWeapon(GameManager gameManager, CardModel owner);
    public abstract void OnDrinkPotion(GameManager gameManager, CardModel owner);
    public abstract void OnDiscardPotion(GameManager gameManager, CardModel owner);
    public abstract void OnAttack(GameManager gameManager, CardModel owner);
    public abstract void OnCardRemoval(GameManager gameManager, CardModel owner);
    public abstract void OnUpdate(GameManager gameManager, CardModel owner);
}
