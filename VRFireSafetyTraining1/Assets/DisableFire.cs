using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFire : MonoBehaviour {
    public int counter=5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (counter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Bullet"))
        {
            counter--;
        }
        
    }
}
