using Shootball.Utility;
using UnityEngine;

public class Houses : MonoBehaviour
{
    public GameObject[] HousePrefabs;
    public int[] Weights;

    void Start()
    {
        var mapBuilder = new MapBuilder(HousePrefabs, Weights);
        mapBuilder.Instantiate(new Vector2(-150, -150), new Vector2(150, 150), 0, 0.8f);
    }
}
