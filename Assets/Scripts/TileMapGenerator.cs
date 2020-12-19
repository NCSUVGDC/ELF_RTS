using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to generate a random tile map given input prefabs
/// Code referenced from: https://www.youtube.com/watch?v=1qxytunVuqQ
/// </summary>
public class TileMapGenerator : MonoBehaviour
{

    public GameObject[] tilePrefabs;
    public GameObject container;

    public int rows;
    public int cols;

    public int numberOfSeeds;

    List<Vector3> seeds = new List<Vector3>();
    List<int> tilePrefabIndex = new List<int>();

    // Start is called before the first frame update
    public void GameStart()
    {
        Debug.Log("game started");
        CreateRandomPoints();
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {

                Vector3 point = new Vector3(i, j, 0);
                if (!seeds.Contains(point))
                {
                    int closestPointIndex = FindClosestPoint(point);

                    GameObject tile = Instantiate(tilePrefabs[tilePrefabIndex[closestPointIndex]], point, Quaternion.identity);
                    tile.transform.parent = container.transform;
                }

                
            }
        }
    }

    private void CreateRandomPoints()
    {
        for (int i = 0; i < numberOfSeeds; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0, rows), Random.Range(0, cols), -1);
            seeds.Add(randomPosition);

            int RandomTileNumber = Random.Range(0, tilePrefabs.Length);
            tilePrefabIndex.Add(RandomTileNumber);

            GameObject tile = Instantiate(tilePrefabs[RandomTileNumber], randomPosition, Quaternion.identity);
            tile.transform.parent = container.transform;
        }
    }

    private int FindClosestPoint(Vector3 point)
    {
        int closestPointIndex = 0;
        var distance = Vector3.Distance(point, seeds[0]);

        for (int i = 0; i < seeds.Count; i++)
        {
            var tempDistance = Vector3.Distance(point, seeds[i]);
            if (tempDistance < distance)
            {
                distance = tempDistance;
                closestPointIndex = i;
            }
        }

        return closestPointIndex;
    }
}
