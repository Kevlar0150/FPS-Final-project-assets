using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//Sources used:
//https://answers.unity.com/questions/44137/if-gameobject-is-active.html - Check if object is active or not
//https://docs.unity3d.com/ScriptReference/Transform.GetChild.html - To get child of a child
// Entire code presen in the script has been produced by me while using the above resources to achieve desired results.

public class PowerUps : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        // Gets player object from collision.
        Player playerObject = other.transform.GetComponent<Player>();

        //Gets the child of CharacterPlayer object called Camera. Then gets the child(WeaponSlots) of the child (Camera).
        Debug.Log(playerObject.gameObject.transform.GetChild(2).GetChild(0));
        Debug.Log(playerObject.gameObject.transform.GetChild(2).GetChild(1));

        // Gets the transform of the weaponslot by using GetChild function and store in variable.
        Transform weaponSlot1 = playerObject.gameObject.transform.GetChild(2).GetChild(0);
        Transform weaponSlot2 = playerObject.gameObject.transform.GetChild(2).GetChild(1);

        // Gets transform of handslot for VR 
        //Transform rightHand = FindObjectOfType<XRController>();

        if (other.gameObject.tag == "Player")
        {
            DestroyObject(gameObject);
            if (gameObject.transform.tag == "Ammo")
            {
                Debug.Log("AMMO");

                if (weaponSlot1.gameObject.activeSelf) // Checks if WeaponSlot object is active.
                {
                    // Increases mag size of the childObject of WeaponSlot1 which is the Raycast type guns.
                    if (weaponSlot1.gameObject.GetComponentInChildren<Gun_raycast>())
                    {
                        weaponSlot1.gameObject.GetComponentInChildren<Gun_raycast>().increaseMag(4);
                    }

                    // Increases mag size of the childObject of WeaponSlot1 which is the energy type gun
                    if (weaponSlot1.gameObject.GetComponentInChildren<EnergyCannon>())
                    {
                        weaponSlot1.gameObject.GetComponentInChildren<EnergyCannon>().increaseMag(4);
                    }                
                }
                if (weaponSlot2.gameObject.activeSelf)
                {
                    // Increases mag size of the childObject of WeaponSlot2 which is the guns.
                    if (weaponSlot2.gameObject.GetComponentInChildren<Gun_raycast>())
                    {
                        weaponSlot2.gameObject.GetComponentInChildren<Gun_raycast>().increaseMag(4);
                    }

                    if (weaponSlot2.gameObject.GetComponentInChildren<EnergyCannon>())
                    {
                        weaponSlot2.gameObject.GetComponentInChildren<EnergyCannon>().increaseMag(4);
                    }
                }
               /* if (rightHand.gameObject.activeSelf)
                {
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
                }*/

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
