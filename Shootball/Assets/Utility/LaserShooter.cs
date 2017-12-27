using System.Collections;
using UnityEngine;

namespace Shootball.Utility
{
    public class LaserShooter
    {
        private readonly MonoBehaviour _monoBehaviour;

        public LaserShooter(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        public void Shoot(LineRenderer line, Vector3 origin, Vector3 direction, float speed)
        {
            Vector3 hitPoint;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit))
            {
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = origin + direction * 50;
            }
            _monoBehaviour.StartCoroutine(ShotEffect(line, origin, hitPoint, speed));
        }

        private IEnumerator ShotEffect(LineRenderer lineRenderer, Vector3 start, Vector3 end, float speed)
        {
            var line = UnityEngine.Object.Instantiate(lineRenderer) as LineRenderer;
            line.useWorldSpace = true;

            var startingPoint = start;
            var direction = (end - start).normalized;
            var maxDistance = Vector3.Distance(start, end);

            line.enabled = true;

            while (true)
            {
                var endingPoint = startingPoint + direction * 2;
                line.SetPosition(0, startingPoint);
                line.SetPosition(1, endingPoint);

                yield return null;

                startingPoint += direction * speed * Time.deltaTime;

                if (maxDistance <= Vector3.Distance(startingPoint, start))
                {
                    break;
                }
            }

            line.enabled = false;
            UnityEngine.Object.Destroy(line.gameObject);
            yield return null;
        }
    }
}