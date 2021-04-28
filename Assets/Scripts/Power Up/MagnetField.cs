using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The code to move the object along with the forceFactor variable has been produced following a tutorial by MakingMagic, 2016 https://www.youtube.com/watch?v=Xhp7nciaACU&t=8s
// The rest of the code has been produced 100% by me.
public class MagnetField : MonoBehaviour
{

    public Transform player;
    public float forceFactor = 50f;
    
    [SerializeField] private bool playerInMagnetRange;
    [SerializeField] public float magnetRange;
    [SerializeField] private LayerMask Player;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerInMagnetRange = Physics.CheckSphere(transform.position, magnetRange, Player);

        if (playerInMagnetRange)
        {
            // Adds force to object and moves it towrads the player object
            GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position) * forceFactor * Time.smoothDeltaTime);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}
