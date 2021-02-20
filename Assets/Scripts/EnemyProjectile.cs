using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 1f;
    public float lifeDuration = 2f;
    public float spread;

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
        transform.position += transform.forward;

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
        GameObject impactVFXObject = Instantiate(impactVFX, transform.position, transform.rotation);
        DestroyObject(gameObject);
        Destroy(impactVFXObject, 1.7f);
    }
}
