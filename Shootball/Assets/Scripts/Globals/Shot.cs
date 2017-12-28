using UnityEngine;
using Shootball.Model;

public class Shot : MonoBehaviour
{
    public float Speed;
	public IShooter Shooter;

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * Speed;
    }

	void OnCollisionEnter(Collision collisionInfo)
	{
		Destroy(gameObject);
	}
}
