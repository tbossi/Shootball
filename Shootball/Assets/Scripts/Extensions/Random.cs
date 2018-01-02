using System.Collections.Generic;
using System.Linq;

namespace Shootball.Extensions
{
    public static class Random
    {
        public static int Range(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Range(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        public static T FromArray<T>(T[] list, out int index)
        {
            index = Range(0, list.Length);
            return list[index];
        }

        public static T FromArray<T>(T[] list)
        {
            int i;
            return FromArray(list, out i);
        }

        public static T FromCollection<T>(ICollection<T> list)
        {
            return list.ElementAt(Range(0, list.Count));
        }

        public static T FromWeightedList<T>(IDictionary<T, int> weightedList)
        {
            var totalWeight = weightedList.Values.Sum();
            int chosen = Range(0, totalWeight);

            T selected = default(T);
            foreach (var keyValue in weightedList)
            {
                if (chosen < keyValue.Value)
                {
                    selected = keyValue.Key;
                    break;
                }
                chosen = chosen - keyValue.Value;
            }

            return selected;
        }

        public static UnityEngine.Vector3 VectorRange(float minLength, float maxLength)
        {
            return UnityEngine.Random.onUnitSphere * Range(minLength, maxLength);
        }
    }
}
