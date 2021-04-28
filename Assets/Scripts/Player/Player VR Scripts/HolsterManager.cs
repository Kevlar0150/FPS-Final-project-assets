using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// The script solely containing code to rotate the object as VR headset rotates has been produced using tutorial by My.Pineapple Studio, 2020, https://www.youtube.com/watch?v=FFM2oyLUysk&t=5s
public class HolsterManager : MonoBehaviour
{
    [SerializeField]private Camera playerCamera;
    [SerializeField] public Transform holsterPosition;

    private float rotationSpeed = 150;

    private XRGrabInteractable grabInteractable;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(playerCamera.transform.position.x,playerCamera.transform.position.y-0.75F, playerCamera.transform.position.z);

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
