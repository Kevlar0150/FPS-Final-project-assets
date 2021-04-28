using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Entire code present in this script has been 100% created by me.

public class EndPortal : MonoBehaviour
{
    MainMenu mainMenuScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mainMenuScript = FindObjectOfType<MainMenu>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CharacterPlayer(Clone)")
        {
            if (mainMenuScript.getNormalSelected())
            {
                Debug.Log("Normal");
                mainMenuScript.StartGameNormal();
            }
            if (mainMenuScript.getHardSelected())
            {
                Debug.Log("Hard");
                mainMenuScript.StartGameHard();
            }
        }
        if (other.gameObject.name == "CharacterPlayerVR(Clone)")
        {
            
            if (mainMenuScript.getVRSelected())
            {
                Debug.Log("VR");
                mainMenuScript.StartVRMode();
            }       
        }
    }
}
