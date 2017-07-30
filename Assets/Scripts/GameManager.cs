using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            return _Instance;
        }
        private set
        {
            if (_Instance != null)
            {
                Destroy(value);
            }
            else
            {
                _Instance = value;
                DontDestroyOnLoad(value);
            }
        }
    }


    public float iterationLengthSeconds = 5f;
    private int happySinceXIterations = 0;
    private int sadSinceXIterations = 0;
    public float percentageHappytoGrow = 0.75f;

    public ResidentalBuildingMapping[] buildingMappings;

    private MapGenerator mapGenerator;
    private BuildManager buildManager;
    public int GameSpeed = 1;

    public int Money { get; set; }

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(GameCycle());
        mapGenerator = FindObjectOfType<MapGenerator>();
        buildManager = FindObjectOfType<BuildManager>();

        buildManager.BuildBuilding(GetPrefabOfLevel(ResidentalBuildingMapping.BuildingLevel.Tier1), mapGenerator.getTileByPos(new Vector2(1, 1)));
        Money = 20000;

        Tile tile1 = mapGenerator.getTileByPos(new Vector2(1, 1));
        Tile tile2 = mapGenerator.getTileByPos(new Vector2(2, 1));
        Debug.Log("Distance " + Vector3.Distance(tile1.transform.position, tile2.transform.position));


    }

    IEnumerator GameCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(iterationLengthSeconds/(GameSpeed*2f));
            Money -= buildManager.productionBuilding.Where(x => x.IsOn).Sum(x => x.maintenanceCost);
            CalculateHappyness();

            if (buildManager.residentalBuildings.Count(x => x.mapping.buildingLevel == ResidentalBuildingMapping.BuildingLevel.Tier6) >= 4)
            {
                FindObjectOfType<SceneChanger>().LoadEndScene();
            }

        }
    }

    private void CalculateHappyness()
    {
        int totalElectricity = GetAvailableEnergy();

        buildManager.residentalBuildings.ForEach(x => x.hasEnoughEnergy = false);
        List<ResidentalBuilding> payableResidents = buildManager.residentalBuildings.Where(x => x.HasEnoughEnergy(ref totalElectricity)).ToList();
        payableResidents.ForEach(x => { x.hasEnoughEnergy = true; x.OnRoundElapsed(); });

        Money += payableResidents.Sum(x => x.Income);

        if (buildManager.residentalBuildings.Count * percentageHappytoGrow <= payableResidents.Count)
        {
            happySinceXIterations++;
            sadSinceXIterations = 0;
        }
        else
        {
            happySinceXIterations = 0;
            sadSinceXIterations++;
        }

        if (happySinceXIterations > 0 && happySinceXIterations % 5 == 0)
        {
            EnlargeCity(totalElectricity);
        }
        if (sadSinceXIterations > 0 && sadSinceXIterations % 5 == 0)
        {
            ShrinkCity();
        }
    }

    private void EnlargeCity(int leftElectricity)
    {
        if (buildManager.residentalBuildings == null || buildManager.residentalBuildings.Count == 0)
        {
            return;
        }
        if (leftElectricity -GetPrefabOfLevel(ResidentalBuildingMapping.BuildingLevel.Tier1).GetComponent<ResidentalBuilding>().ElectricityNeeded >= 0)
        {
            List<Tile> allFreeAdjacentTiles = new List<Tile>();
            foreach (var item in buildManager.residentalBuildings)
            {
                allFreeAdjacentTiles.AddRange(mapGenerator.getAdjacentToPos(item.position).ToList().FindAll(y => y.building == null && y.tileType == Tile.TileType.Plain));
            }
            allFreeAdjacentTiles = allFreeAdjacentTiles.OrderBy(x => x.GetPolutionLevel()).ToList();
            foreach (var item in allFreeAdjacentTiles)
            {
                if (buildManager.BuildBuilding(GetPrefabOfLevel(ResidentalBuildingMapping.BuildingLevel.Tier1), item))
                {
                    return;
                }
            }
        }

    }
    private void ShrinkCity()
    {
        if (buildManager.residentalBuildings == null || buildManager.residentalBuildings.Count <= 1)
        {
            return;
        }
        buildManager.DestroyBuilding(mapGenerator.getTileByPos(buildManager.residentalBuildings.OrderBy(x => mapGenerator.getTileByPos(x.position).GetPolutionLevel()).First().position));
    }
    public int GetPopulation()
    {
        return buildManager.residentalBuildings.Sum(x => x.ElectricityNeeded);
    }

    public int GetRequestedEnergy()
    {
        return buildManager.residentalBuildings.Sum(x => x.ElectricityNeeded);
    }
    public int GetAvailableEnergy()
    {
        return buildManager.productionBuilding.Where(x => x.IsOn).Sum(x => x.ElectricityProduction);
    }

    public GameObject GetPrefabOfLevel(ResidentalBuildingMapping.BuildingLevel level)
    {
        foreach (var item in buildingMappings)
        {
            if (item.buildingLevel == level)
            {
                return item.prefab;
            }

        }
        return null;
    }

    public ResidentalBuildingMapping GetMappingByPrefab(GameObject prefab)
    {
        foreach (var item in buildingMappings)
        {
            if (item.prefab.name == prefab.name)
            {
                return item;
            }
        }
        return null;
    }



}

