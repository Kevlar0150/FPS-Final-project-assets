using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class SwordCombatVR : MonoBehaviour
{
    //Controller variables
    InputDevice deviceR;
    public XRNode rightController;
    public InputDeviceCharacteristics controllerCharacteristics;
    Vector3 controllerVelocity;
    private InputDevice targetDevice;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        deviceR = InputDevices.GetDeviceAtXRNode(rightController);
        
    }

    // Update is called once per frame
    void Update()
    {
        deviceR.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 controllerVelocity);
        // If sword is held
        if (GetComponent<XRGrabInteractable>().GetIsHeld())
        {
            // Disable the pickup box collider when held
            GetComponent<BoxCollider>().enabled = false;

            // IF velocity of controller is more than 1 - means it has been swung
            if (controllerVelocity.magnitude > 1.0f)
            {
                Debug.Log("Swung Sword");
                GetComponentInChildren<MeshCollider>().enabled = true;
            }
            else
            {
                GetComponentInChildren<MeshCollider>().enabled = false;
            }

        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
        }
        
    }
    public void EnableOutline()
    {
        if (transform.parent == null)
        {
            GetComponent<Outline>().OutlineColor = Color.green;
            GetComponent<Outline>().enabled = true;
        }
    }
    public void DisableOutline()
    {
        GetComponent<Outline>().enabled = false;
    }

    public void ToggleSwordHitbox()
    {
        
    }
}
