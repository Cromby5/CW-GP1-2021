using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : gunLogic
{
    [SerializeField] private int pelletAmount; // Amount of pellets to fire

    public Vector3 pelletOffset; // Offset of pellets to be fired

    protected override void RayCast()
    {
        for (int i = 0; i < pelletAmount; i++)
        {
            pelletOffset = new Vector3(Random.Range(0.45f,0.55f), Random.Range(0.45f, 0.55f), Random.Range(0.45f, 0.55f)); // Generate a random vector3 in this range
            RaycastHit hit;
            Ray ray = cam.ViewportPointToRay(pelletOffset); // Shoot out a ray at the generated random offset

            if (Physics.Raycast(ray, out hit, range))
            {
                GameObject choosenBulletHole = bulletHolePrefabs[Random.Range(0, bulletHolePrefabs.Length)]; // Randomize Bullet Hole
                var tempBullet = Instantiate(choosenBulletHole, hit.point, Quaternion.LookRotation(hit.normal)); // Bullet Hole
                tempBullet.transform.parent = hit.transform; // Put the bullet hole on the object by making it a child
                
                if (hit.transform.gameObject.tag == "ForceAffected")
                {
                    var direction = new Vector3(hit.transform.position.x - transform.position.x, hit.transform.position.y - transform.position.y, hit.transform.position.z - transform.position.z); // Determine the direction we should end up shooting the object in
                    hit.rigidbody.AddForceAtPosition(force * Vector3.Normalize(direction), hit.point); // Applys a force in the direction of the shot
                }

                EnemyHealthSystem thierHealthSystem = hit.transform.GetComponent<EnemyHealthSystem>(); // Looking for an enemy health system
                if (thierHealthSystem != null)
                {
                    thierHealthSystem.TakeDamage(damage); // Thier health system takes the amount of damage we output to them
                }
            }
        }
    }
}
