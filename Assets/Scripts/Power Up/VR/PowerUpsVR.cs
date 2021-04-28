using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Sources used:
//https://answers.unity.com/questions/44137/if-gameobject-is-active.html - Check if object is active or not
//https://docs.unity3d.com/ScriptReference/Transform.GetChild.html - To get child of a child

// Entire code present in this script has been produced by me.
// Code has been reused from the PowerUps script and adapted to work in VR Mode
public class PowerUpsVR : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        // Gets player object from collision.
        Player playerObject = other.transform.GetComponent<Player>();

        // Gets transform of handslot for VR 
        Transform rightHand = playerObject.transform.GetChild(1).GetChild(0).GetChild(2);

        if (other.gameObject.tag == "Player")
        {
            DestroyObject(gameObject);
            if (gameObject.transform.tag == "Ammo")
            {
                Debug.Log("AMMO");

                if (rightHand.gameObject.activeSelf)
                 {
                    Debug.Log("IT HAPPNONG");
                     // Increases mag size of the childObject of WeaponSlot1 which is the Raycast type guns.
                     if (rightHand.gameObject.GetComponentInChildren<Gun_raycastVR>())
                     {
                         rightHand.gameObject.GetComponentInChildren<Gun_raycastVR>().increaseMag(4);
                     }

                     // Increases mag size of the childObject of WeaponSlot1 which is the energy type gun
                     if (rightHand.gameObject.GetComponentInChildren<EnergyCannonVR>())
                     {
                         rightHand.gameObject.GetComponentInChildren<EnergyCannonVR>().increaseMag(4);
                     }
                 }

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
