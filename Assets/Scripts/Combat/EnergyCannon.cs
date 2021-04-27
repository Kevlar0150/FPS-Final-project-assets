using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCannon : MonoBehaviour
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

    private void Start()
    {
        // Initialise variables upon start
        magSize = gunClipSize * 4;
        magSizeCapacity = magSize;
        bulletsRemaining = gunClipSize;

        // Initialise references and prefabs
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        anim = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        player = player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
        anim = GetComponent<Animator>();

        if (Input.GetMouseButton(0) && Time.time >= shootInterval && !reloading && bulletsRemaining > 0)
        {
            shootInterval = Time.time + 1f / firingRate; // Sets shoot interval based on time and firing rate
            Shoot(); // Calls shoot function below
        }
        // Reload button
        if (Input.GetKeyDown(KeyCode.R) && bulletsRemaining < gunClipSize && !reloading && magSize > 0)
        {
            Reload(); // Calls function for reloading the gun
        }
    }

    void Shoot()
    {
        bulletsRemaining--;

        // Randomize the spread of the bullet to simulate recoil
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

            muzzleFlash.Play();
            GameObject bulletObject = Instantiate(bulletPrefab); // Instantiate bullet mesh being passed in
            bulletObject.transform.position = muzzlePosition.transform.position + playerCamera.transform.forward + new Vector3(spreadX, spreadY, 0); // Set position of the bullet
            bulletObject.transform.forward = playerCamera.transform.forward; // Set facing direction of the bullet
            Debug.Log("Fire!"); // Debug log to see if this If statement works.
    }
    private void Reload()
    {
        Debug.Log("Reload");
        reloading = true; // Set to true so player cannot shoot while reloading.
        anim.SetBool("isReloading", true);

        Invoke("ReloadFinished", reloadTime); // Call ReloadFinish function after reloadTime has finished
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
        anim.SetBool("isReloading", false);
    }

    public int getAmmoClip() { return gunClipSize; }
    public int getAmmoMag() { return magSize; }
    public int getBulletsRemaining() { return bulletsRemaining; }

    // Setters function (Increasing variable or something)
    public void increaseMag(int multiplier)
    {
        Debug.Log("ENERGYGUN RELOAD");
        // Increase mag size by maximum Mag capacity divided by a multiplier (Value being passed in).
        magSize += (magSizeCapacity / multiplier);

        // Stops mag size from going over the maximum capicity
        if (magSize >= magSizeCapacity)
        {
            magSize = magSizeCapacity;
        }
    }
}
