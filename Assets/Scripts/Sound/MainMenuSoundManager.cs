using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code present in this script has been produced 100% by me
public class MainMenuSoundManager : MonoBehaviour
{
    public static AudioClip mainMenuSong;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuSong = Resources.Load<AudioClip>("MainMenu");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerMenuMusic()
    {
        audioSource.Play();
    }

}
