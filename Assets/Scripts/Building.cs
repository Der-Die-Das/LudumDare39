using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {

    [HideInInspector]
    public Vector2 position;

    protected MapGenerator mapGenerator;
    protected BuildManager buildManager;

    protected virtual void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        buildManager = FindObjectOfType<BuildManager>();
    }

}
