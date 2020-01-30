using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
     

            RaycastHit raycastHit;
            GameObject gameObject = null;
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit))
            {
                gameObject = raycastHit.collider.gameObject;

                //Do sth. with the found GameObject here

                
            }
    }

    
}
