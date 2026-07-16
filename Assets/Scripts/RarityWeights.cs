using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityWeights", menuName = "RarityWeights", order = 0)]
public class RarityWeightsDefinition : SerializedScriptableObject
{
    [OdinSerialize] public Dictionary<Rarity, int> Weights;
}
