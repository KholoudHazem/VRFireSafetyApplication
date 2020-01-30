using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FireManager : MonoBehaviour {

    // Use this for initialization
    public bool fireOn = true;
    public Canvas endScreen;
    public GameObject AlarmButton;
	void Start () {
        InvokeRepeating("checkFire", 1, 5);
        
    }

    // Update is called once per frame
    void Update () {
        
    }

    void checkFire()
    {
        fireOn = (GameObject.FindGameObjectWithTag("FireC") != null);
        if (!fireOn)
        {
            endScreen.GetComponent<Canvas>().enabled = true;
            endScreen.GetComponentInChildren<Text>().text = "Great Job!! \nyou finished in: " + System.TimeSpan.FromSeconds((int)Time.timeSinceLevelLoad).ToString();
            
            Debug.Log("end screen should be up!");
            Debug.Log("Real time "+Time.realtimeSinceStartup);
            this.GetComponent<AudioSource>().mute = true;
            AlarmButton.GetComponent<AudioSource>().mute = true;

            CancelInvoke();
        }

    }
}
