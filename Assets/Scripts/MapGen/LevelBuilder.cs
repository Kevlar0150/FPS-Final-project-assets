using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sources used:
//https://docs.unity3d.com/ScriptReference/Physics.OverlapBox.html

public class LevelBuilder : MonoBehaviour
{
	public Room startRoomPrefab, endRoomPrefab;
	public List<Room> roomPrefabs = new List<Room> ();
	public int iterationMin, iterationMax;
	public Player playerPrefab;

	List<Doorway> availableDoorways = new List<Doorway> ();

	StartRoom startRoom;
	EndRoom endRoom;
	List<Room> placedRooms = new List<Room> ();

	LayerMask roomLayerMask;

	Player player;

	void Start ()
	{
		roomLayerMask = LayerMask.GetMask ("Room");
		StartCoroutine ("GenerateLevel");
	}

	IEnumerator GenerateLevel ()
    {
        WaitForSeconds startup = new WaitForSeconds(1); // Time to wait until Generator starts
        WaitForFixedUpdate interval = new WaitForFixedUpdate(); // Time to wait until next code execution.

        yield return startup;

        // Place start room
        PlaceStartRoom();
        yield return interval;

        //How many times to loop. Basically how many times to spawn a room(Corridor) before end room is spawned
        int numOfCorridors = Random.Range(iterationMin, iterationMax);

        for (int i = 0; i < numOfCorridors; i++)
        {
            // Place random room from Room prefabs list (roomPrefabs)
            PlaceRoom();
            yield return interval;
        }

        // Place end room
        PlaceEndRoom();
        yield return interval;

        // Level generation finished
        Debug.Log("Level generation finished");

        SpawnPlayer();

        //		yield return new WaitForSeconds (3);
        //		ResetLevelGenerator ();
    }

    private void SpawnPlayer()
    {
        // Place player
        player = Instantiate(playerPrefab) as Player;
        player.transform.position = startRoom.playerStartPosition.position;
        player.transform.rotation = startRoom.playerStartPosition.rotation;
    }

    void PlaceStartRoom ()
	{
		// Instantiate room
		startRoom = Instantiate (startRoomPrefab) as StartRoom;
		startRoom.transform.parent = this.transform;

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList (startRoom, ref availableDoorways);

		// Position room
		startRoom.transform.position = Vector3.zero;
		startRoom.transform.rotation = Quaternion.identity;
	}

	void AddDoorwaysToList (Room room, ref List<Doorway> list)
	{
		foreach (Doorway doorway in room.doorways) {

			// For Loop that makes the order of the doorway list more random thus making level generation more random.
			for (int i = 0; i < list.Count; i++) 
			{
				Doorway tempDoorway = list[i];
				int randomIndex = Random.Range(i, list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = tempDoorway;
			}
			int r = Random.Range (0, list.Count);
			list.Insert (r, doorway);
		}
	}

	void PlaceRoom ()
	{
		// Instantiate room
		Room currentRoom = Instantiate (roomPrefabs [Random.Range (0, roomPrefabs.Count)]) as Room;
		currentRoom.transform.parent = this.transform;

		// Create doorway lists to loop over
		List<Doorway> allAvailableDoorways = new List<Doorway> (availableDoorways);
		List<Doorway> currentRoomDoorways = new List<Doorway> ();
		AddDoorwaysToList (currentRoom, ref currentRoomDoorways);

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList (currentRoom, ref availableDoorways);

		bool roomPlaced = false;

		// Try all available doorways
		foreach (Doorway availableDoorway in allAvailableDoorways) {
			// Try all available doorways in current room
			foreach (Doorway currentDoorway in currentRoomDoorways) {
				// Position room
				PositionRoomAtDoorway (ref currentRoom, currentDoorway, availableDoorway);

				// Check if currentRoom overlaps with any other colliders - If false then that means no overlaps
				if (!CheckRoomOverlap(currentRoom))
				{
					roomPlaced = true;
				}
				
				// Add room to list
				placedRooms.Add (currentRoom);

				// Remove doorway that's connected
				currentDoorway.gameObject.SetActive (false);
				availableDoorways.Remove (currentDoorway);

				availableDoorway.gameObject.SetActive (false);
				availableDoorways.Remove (availableDoorway);

				// Exit loop if room has been placed
				if (roomPlaced)
				{
					break;
				}
			}

			// Exit loop if room has been placed
			if (roomPlaced) {
				break;
			}
		}

		// Room couldn't be placed. Restart generator and try again
		if (!roomPlaced) {
			Destroy (currentRoom.gameObject);
			ResetLevelGenerator ();
		}
	}

	void PositionRoomAtDoorway (ref Room room, Doorway roomDoorway, Doorway targetDoorway)
	{
		// Reset room position and rotation
		room.transform.position = Vector3.zero;
		room.transform.rotation = Quaternion.identity;

		// Rotate room to match previous doorway orientation
		Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
		Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
		float deltaAngle = Mathf.DeltaAngle (roomDoorwayEuler.y, targetDoorwayEuler.y);
		Quaternion currentRoomTargetRotation = Quaternion.AngleAxis (deltaAngle, Vector3.up);
		room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler (0, 180f, 0);

		// Position room
		Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
		room.transform.position = targetDoorway.transform.position - roomPositionOffset;
	}

	bool CheckRoomOverlap (Room room)
	{
		Bounds bounds = room.RoomBounds;
		bounds.center = room.transform.position;
		bounds.Expand (-0.1f);

		Collider[] colliders = Physics.OverlapBox (bounds.center, bounds.size/1.5f, Quaternion.identity, roomLayerMask);
		if (colliders.Length > 0) {
			// Ignore collisions with current room
			foreach (Collider c in colliders) {
				while (!c.transform.parent.gameObject.Equals (room.gameObject))  // while collider in arrays' parent does not equal to itself then return true;
				{
					return true;
				}
			}
		}
		return false; // Returns false if no overlap detected
	}

	void PlaceEndRoom ()
	{
		// Instantiate room
		endRoom = Instantiate (endRoomPrefab) as EndRoom;
		endRoom.transform.parent = this.transform;

		// Create doorway lists to loop over
		List<Doorway> allAvailableDoorways = new List<Doorway> (availableDoorways);
		Doorway doorway = endRoom.doorways [0];

		bool roomPlaced = false;

		// Try all available doorways
		foreach (Doorway availableDoorway in allAvailableDoorways) {
			// Position room
			Room room = (Room)endRoom;
			PositionRoomAtDoorway (ref room, doorway, availableDoorway);

			// Check if endRoom overlaps with any other room. If false returned it means no overlaps found
			if (!CheckRoomOverlap(endRoom))
			{
				roomPlaced = true;
			}

			// Remove occupied doorways
			doorway.gameObject.SetActive (false);
			availableDoorways.Remove (doorway);

			availableDoorway.gameObject.SetActive (false);
			availableDoorways.Remove (availableDoorway);

			// Exit loop if room has been placed
			if (roomPlaced)
			{
				break;
			}
		}

		// Room couldn't be placed. Restart Generator
		if (!roomPlaced) {
			ResetLevelGenerator ();
		}
	}

	void ResetLevelGenerator ()
    {
        //Debug.LogError("Reset generator");

		// Stop Coroutine
        StopCoroutine("GenerateLevel");

        DeleteAllRooms();

        ClearLists();

        // Restart Coroutine
        StartCoroutine("GenerateLevel");
    }

    private void ClearLists()
    {
		// Clears the list of placed rooms and available doorways so it doesn't interfere with next generation
        placedRooms.Clear();
        availableDoorways.Clear();
    }

    void DeleteAllRooms()
	{
		// Deletes all rooms
		if (startRoom)
		{ Destroy(startRoom.gameObject); }

		if (endRoom)
		{ Destroy(endRoom.gameObject); }

		foreach (Room room in placedRooms) //Deletes all rooms in placedRooms lists
		{ Destroy(room.gameObject); }
	}
}
