using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class enableHose : MonoBehaviour {

	    // Use this for initialization
	    void Start () {
		
	    }

        public void AllowHose()
        {
            GameObject fireExtHose = GameObject.FindGameObjectWithTag("ExtHose");
            fireExtHose.GetComponent<SphereCollider>().enabled = true;
            fireExtHose.GetComponent<Interactable>().enabled = true;

        }
    }

}


