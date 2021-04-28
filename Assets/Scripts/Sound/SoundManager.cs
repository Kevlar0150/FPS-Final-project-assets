using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code produced in this script has been produced 100% by me
public class SoundManager : MonoBehaviour
{
    public static AudioClip enemyLaserShoot, explosion1, explosion2, explosion3,
          playerHit,speedup,healthup,ammopickup;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        enemyLaserShoot = Resources.Load<AudioClip>("enemyLaserShoot");
        explosion1 = Resources.Load<AudioClip>("explosion1");
        explosion2 = Resources.Load<AudioClip>("explosion2");
        explosion3 = Resources.Load<AudioClip>("explosion3");
        playerHit = Resources.Load<AudioClip>("playerHit");
        healthup = Resources.Load<AudioClip>("healthup");
        speedup = Resources.Load<AudioClip>("speedup");
        ammopickup = Resources.Load<AudioClip>("ammopickup");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string sound)
    {

        if (sound == "enemylaserShot")
        {
            audioSource.PlayOneShot(enemyLaserShoot);
        }
        if (sound == "playerhit")
        {
            audioSource.PlayOneShot(playerHit);
        }
        if (sound == "ammopickup")
        {
            audioSource.PlayOneShot(ammopickup);
        }
        if (sound == "speedup")
        {
            audioSource.PlayOneShot(speedup);
        }
        if (sound == "healthup")
        {
            audioSource.PlayOneShot(healthup);
        }
        if (sound == "explosion")
        {
            int random = Random.Range(0, 2);

            if (random == 0)
            {
                audioSource.PlayOneShot(explosion1);
            }
            if (random == 1)
            {
                audioSource.PlayOneShot(explosion2);
            }
            if (random == 2)
            {
                audioSource.PlayOneShot(explosion3);
            }
        }
    }
}
