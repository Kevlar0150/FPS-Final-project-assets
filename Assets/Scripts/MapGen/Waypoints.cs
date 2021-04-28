using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entire code present in this script has been 100% created by me
public class Waypoints : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}
