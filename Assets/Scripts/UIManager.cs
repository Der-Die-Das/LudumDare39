using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    private BuildManager buildManager;

    [Header("TopBar")]
    public Text money;
    public Text population;
    public Text neededEnergy;
    public Text availableEnergy;
    public Image speed;

    [Header("Toolbar")]
    private bool toolbarIn = true;
    private float toolbarStandartPos;
    public RectTransform toolbar;
    public float toolBarSpeed = 100f;
    public Image[] borders;
    public Color highlightedColor;
    private Color defaultColor;
    private HoverUI[] hoverElements;
    [Header("InfoDialog")]
    public GameObject InfoDialog;
    public Vector3 infoDialogOffset;
    public Text buildingName;
    public Text Price;
    public Text elecProduction;
    public Text pollutionRadius;
    public Text breakdownTime;
    public Text maintenanceCost;



    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.Instance;
        buildManager = GetComponent<BuildManager>();
        toolbarStandartPos = toolbar.position.x;
        defaultColor = borders[0].color;
        SelectBuildingToPlace(0);

        hoverElements = new HoverUI[borders.Length - 2];
        for (int i = 0; i < hoverElements.Length; i++)
        {
            hoverElements[i] = borders[i + 2].GetComponentInParent<HoverUI>();
        }
        InvokeRepeating("UpdateTopBar", 0, 0.5f);
        SetGameSpeedTo(1);
    }

    void UpdateTopBar()
    {
        money.text = gameManager.Money + " $";
        population.text = gameManager.GetPopulation().ToString();
        neededEnergy.text = gameManager.GetRequestedEnergy().ToString();
        availableEnergy.text = gameManager.GetAvailableEnergy().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //hover over icons
        for (int i = 0; i < hoverElements.Length; i++)
        {
            if (hoverElements[i].isPointerOverIt)
            {
                InfoDialog.SetActive(true);
                InfoDialog.transform.position = hoverElements[i].transform.position + infoDialogOffset;

                //update it with data
                ProductionBuilding building = buildManager.buildingPrefabs[i].GetComponent<ProductionBuilding>(); //may cause lags
                buildingName.text = building.Name;
                Price.text = building.Price + " $";
                elecProduction.text = building.ElectricityProduction.ToString();
                pollutionRadius.text = building.pollutionRadius.ToString();
                breakdownTime.text = ((Mathf.Lerp(building.minTimeUntilBreakDown, building.maxTimeUntilBreakDown, 0.5f) / 60f) / 2f).ToString("0.00") + " Min.";
                maintenanceCost.text = building.maintenanceCost + " $";
                return;
            }
        }
        InfoDialog.SetActive(false);


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetGameSpeedTo(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetGameSpeedTo(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetGameSpeedTo(3);
        }
    }

    public void ToolbarClicked()
    {
        StopAllCoroutines();
        if (toolbarIn)
        {
            StartCoroutine(MoveOutToolBar());
        }
        else
        {
            StartCoroutine(MoveInToolBar());
        }
        toolbarIn = !toolbarIn;
    }

    IEnumerator MoveInToolBar()
    {
        while (toolbar.position.x < toolbarStandartPos)
        {
            toolbar.position += Vector3.right * toolBarSpeed * 100f * Time.deltaTime;
            yield return 0;
        }
    }
    IEnumerator MoveOutToolBar()
    {
        while (toolbar.position.x > Screen.width)
        {
            toolbar.position -= Vector3.right * toolBarSpeed * 100f * Time.deltaTime;
            yield return 0;
        }
    }

    public void SelectBuildingToPlace(int index)
    {
        buildManager.SelectBuildingToPlace(index);
        for (int i = 0; i < borders.Length; i++)
        {
            if (i == index + 2)
            {
                borders[i].color = highlightedColor;
            }
            else
            {
                borders[i].color = defaultColor;
            }
        }
    }

    private void SetGameSpeedTo(int speed)
    {
        gameManager.GameSpeed = speed;
        this.speed.fillAmount = (1f / 3f * speed);
    }

}
