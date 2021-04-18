using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//Sources used:
//https://forum.unity.com/threads/solved-shotgun-shooting-c.464014/ - For shotgun creation
//https://forum.unity.com/threads/hitscan-raycasting-hits-vs-projectile-simulation.334463/ - Help deciding on what hit detection to use
//https://docs.unity3d.com/Manual/Layers.html - Help with LayerMask when shooting.

public class Gun_raycastVR : MonoBehaviour
{
    // Gun properties
    public float damage;
    public float firingRate;
    public float shootRange;
    public float bulletSpread;
    public float reloadTime;
    private float shootInterval = 0f;
    public int bulletsPerShot;

    // Ammo variables
    public int gunClipSize; // Clip of the gun *Doesn't get changed*
    public int tempclipSize; // Temporary variable with same value of clip size.
    public int clipDifferent; // Variable that stores difference of bullets remaining and clip size.
    public int magSize; // Mag size of gun
    public int magSizeCapacity; // Default mag size that *doesn't get changed*
    public int bulletsRemaining; // How many bullets are remaining in the gun

    // Bool
    bool reloading;

    // References
    public ParticleSystem muzzleFlash;
    public GameObject impactVFX;
    public RaycastHit hitInfo; // Store's whatever the raycast hits into the variable hitInfo
    public Transform player;
    Animator anim;

    //Rays
    Ray singleFireRay;
    Ray shotgunRay;

    // VR Controller variables
    public InputDeviceCharacteristics controllerCharacteristics;
    private InputDevice targetDevice;

    private void Start()
    {
        // Initialise variables upon start
        magSize = gunClipSize * 4;
        magSizeCapacity = magSize;
        bulletsRemaining = gunClipSize;

        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

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
    

    // Shoot function
    public void Shoot()
    {
        bulletsRemaining--; // Decrease amount of bullets available to shoot

        // Bullet spread for Pistol, Assault Rifle, Revolver
        float spreadX = Random.Range(-bulletSpread, bulletSpread);
        float spreadY = Random.Range(-bulletSpread, bulletSpread);

        Vector3 spreadDirection = gameObject.transform.forward + new Vector3(spreadX, spreadY, 0);
        var layerMask = 1 << 11; // Bit shifts the index of layer 11 (Player layer) to get bit mask

        layerMask = ~layerMask; // We invert it using the ~ sign so that we can collide with everything EXCEPT Layer 11 which is the player.
                                // This is so that we don't shoot ourselves.

        muzzleFlash.Play(); // Play the muzzle flash particle system  

        // ----------- Pistol,AssaultRifle,Sniper --------------

        // Creates the ray at camera position, direction. Outputs whatever the ray touches into the hitInfo variable, 
        // length of ray is based on shoot range, Ray collides with everything except layer 11.
        if (!transform.tag.Equals("Shotgun") && Physics.Raycast(gameObject.transform.position, spreadDirection, out hitInfo, shootRange, layerMask))
        {

            // Instantiate a gameobject with the particle effect at the point of hit. Make the effect normalized so that it is instantiated in the correct direction.
            GameObject impactVFXObject = Instantiate(impactVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactVFXObject, 0.75f); // Destroy the impact VFX after 2 seconds.

            // If ray hit info is enemy, meaning if bullet hits the enemy then damage enemy
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            BossEnemy boss = hitInfo.transform.GetComponent<BossEnemy>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }

        // ----------- Shotgun ------------

        // Shotgun raycast with for loop to create multiple raycasts depending on bulletsPerShot on click.
        if (transform.tag.Equals("Shotgun"))
        {
            // For loop to create multiple ray/bullet shots
            for (int i = 0; i <= bulletsPerShot; i++)
            {
                // Own bullet spread so that each bullet from 1 shot has a randomized spread
                float shotgunSpreadX = Random.Range(-bulletSpread, bulletSpread);
                float shotgunSpreadY = Random.Range(-bulletSpread, bulletSpread);
                Vector3 shotgunSpread = gameObject.transform.forward + new Vector3(shotgunSpreadX, shotgunSpreadY, 0);

                if (Physics.Raycast(gameObject.transform.position, shotgunSpread, out hitInfo, shootRange, layerMask))
                {
                    // Impact VFX
                    GameObject impactVFXObject = Instantiate(impactVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(impactVFXObject, 0.75f); // Destroy the impact VFX after 2 seconds.

                    // Damage enemy
                    Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                    BossEnemy boss = hitInfo.transform.GetComponent<BossEnemy>();
                    if (boss != null)
                    {
                        boss.TakeDamage(damage);
                    }
                }
            }
        }
    }

    // Reloading functions
    private void Reload()
    {
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

    // Getters function
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

}

