using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HolsterManager : MonoBehaviour
{
    public BoxCollider rightHolster;
    public BoxCollider leftHolster;  

    [SerializeField]private Camera playerCamera;
    private float rotationSpeed = 75;

    private XRGrabInteractable grabInteractable;


    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y/1.3f , playerCamera.transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        //Position holster at hips of player (Usually halfway)
      

        var rotationDifference = Mathf.Abs(playerCamera.transform.eulerAngles.y - transform.eulerAngles.y);
        var finalRotationSpeed = rotationSpeed;

        if (rotationDifference > 60)
        {
            finalRotationSpeed = rotationSpeed * 2;
        }
        else if (rotationDifference > 40 && rotationDifference < 60)
        {
            finalRotationSpeed = rotationSpeed;
        }
        else if (rotationDifference < 40 && rotationDifference > 20)
        {
            finalRotationSpeed = rotationSpeed / 2;
        }
        else if (rotationDifference < 20 && rotationDifference > 0)
        {
            finalRotationSpeed = rotationSpeed / 4;
        }

        var step = finalRotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0), step);
    }
   
}
