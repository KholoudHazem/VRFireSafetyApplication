using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBullet : MonoBehaviour {
    private float timer = 0.0f;
    public float bulletLifeTime = 0.5f;
	// Use this for initialization
	void Start () {
		
        
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > bulletLifeTime)
        {
            timer = 0.0f;
            Destroy(this.gameObject);

        }
	}
}
