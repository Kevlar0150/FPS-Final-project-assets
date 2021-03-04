using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropMotion : MonoBehaviour
{
    private Vector3 velocity = Vector3.up;
    private Rigidbody rb;
    private Vector3 startPosition;
    public bool hitGround = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        velocity *= Random.Range(4f, 6f);
        velocity += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1, 1));

        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb.position += velocity * 2 * Time.deltaTime;

        Quaternion deltaRotation = Quaternion.Euler(new Vector3(Random.Range(-150f, 150f), Random.Range(-200, 200), Random.Range(-150, 150)) * 2 *  Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        if (velocity.y < -4f)
        {
            //velocity.y = -4f;
        }
        else
        {
            velocity -= Vector3.up * 20 * Time.deltaTime;
        }

        if (hitGround == true)
        {
            Debug.Log("Hit the ground");

            //Enable and disable RB properties so that Unity can handle the physics when hitting the ground
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            //rb.constraints = RigidbodyConstraints.FreezeAll;
            this.enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            hitGround = true;
        }
    }
}

