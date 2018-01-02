using System.Collections.Generic;
using System.Linq;
using Shootball.Extensions;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model
{
    public class MapModel
    {
        private readonly ICollection<Vector3> _spawnPoints;
        private readonly IEnumerable<GameObjectBuilder> _houses;

        public MapModel(ICollection<Vector3> spawnPoints, IEnumerable<GameObjectBuilder> houses)
        {
            _spawnPoints = spawnPoints;
            _houses = houses;
        }

        public void Instantiate()
        {
            _houses.ForEach(h => h.Instantiate());            
        }

        public Vector3 RandomSpawnPoint()
        {
            return Extensions.Random.FromCollection(_spawnPoints);
        }
    }
}