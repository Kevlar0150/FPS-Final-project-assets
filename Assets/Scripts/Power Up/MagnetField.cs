using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetField : MonoBehaviour
{

    public Player player;
    public float forceFactor = 10f;

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<Player>();

        // Adds force to object and moves it towrads the player object
        GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position) * forceFactor * Time.smoothDeltaTime);
    }
}
