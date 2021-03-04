using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Player playerObject = other.transform.GetComponent<Player>();
        if (other.gameObject.tag == "Player")
        {
            DestroyObject(gameObject);
            if (gameObject.transform.tag == "Ammo")
            {
                Debug.Log("AMMO");
            }
            if (gameObject.transform.tag == "Health")
            {
                Debug.Log("HEALTH");
                playerObject.increaseHealth(10);
            }
            if (gameObject.transform.tag == "Shield")
            {
                Debug.Log("SHIELD");
                //Activate shield - possibly call function from Player to enable
            }
            if (gameObject.transform.tag == "SpeedBoost")
            {
                Debug.Log("SPEED");
                playerObject.increaseSpeed(1.5f, true);
                //Activate speed boost - call function to multiple the speed of character
            }
        }
    }
}
