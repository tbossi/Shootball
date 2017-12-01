using UnityEngine;

namespace Shootball
{
    public class ArenaBounds : MonoBehaviour
    {
        void Start()
        {

        }

		void OnTriggerExit(Collider other)
		{
			System.Console.WriteLine("qwe");
			Destroy(other.gameObject);
		}
    }
}