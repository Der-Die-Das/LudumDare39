using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public float offsetBetweenTiles = 1f;
    public float maxNoiseValue = 1f;
    public List<ColorToPrefab> colorMappings;
    public Texture2D mapImage;
    private List<Tile> allTiles = new List<Tile>();



    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapImage.width; x++)
        {
            for (int y = 0; y < mapImage.height; y++)
            {
                float randomNoise = Random.Range(0, maxNoiseValue);
                GameObject prefab = GetPrefab(x, y);
                if (prefab.name == "Tile_Water")
                {
                    randomNoise = 0;
                }
                Tile thisTile = Instantiate(prefab, new Vector3(x * offsetBetweenTiles, randomNoise, y * offsetBetweenTiles), Quaternion.identity, transform).GetComponent<Tile>();
                thisTile.position = new Vector2(x, y);
                allTiles.Add(thisTile);
            }
        }
    }

    private GameObject GetPrefab(int x, int y)
    {
        Color pixelColor = mapImage.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            // The pixel is transparrent. Let's ignore it!
            return null;
        }

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                return colorMapping.prefab;
            }
        }
        return null;
    }

    public Tile getTileByPos(Vector2 pos)
    {
        foreach (var item in allTiles)
        {
            if (item.position == pos)
            {
                return item;
            }
        }
        return null;
    }

    public Tile[] getAdjacentToPos(Vector2 pos)
    {
        Vector2[] offsets = new Vector2[] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
        List<Tile> tiles = new List<Tile>();
        Tile center = getTileByPos(pos);
        if (center == null)
        {
            return null;
        }
        foreach (var item in offsets)
        {
            Tile adjacent = getTileByPos(pos + item);
            if (adjacent != null)
            {
                tiles.Add(adjacent);
            }
        }
        return tiles.ToArray();
    }

}
