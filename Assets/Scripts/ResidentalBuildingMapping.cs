using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResidentalBuildingMapping
{
    public enum BuildingLevel
    {
        Tier1, Tier2, Tier3, Tier4, Tier5, Tier6
    }

    public GameObject prefab;

    public BuildingLevel buildingLevel;

    public int roundsForUpgrade = 5;

    public float maxPollutionForUpgrade = 3f;

    public int GetRoundsForUpgrade(float pollution)
    {
        if (pollution > maxPollutionForUpgrade)
        {
            return -1;
        }

        return Mathf.RoundToInt(roundsForUpgrade * (1 + 1f / maxPollutionForUpgrade * pollution));

    }
}
