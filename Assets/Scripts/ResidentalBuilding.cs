using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResidentalBuilding : Building
{
    public ResidentalBuildingMapping mapping;
    public int Income = 20;
    public int ElectricityNeeded = 10;
    [HideInInspector]
    public int enoughEnergySinceXRounds = 0;
    [HideInInspector]
    public bool hasEnoughEnergy = false;



    public void OnRoundElapsed()
    {
        if (hasEnoughEnergy)
        {
            if (mapGenerator.getAdjacentToPos(position).Count(x =>
            {
                return (x.building != null && x.building is ResidentalBuilding && ((ResidentalBuilding)x.building).mapping.buildingLevel >= mapping.buildingLevel);
            }
                  ) >= 4)
            {
                enoughEnergySinceXRounds++;
            }
        }
        else
        {
            enoughEnergySinceXRounds = 0;
        }
        if (mapping.buildingLevel < ResidentalBuildingMapping.BuildingLevel.Tier6 && enoughEnergySinceXRounds > mapping.roundsForUpgrade && GameManager.Instance.GetPrefabOfLevel(mapping.buildingLevel).GetComponent<ResidentalBuilding>().ElectricityNeeded + GameManager.Instance.GetRequestedEnergy() <= GameManager.Instance.GetAvailableEnergy())
        {
            int roundsForUpgrade = mapping.GetRoundsForUpgrade(GetPolution());
            if (roundsForUpgrade <= enoughEnergySinceXRounds && roundsForUpgrade > 0)
            {

                Upgrade();
            }
        }

    }

    private float GetPolution()
    {
        return mapGenerator.getTileByPos(position).GetPolutionLevel();
    }

    private void Upgrade()
    {
        buildManager.BuildBuilding(GameManager.Instance.GetPrefabOfLevel(mapping.buildingLevel + 1), mapGenerator.getTileByPos(position));
    }

    public bool HasEnoughEnergy(ref int electricityLeft)
    {
        electricityLeft -= ElectricityNeeded;
        return electricityLeft >= 0;
    }

}
