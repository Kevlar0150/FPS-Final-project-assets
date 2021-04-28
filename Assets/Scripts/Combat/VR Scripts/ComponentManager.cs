using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Entire script has been produced 100% by me.
public class ComponentManager : MonoBehaviour
{
    XRGrabInteractable grabScript;
    Gun_raycastVR GunScript;
    LineRenderer lineRenderer;
    EnergyCannonVR energyCannonVR;
    // Start is called before the first frame update
    void Start()
    {
        GunScript = GetComponent<Gun_raycastVR>();
        grabScript = GetComponent<XRGrabInteractable>();
        lineRenderer = GetComponent<LineRenderer>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (grabScript.GetIsHeld())
        {
            GunScript.enabled = true;
            lineRenderer.enabled = true;
        }
        else
        {
            GunScript.enabled = false;
            lineRenderer.enabled = false;
        }
    }
}
