using UnityEngine;

// Script entirely from the youtube tutorial by ProjectShasta, (2018) https://www.youtube.com/watch?v=C4ZqrhCP0Bg&list=PLvMpomwW7ZQH3jHDyFP_hUS8560E4SdKM
public class Doorway : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
   
}
