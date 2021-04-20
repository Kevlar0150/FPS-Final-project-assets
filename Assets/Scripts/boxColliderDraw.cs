using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxColliderDraw : MonoBehaviour
{
    BoxCollider box;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, box.bounds.size);
    }
}
