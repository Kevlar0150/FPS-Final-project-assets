using UnityEngine;

// Script entirely from the youtube tutorial by ProjectShasta, (2018) https://www.youtube.com/watch?v=C4ZqrhCP0Bg&list=PLvMpomwW7ZQH3jHDyFP_hUS8560E4SdKM
public class Room : MonoBehaviour
{
	public Doorway[] doorways;
	public BoxCollider boxCollider;

	public BoxCollider RoomBounds {
		get { return boxCollider; }
	}
}
