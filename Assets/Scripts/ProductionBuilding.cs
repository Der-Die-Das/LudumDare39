using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : Building
{
    public string Name;
    public int Price;
    public int ElectricityProduction;
    public int maintenanceCost;
    private bool _IsOn;
    public bool IsOn
    {
        get
        {
            return _IsOn;
        }
        set
        {
            if (value == true)
            {
                foreach (var item in stuffToTurnOff)
                {
                    item.SetActive(true);
                }
                StartCoroutine(BreakDown());
            }
            else
            {
                foreach (var item in stuffToTurnOff)
                {
                    item.SetActive(false);
                }
            }
            _IsOn = value;
        }
    }
    public int pollutionRadius = 1;
    public int pollutionStrength = 1;

    public int minTimeUntilBreakDown = 3;
    public int maxTimeUntilBreakDown = 10;
    public float chanceToNeedRepair = 0.5f;
    public GameObject[] stuffToTurnOff;

    [Header("Repair Icon")]
    public GameObject repairIconPrefab;
    private GameObject repairIcon;
    public float floatSpeed = 2f;
    public float floatStrength = 0.02f;
    public Vector3 offset;

    private AudioSource audioSource;


    protected override void Start()
    {
        base.Start();
        StartCoroutine(BreakDown());
        _IsOn = true;
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (repairIcon != null)
        {
            repairIcon.transform.position += Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        }
    }

    IEnumerator BreakDown()
    {
        float pollution = mapGenerator.getTileByPos(position).GetPolutionLevel();
        float timeUntilBreakDown = Random.Range(minTimeUntilBreakDown, maxTimeUntilBreakDown);
        if (pollution != 0)
        {
            timeUntilBreakDown *= (1f / pollution);
        }
        timeUntilBreakDown /= (GameManager.Instance.GameSpeed * 2f);
        Debug.Log("I will have a malfunction in " + timeUntilBreakDown + " Seconds (:");
        yield return new WaitForSeconds(timeUntilBreakDown);
        BuildingNeedsRepair();
    }
    public void Repair()
    {
        IsOn = true;
        Destroy(repairIcon);
        //maybe some repair cost
    }
    private void BuildingNeedsRepair()
    {
        if (Random.value <= chanceToNeedRepair)
        {
            IsOn = false;

            GameObject go = Instantiate(repairIconPrefab, transform, true);
            go.transform.position = transform.position + Vector3.up * 8f + offset;
            repairIcon = go;
            audioSource.Play();
        }
        else
        {
            Debug.Log("Lucky, no breakdown");
            StartCoroutine(BreakDown());
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (mapGenerator != null)
            Gizmos.DrawWireSphere(transform.position, pollutionRadius * mapGenerator.offsetBetweenTiles);
    }
}
