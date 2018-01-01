using System.Collections.Generic;
using System.Linq;
using Shootball.Utility;
using UnityEngine;

public class Houses : MonoBehaviour
{
    public GameObject[] HousePrefabs;

    void Start()
    {
        var mapBuilder = new MapBuilder(HousePrefabs);
        mapBuilder.Instantiate(new Vector2(-150, -150), new Vector2(150, 150), 0, 0.7f);
    }
}
