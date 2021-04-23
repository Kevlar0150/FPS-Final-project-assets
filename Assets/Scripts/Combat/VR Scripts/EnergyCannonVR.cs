using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class EnergyCannonVR : MonoBehaviour
{
    // Prefabs & References
    public Transform playerCamera;
    public Transform player;
    public GameObject muzzlePosition;
    public ParticleSystem muzzleFlash;
    public GameObject bulletPrefab;
    Animator anim;

    // Gun property
    public float shootInterval = 69;
    public float firingRate;
    public float spread;

    //Ammo variables
    public int gunClipSize; // Clip of the gun *Doesn't get changed*
    public int tempclipSize; // Temporary variable with same value of clip size.
    public int clipDifferent; // Variable that stores difference of bullets remaining and clip size.
    public int magSize; // Mag size of gun
    public int magSizeCapacity; // Default mag size that *doesn't get changed*
    public int bulletsRemaining; // How many bullets are remaining in the gun

    //Reloading variables
    bool reloading;
    public float reloadTime;

    // VR Controller variables
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;

    private void Start()
    {
        // Initialise variables upon start
        magSize = gunClipSize * 4;
        magSizeCapacity = magSize;
        bulletsRemaining = gunClipSize;

        // Initialise references and prefabs
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;

        // Get Controllers

        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }

    }
    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Gets VR controller trigger and if pushed, calls Shoot function
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0 && Time.time >= shootInterval && !reloading && bulletsRemaining > 0)
        {
            shootInterval = Time.time + 1f / firingRate; // Sets shoot interval based on time and firing rate
            Shoot(); // Calls shoot function below
        }

        if (triggerValue > 0 && bulletsRemaining <= 0) { Reload(); }

        // Reload button
        if (Input.GetKeyDown(KeyCode.R) && bulletsRemaining < gunClipSize && !reloading && magSize > 0)
        {
            Reload(); // Calls function for reloading the gun
        }
        //Debug.DrawLine(transform.position, hitInfo.point, Color.red);
    }

    void Shoot()
    {
        bulletsRemaining--;

        // Randomize the spread of the bullet to simulate recoil
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

            muzzleFlash.Play();
            GameObject bulletObject = Instantiate(bulletPrefab); // Instantiate bullet mesh being passed in
            bulletObject.transform.position = muzzlePosition.transform.position + gameObject.transform.forward + new Vector3(spreadX, spreadY, 0); // Set position of the bullet
            bulletObject.transform.forward = gameObject.transform.forward; // Set facing direction of the bullet
            Debug.Log("Fire!"); // Debug log to see if this If statement works.
    }
    public void Reload()
    {
        if(bulletsRemaining < gunClipSize && !reloading && magSize > 0)
        {
            Debug.Log("Reload");
            reloading = true; // Set to true so player cannot shoot while reloading.

            Invoke("ReloadFinished", reloadTime); // Call ReloadFinish function after reloadTime has finished
        }
    }

    private void ReloadFinished()
    {
        tempclipSize = gunClipSize; // Temp variable = the gun's clip size that's been set
        clipDifferent = tempclipSize -= bulletsRemaining; // clip difference is temp clip size minus bullets remaining
        bulletsRemaining = bulletsRemaining + clipDifferent; // Add the difference to bullets remaining
        magSize -= clipDifferent; // Subtract the difference to magSize

        // If statement to stop magSize from being minus value.
        if (magSize <= 0) { magSize = 0; }

        reloading = false;
    }

    public int getAmmoClip() { return gunClipSize; }
    public int getAmmoMag() { return magSize; }
    public int getBulletsRemaining() { return bulletsRemaining; }

    // Setters function (Increasing variable or something)
    public void increaseMag(int multiplier)
    {
        // Increase mag size by maximum Mag capacity divided by a multiplier (Value being passed in).
        magSize += (magSizeCapacity / multiplier);

        // Stops mag size from going over the maximum capicity
        if (magSize >= magSizeCapacity)
        {
            magSize = magSizeCapacity;
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
}
