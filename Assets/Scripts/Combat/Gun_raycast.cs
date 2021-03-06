﻿using UnityEngine;

//Sources used:
//https://forum.unity.com/threads/solved-shotgun-shooting-c.464014/ - For shotgun creation
//https://forum.unity.com/threads/hitscan-raycasting-hits-vs-projectile-simulation.334463/ - Help deciding on what hit detection to use
//https://docs.unity3d.com/Manual/Layers.html - Help with LayerMask when shooting.

//The Shoot,reload and reloadfinished function has been produced using the tutorial provided by Dave/Gamedevelopment (2020) https://www.youtube.com/watch?v=bqNW08Tac0Y&t=4s
//The rest of the code has been produced by me,

public class Gun_raycast : MonoBehaviour
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
    public Transform playerCamera; 
    public ParticleSystem muzzleFlash;
    public GameObject impactVFX;
    public RaycastHit hitInfo; // Store's whatever the raycast hits into the variable hitInfo
    public Transform player;
    Animator anim;


    private void Start()
    {
        // Initialise variables upon start *My own code*
        magSize = gunClipSize * 4;
        magSizeCapacity = magSize;
        bulletsRemaining = gunClipSize;

        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;
    }

    // Update is called once per frame
    void Update()
    {
        //*My own code*
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetChild(2).transform;

        //Handling input was taken from Dave/GameDevelopment - Gun/Reload tutorial and adapted according to the new variables I've added.     
        if (Input.GetMouseButton(0) && Time.time >= shootInterval && !reloading && bulletsRemaining > 0) // Shoot button
        {
            shootInterval = Time.time + 1f / firingRate; // Sets shoot interval based on time and firing rate
            Shoot(); // Calls shoot function below
        }  
        if (Input.GetKeyDown(KeyCode.R) && bulletsRemaining < gunClipSize && !reloading && magSize > 0)    // Reload button
        {
            Reload(); // Calls function for reloading the gun
        }
    }

    // Shoot function - Base of the function taken from Dave/GameDevelopment https://www.youtube.com/watch?v=bqNW08Tac0Y&t=4s and added my own code to
    public void Shoot()
    {
        bulletsRemaining--; // Decrease amount of bullets available to shoot

        // Bullet spread for Pistol, Assault Rifle, Revolver
        float spreadX = Random.Range(-bulletSpread, bulletSpread);
        float spreadY = Random.Range(-bulletSpread, bulletSpread);

        Vector3 spreadDirection = playerCamera.transform.forward + new Vector3(spreadX, spreadY, 0);

        // Layermask code taken from 
        var layerMask = 1 << 11; // Bit shifts the index of layer 11 (Player layer) to get bit mask
        
        layerMask = ~layerMask; // We invert it using the ~ sign so that we can collide with everything EXCEPT Layer 11 which is the player.
                                // This is so that we don't shoot ourselves.

        muzzleFlash.Play(); // Play the muzzle flash particle system  

        // ----------- Pistol,AssaultRifle,Sniper --------------

        // Creates the ray at camera position, direction. Outputs whatever the ray touches into the hitInfo variable, 
        // length of ray is based on shoot range, Ray collides with everything except layer 11.
        if (!transform.tag.Equals("Shotgun") && Physics.Raycast(playerCamera.transform.position, spreadDirection, out hitInfo, shootRange, layerMask))
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
                Vector3 shotgunSpread = playerCamera.transform.forward + new Vector3(shotgunSpreadX, shotgunSpreadY, 0);

                if (Physics.Raycast(playerCamera.transform.position, shotgunSpread, out hitInfo, shootRange, layerMask))
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

    // Reloading functions also taken from Dave/Gamedev https://www.youtube.com/watch?v=bqNW08Tac0Y&t=4s
    private void Reload()
    {
        reloading = true; // Set to true so player cannot shoot while reloading.
        anim.SetBool("isReloading", true);
        
        Invoke("ReloadFinished", reloadTime); // Call ReloadFinish function after reloadTime has finished
    }

    // ReloadingFinished functions also taken from Dave/Gamedev https://www.youtube.com/watch?v=bqNW08Tac0Y&t=4s
    // But tweaked so that it you reload the gun, the number of bullets used to reload is also reduced from the magazine capacity.
    private void ReloadFinished()
    {
        tempclipSize = gunClipSize;                             // Temp variable = the gun's clip size that's been set
        clipDifferent = tempclipSize -= bulletsRemaining;       // clip difference is temp clip size minus bullets remaining
        bulletsRemaining = bulletsRemaining + clipDifferent;    // Add the difference to bullets remaining
        magSize -= clipDifferent;                                // Subtract the difference to magSize

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

