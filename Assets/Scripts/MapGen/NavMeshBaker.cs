using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshBaker : MonoBehaviour
{
   
    [SerializeField] private List<NavMeshSurface> navMeshSurfaces;
    [SerializeField] private List<Room> roomList;
    [SerializeField] private LevelBuilder levelBuilder;
    [SerializeField] private GameObject[] roomArray;
    [SerializeField] bool hasBaked = false;
    [SerializeField] bool hasAddedRoom = false;
    [SerializeField] int index;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        BakeRooms();
    }

    private void BakeRooms()
    {
        // returns array of all objects in current scene with tag "Room" and stores the array in roomArray
        roomArray = GameObject.FindGameObjectsWithTag("Room");

        // If levelgeneration has finished AND game hasn't added rooms to List
        if (levelBuilder.hasRoomFinished() && !hasAddedRoom)
        {
            Debug.Log("Adding");

            // If iteration index is less than the amount of rooms in the scene...
            if (index < roomArray.Length) 
            {
                //Get gameobject that contains the NavMeshSurface component. Allowing us to store gameobject as a NavMeshSurface type.
                foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
                {
                    // Adds Room object to the list
                    roomList.Add(room.GetComponent<Room>()); 

                    // Gets the child object of the Room object which is the Mesh and Add to the navMeshSurfaces list
                    navMeshSurfaces.Add(room.gameObject.transform.GetChild(0).GetComponent<NavMeshSurface>() as NavMeshSurface);   
                    
                    // Increase index by 1
                    index++;
                }
            }  
        }
        // If levelgeneration has finished AND game hasn't built NavMesh for each mesh
        if (levelBuilder.hasRoomFinished() && !hasBaked)
        { 

            // iterate through each mesh and build navigation mesh
            for (int i = 0; i < roomArray.Length; i++)
            {
                navMeshSurfaces[i].BuildNavMesh();
            }

            // Bool to tell program that baking has completed.
            hasBaked = true; 
        }
    }
}
