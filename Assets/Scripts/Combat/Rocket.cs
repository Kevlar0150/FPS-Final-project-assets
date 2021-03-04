using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 15f;
    public float lifeDuration = 2f;
    public float spread;
    public float damage = 25f;

    private float lifeTimer;
    public GameObject impactVFX;
    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = lifeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        // Make bullet move forward
        transform.position += transform.forward  * speed * Time.deltaTime;

        //Check if bullet needs to be destroyed to save memory.
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            DestroyObject(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // If we want collision to occur when specified object is hit add "if (collision.gameObject.tag == "Something")"
        GameObject impactVFXObject = Instantiate(impactVFX,transform.position,transform.rotation);
        DestroyObject(gameObject);
        Destroy(impactVFXObject, 1.7f);

        // Damage player
        Enemy enemyObject = collision.transform.GetComponent<Enemy>(); // Gets Player component when collides Player - Allows us to call functions in Player script.
        if (enemyObject != null) // If playerObject HAS FOUND the Player component and does not equal NULL
        {
            //Debug.Log("I'm hit");
            enemyObject.TakeDamage(damage);
        }
    }
}
