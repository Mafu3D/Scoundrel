// using System;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Project.Decks
// {
//     [CreateAssetMenu(fileName="NewSuitDeck", menuName="New Suit Deck")]
//     public class SuitDeckDefinition : ScriptableObject, IDeckDefinition<RuntimeCardModel>
//     {
//         public Suit Suit;
//         public List<CardDefinition> Cards;

//         public List<RuntimeCardModel> UnpackContents()
//         {
//             List<RuntimeCardModel> contents = new();
//             foreach (CardDefinition card in Cards)
//             {
//                 contents.Add(new RuntimeCardModel(card.Suit, card.Value, card.Buffs));
//             }
//             return contents;
//         }
//     }
// }