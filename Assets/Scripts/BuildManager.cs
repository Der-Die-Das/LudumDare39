using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject[] buildingPrefabs;
    private int activeBuilding = 0;
    public List<ResidentalBuilding> residentalBuildings = new List<ResidentalBuilding>();
    public List<ProductionBuilding> productionBuilding = new List<ProductionBuilding>();
    private GameManager gameManager;
    private AudioSource source;
    public AudioClip onDestroy;
    public AudioClip onRepair;
    public AudioClip onBuild;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
        gameManager = GameManager.Instance;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Tile hitTile = hit.collider.GetComponentInParent<Tile>();
                if (hitTile != null && hitTile.tileType == Tile.TileType.Plain)
                {
                    if (activeBuilding >= 0)
                    {
                        if (hitTile.building == null)
                        {
                            int price = buildingPrefabs[activeBuilding].GetComponent<ProductionBuilding>().Price;
                            if (gameManager.Money < price)
                            {
                                return;
                            }

                            gameManager.Money -= price;
                            BuildBuilding(buildingPrefabs[activeBuilding], hitTile);
                            SetSound(onBuild);
                        }
                    }
                    else if (activeBuilding == -2)
                    {
                        if (hitTile.building != null && hitTile.building is ProductionBuilding)
                        {
                            DestroyBuilding(hitTile);
                            SetSound(onDestroy);
                        }
                    }
                    else if (activeBuilding == -1)
                    {
                        if (hitTile.building is ProductionBuilding && ((ProductionBuilding)hitTile.building).IsOn == false)
                        {
                            if (gameManager.Money >= ((ProductionBuilding)hitTile.building).Price * 0.02f)
                            {
                                gameManager.Money -= Mathf.RoundToInt(((ProductionBuilding)hitTile.building).Price * 0.02f);
                                SetSound(onRepair);
                                ((ProductionBuilding)hitTile.building).Repair();
                            }
                        }
                    }
                }
                else
                {
                    ProductionBuilding hitBuilding = hit.collider.GetComponentInParent<ProductionBuilding>();
                    if (hitBuilding != null && hitBuilding.IsOn == false)
                    {

                        hitBuilding.Repair();
                    }
                }
            }
        }

    }
    public bool BuildBuilding(GameObject prefab, Tile tile)
    {
        if (tile.tileType == Tile.TileType.Plain)
        {
            GameObject building = Instantiate(prefab, tile.transform, true);

            building.transform.position = tile.transform.position;

            Building buildingComp = building.GetComponent<Building>();

            buildingComp.position = tile.position;

            if (tile.building != null)
            {
                DestroyBuilding(tile);
            }

            tile.building = buildingComp;

            if (buildingComp is ResidentalBuilding)
            {
                residentalBuildings.Add((ResidentalBuilding)buildingComp);
                ((ResidentalBuilding)buildingComp).mapping = gameManager.GetMappingByPrefab(prefab);
            }
            else
                productionBuilding.Add((ProductionBuilding)buildingComp);

            return true;
        }
        return false;
    }

    public bool DestroyBuilding(Tile tile)
    {
        if (tile.building != null)
        {

            if (tile.building is ResidentalBuilding)
                residentalBuildings.Remove((ResidentalBuilding)tile.building);
            else
                productionBuilding.Remove((ProductionBuilding)tile.building);

            Destroy(tile.building.gameObject);
            return true;
        }
        return false;
    }

    public void SelectBuildingToPlace(int index)
    {
        activeBuilding = index;
    }

    private void SetSound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
