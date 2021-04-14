using UnityEngine;

public class Room : MonoBehaviour
{
	public Doorway[] doorways;
	public BoxCollider boxCollider;

	public BoxCollider RoomBounds {
		get { return boxCollider; }
	}
}
