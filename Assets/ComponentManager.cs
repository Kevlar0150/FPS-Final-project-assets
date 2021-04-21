using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ComponentManager : MonoBehaviour
{
    XRGrabInteractable grabScript;
    Gun_raycastVR GunScript;
    // Start is called before the first frame update
    void Start()
    {
        GunScript = GetComponent<Gun_raycastVR>();
        grabScript = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabScript.GetIsHeld())
        {
            GunScript.enabled = true;
        }
        else
        {
            GunScript.enabled = false;
        }
    }
}
