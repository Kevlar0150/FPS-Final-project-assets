using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    StartRoom startRoom;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        startRoom = FindObjectOfType<StartRoom>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        { 
            other.transform.position = startRoom.playerStartPosition.position;
            other.transform.rotation = startRoom.playerStartPosition.rotation;
        }
    }
}
