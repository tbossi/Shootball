using Shootball.Model;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.GlobalScripts
{
    public class Map : MonoBehaviour
    {
        public float Width;
        public float Length;
        public float BaseHeight;
        public float FillingRate;
        public float SpawnPointWidth;
        public int SpawnPoints;
        public GameObject SpawnPointPrefab;
        public GameObject GroundPrefab;
        public GameObject[] HousePrefabs;
        public int[] Weights;

        [HideInInspector]
        public MapModel MapModel;

        void OnEnable()
        {
            var mapBuilder = new MapBuilder(HousePrefabs, Weights, SpawnPointPrefab, SpawnPointWidth, GroundPrefab);
            var halfDimensions = new Vector2(Width / 2, Length / 2);
            MapModel = mapBuilder.Generate(-halfDimensions, halfDimensions, BaseHeight, FillingRate, SpawnPoints);
        }
    }
}