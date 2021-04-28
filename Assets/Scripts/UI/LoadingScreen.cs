using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Entire code present in this script has been produced 100% by me.
public class LoadingScreen : MonoBehaviour
{
    public LevelBuilder levelBuilder;
    public Canvas HUD;
    // Start is called before the first frame update
    void Start()
    {
        levelBuilder = FindObjectOfType<LevelBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelBuilder.hasRoomFinished())
        {
            HUD.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
