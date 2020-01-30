using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hintsController : MonoBehaviour {

    public Canvas hintsCanvas;
    public GameObject AlarmButton;
    private GameObject nozel;
    // use it as index for the current step
    private int counter=0;
    // to check if current step is already executed
    private bool green=false;
    private bool white = false;

    void Start()
    {
        InvokeRepeating("flashing", 0.3f, 1);
    }

    // Update is called once per frame
    void Update () {
       

        // make sure that counter won't exceed the number of steps
        if (counter< (hintsCanvas.GetComponentsInChildren<Text>().Length))
        {
            // get the currnt value
            green = hintsCanvas.GetComponentsInChildren<Text>()[counter].color == Color.green;
            //flashing();
        }
        else
        {
            CancelInvoke();
        }

        // search for the nozel each frame
        nozel = GameObject.FindGameObjectWithTag("fireExtNozel");

        // if alarm is on change color of 1st step's text to green
        if (AlarmButton.GetComponent<AudioSource>().isPlaying)
        {
            hintsCanvas.GetComponentsInChildren<Text>()[0].color = Color.green;

        }
        // if fire extinguisher audio is on change color of 5th step's text to green
        if ((nozel != null) && (nozel.GetComponent<AudioSource>().isPlaying))
        {
            hintsCanvas.GetComponentsInChildren<Text>()[4].color = Color.green;
        }
        // if there is no fire object anymore change color of last step's text to green
        if (!(GameObject.FindGameObjectWithTag("FireC") != null))
        {
            hintsCanvas.GetComponentsInChildren<Text>()[5].color = Color.green;
        }
      
	}

    // once fire extinguisher is picked up change color of 2nd step's text to green
    // activated on pick up event of fireExtPiv3
    public void fireExtPickedUp()
    {
        hintsCanvas.GetComponentsInChildren<Text>()[1].color = Color.green;
    }
    // once pin is picked up change color of 3rd step's text to green
    // activated on pick up event of Pin, A Child of fireExtPiv3 gameobject
    public void PinPickedUp()
    {
        hintsCanvas.GetComponentsInChildren<Text>()[2].color = Color.green;
    }
    // once hose is picked up change color of 4th step's text to green
    // activated on pick up event of HosePickUp, A Child of fireExtPiv3 gameobject
    public void HosePickedUp()
    {
        hintsCanvas.GetComponentsInChildren<Text>()[3].color = Color.green;
    }

    public void flashing()
    {
        if (green)
        {
            // current step changed, reset everything
            counter++;
            green = false;
            white = false;
        }
        else
        {
            // if already white make it black
            if (white)
            {
                hintsCanvas.GetComponentsInChildren<Text>()[counter].color = Color.black;
                white = false;
            }
            // otherwise make it white
            else
            {
                hintsCanvas.GetComponentsInChildren<Text>()[counter].color = Color.white;
                white = true;
            }
        }
    }


}
