using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{
    public enum TileType
    {
        Plain, Mountain, Forest, Water
    }
    public TileType tileType;

    [HideInInspector]
    public Vector2 position;

    [HideInInspector]
    public Building building;

    private BuildManager buildManager;
    private MapGenerator mapGenerator;
    void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    public bool isHappy()
    {
        if (buildManager.productionBuilding.Count == 0 && buildManager != null)
            return true;
        return !buildManager.productionBuilding.Any(x => Vector3.Distance(transform.position, x.transform.position) <= Mathf.FloorToInt(x.pollutionRadius * mapGenerator.offsetBetweenTiles));
    }

    public float GetPolutionLevel()
    {
        float polutionLevel = 0;
        foreach (var item in buildManager.productionBuilding)
        {
            if (building != null && building == item)
            {
                continue;
            }
            float tempPolutionLevel = (1f / item.pollutionRadius * (item.pollutionRadius - Vector2.Distance(item.position, position))) * item.pollutionStrength;
            if (tempPolutionLevel > 0)
            {
                polutionLevel += tempPolutionLevel;
            }
        }
        return polutionLevel;
    }

}
