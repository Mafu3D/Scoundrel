// using System;
// using Mafu.UnityServiceLocator;
// using System.Collections.Generic;
// using Project.Decks;
// using UnityEngine;

// public abstract class WeaponBuff : ScriptableObject
// {
//     [Header("Buff Meta")]
//     [SerializeField] public string Name;
//     [SerializeField] public string Description;

//     [Header("Base Buff Parameters")]
//     [SerializeField] public bool ApplyOnDraw = true;
//     [SerializeField] public bool IsTemporary = false;
//     [SerializeField] public bool RemoveOnParentCleanup = false;
//     [SerializeField] public bool RemoveOnDeath = true;
//     [SerializeField] public List<WeaponBuff> registeredChildBuffs = new();

//     public List<WeaponBuff> ChildBuffInstances { get; private set; } = new();

//     public BuffID ID;
//     public WeaponModel Owner { get; private set; }

//     protected GameManager gameManager;

//     public override string ToString() => $"{Name}({ID})";

//     public WeaponBuff GetRegisteredChildBuffByName(string name)
//     {
//         foreach (WeaponBuff buff in registeredChildBuffs)
//         {
//             if (buff.Name == name)
//             {
//                 return buff;
//             }
//         }
//         throw new ArgumentException($"{name} is not a registered child buff of {this.name}", "name");
//     }

//     public WeaponBuff GetChildBuffInstanceByName(string name)
//     {
//         foreach (WeaponBuff buff in ChildBuffInstances)
//         {
//             if (buff.Name == name)
//             {
//                 return buff;
//             }
//         }
//         throw new ArgumentException($"There is no instance of {name} registered to {this.name}", "name");
//     }

//     public void Initialize(WeaponBuff owner)
//     {
//         this.Owner = owner;
//         ServiceLocator.Global.Get(out gameManager);
//         ID = new(this);
//         OnBuffInitialized();
//     }

//     public void Remove()
//     {
//         Owner.RemoveBuff(this);
//     }

//     private void OnDestroy()
//     {
//         // Cleanup();
//         Debug.Log("destroyed");
//     }

//     public void Cleanup()
//     {
//         // foreach (Buff buff in childBuffInstances)
//         // {
//         //     if (!buff.CleanupWithParent)
//         //     {
//         //         continue;
//         //     }
//         //     buff.Cleanup();
//         // }
//         OnCleanup();
//     }

//     public void TriggerEffect(BuffTrigger trigger)
//     {
//         GetTriggerCallable(trigger)();
//     }

//     private Action GetTriggerCallable(BuffTrigger trigger)
//     {
//         return trigger switch
//         {
//             BuffTrigger.OnBuffApplied => OnBuffApplied,
//             BuffTrigger.OnDraw => OnDraw,
//             BuffTrigger.OnEnterRoom => OnEnterRoom,
//             BuffTrigger.OnRun => OnRun,
//             BuffTrigger.OnOtherDie => OnOtherDie,
//             BuffTrigger.OnSelfDie => OnSelfDie,
//             BuffTrigger.OnEquipWeapon => OnEquipWeapon,
//             BuffTrigger.OnDrinkPotion => OnDrinkPotion,
//             BuffTrigger.OnDiscardPotion => OnDiscardPotion,
//             BuffTrigger.OnAttack => OnAttack,
//             BuffTrigger.OnUpdate => OnAttack,
//             _ => throw new ArgumentOutOfRangeException(nameof(trigger), $"{nameof(trigger)} has not been registered as a callable trigger.")
//         };
//     }

//     protected CardBuff AddBuff(CardModel target, CardBuff buff)
//     {
//         CardBuff newInstance = target.AddNewBuff(buff);
//         ChildBuffInstances.Add(newInstance);
//         return newInstance;
//     }

//     /// <summary>
//     /// Called at initialization of the buff by the Buff Manager when adding a new buff.
//     /// This is for any extra initialization that needs to be
//     /// done for the construction of this buff.
//     /// </summary>
//     protected abstract void OnBuffInitialized();

//     /// <summary>
//     /// Called whenever the buff is removed, no matter the source.
//     /// This is for logic that needs to run if the buff ever becomes
//     /// no longe active. Like state changes.
//     /// </summary>
//     protected abstract void OnCleanup();

//     /// <summary>
//     /// Called when the buff is considered active, after initialization.
//     /// </summary>
//     protected abstract void OnBuffApplied();

//     /// <summary>
//     /// Called when the card is drawn, before the room has been fully created.
//     /// Use this for logic that affects how cards are drawn?
//     /// </summary>
//     protected abstract void OnDraw();

//     /// <summary>
//     /// Called when the card leaves the table for any reason.
//     /// </summary>
//     protected abstract void OnLeave();

//     /// <summary>
//     /// Called every update cycle of the Game Manager update loop.
//     /// </summary>
//     protected abstract void OnUpdate();

//     protected abstract void OnEnterRoom();
//     protected abstract void OnRun();
//     protected abstract void OnSelfDie();
//     protected abstract void OnOtherDie();
//     protected abstract void OnEquipWeapon();
//     protected abstract void OnDrinkPotion();
//     protected abstract void OnDiscardPotion();
//     protected abstract void OnAttack();
// }