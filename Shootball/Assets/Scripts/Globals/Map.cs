using Shootball.Model;
using Shootball.Utility;
using UnityEngine;

public class Map : MonoBehaviour
{
    public float Width;
    public float Length;
    public float BaseHeight;
    public float FillingRate;
    public float SpawnPointWidth;
    public int SpawnPoints;
    public GameObject[] HousePrefabs;
    public int[] Weights;

    [HideInInspector]
    public MapModel MapModel;

    void Start()
    {
        var mapBuilder = new MapBuilder(HousePrefabs, Weights, SpawnPointWidth);
        var halfDimensions = new Vector2(Width / 2, Length / 2);
        MapModel = mapBuilder.Generate(-halfDimensions, halfDimensions, BaseHeight, FillingRate, SpawnPoints);
    }
}
