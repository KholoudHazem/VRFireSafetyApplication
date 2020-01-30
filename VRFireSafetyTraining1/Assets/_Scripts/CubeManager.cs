using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CubeManager : MonoBehaviourPun {

	public float horizontalInput;
	private bool mine = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		horizontalInput = Input.GetAxis("Horizontal");
		if(horizontalInput != 0)
		{
			if(mine)
			{
				this.photonView.transform.localPosition += new Vector3(0,0,horizontalInput);
			}
		}
	}
	void OnMouseDown()
	{
        // this object was clicked - do something
		Debug.Log("Cube is clicked");
		if(mine)
		{
			Debug.Log("transfer back to scene");
			this.photonView.TransferOwnership(0);
			mine = false;
		}
		else if(this.photonView.Owner == null)
		{
			Debug.Log("transfere to player since the scene owned the object");
			this.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
			mine = true;
		}
		
		
		/*
		
		
		
		 */
     //GetComponent<Renderer> ().material.color = Color.green;
  }   
}
