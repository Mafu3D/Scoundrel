using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Decks
{
    public interface IDeckDefinition<T> where T : IDeckStorable
    {
        public List<T> UnpackContents();
    }

    // [CreateAssetMenu(fileName="NewDeck", menuName="New Deck")]
    // public class DeckDefinition : ScriptableObject, IDeckDefinition<RuntimeCardModel>
    // {
    //     public SuitDeckDefinition monsterDeck1;
    //     public SuitDeckDefinition monsterDeck2;
    //     public SuitDeckDefinition potionsDeck;
    //     public SuitDeckDefinition weaponsDeck;

    //     public List<RuntimeCardModel> UnpackContents()
    //     {
    //         List<RuntimeCardModel> contents = monsterDeck1.UnpackContents();
    //         contents.AddRange(monsterDeck2.UnpackContents());
    //         contents.AddRange(monsterDeck2.UnpackContents());
    //         contents.AddRange(weaponsDeck.UnpackContents());
    //         return contents;
    //     }
    // }
}