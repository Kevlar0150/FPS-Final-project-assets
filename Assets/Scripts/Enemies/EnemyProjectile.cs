using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyProjectile : MonoBehaviour
{
    // Projectile properties
    public float speed = 1f;
    public float lifeDuration = 3f;
    public float damage = 1;

    private float lifeTimer;
    public GameObject impactVFX;

    private bool hit = false; 

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // Make bullet move forward
        transform.position += transform.forward * speed;

        //If statement for destroying bullet to save memory.
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            DestroyObject(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
        // Impact VFX when projectil collides with anything
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.transform.tag != "EnemyProjectile")
        {
            DestroyObject(gameObject);
            GameObject impactVFXObject = Instantiate(impactVFX, transform.position, transform.rotation);
            Destroy(impactVFXObject, 1.7f);
        }

        // If function to damage player hasn't been called then run it
        if (!hit)
        {
            // Damage player
            Player playerObject = collision.transform.GetComponent<Player>(); // Gets Player component when collides Player - Allows us to call functions in Player script.
            if (playerObject != null) // If playerObject HAS FOUND the Player component and does not equal NULL
            {
                Debug.Log("HIT BY BOSS");
                playerObject.TakeDamage(damage);

                // Set "hit" to true so that enemy bullet doesn't damage player twice in 1 shot.
                hit = true;
            }
        }
    }
}
