using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The script solely containing code to rotate the object as VR headset rotates has been produced using tutorial by My.Pineapple Studio, 2020, https://www.youtube.com/watch?v=FFM2oyLUysk&t=5s
public class SwordManager : MonoBehaviour
{

    [SerializeField] private Camera playerCamera;
    private float rotationSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }
    private void FixedUpdate()
    {
        //Position at chest of player
        transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y / 1.15f, playerCamera.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
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
