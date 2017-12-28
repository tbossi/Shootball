using UnityEngine;
using Shootball.Model;

public class Shot : MonoBehaviour
{
    public ShotModel ShotModel;

    void Update()
    {
        ShotModel.Move();
    }

	void OnCollisionEnter(Collision collisionInfo)
	{
        ShotModel.Hit(collisionInfo.gameObject);
	}
}
