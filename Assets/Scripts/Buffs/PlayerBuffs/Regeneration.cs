using System;
using System.Collections.Generic;
using Mafu.UnityServiceLocator;
using Project.Decks;
using UnityEngine;

[CreateAssetMenu(fileName="Regeneration", menuName="Buffs/Player/Regeneration")]
public class Regeneration : PlayerBuff
{
    [SerializeField] public int Amount = 0;

    public override void OnGoToNewRoom()
    {
        if (Amount > 0)
        {
            gameManager.Player.Heal(Amount);
            Debug.Log($"Regeneration buff applied: {Amount} health restored to player.");
        }
    }
}
