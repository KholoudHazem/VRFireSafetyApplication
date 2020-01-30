using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour {

    public SteamVR_ActionSet m_ActionSet;

    public SteamVR_Action_Boolean m_BooleanAction;
    public AudioClip ExtClip;
    public AudioSource ClipSource;
    public bool particleSystemActiviated = false;
    public GameObject bullet;
    public bool shooting = false;
    public float speed = 1000f;

    private void Awake()
    {
        m_BooleanAction = SteamVR_Actions._default.GrabPinch;

        #region Events
        m_BooleanAction[SteamVR_Input_Sources.Any].onStateDown += BoolTest;
        #endregion
    }

    // Use this for initialization
    void Start ()
    {
        m_ActionSet.Activate(SteamVR_Input_Sources.Any, 0, true);
        ClipSource.clip = ExtClip;
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region Boolean Action
        if(m_BooleanAction.stateDown)
        {
            Debug.Log("Pinch /clicked!!!");
            particleSystemActiviated = true;
            toggleParticleSystem();
            
            InvokeRepeating("shootBullets", 0, 1);

        }
        if (m_BooleanAction.stateUp)
        {
            Debug.Log("released!!!");
            particleSystemActiviated = false;
            toggleParticleSystem();
            
            CancelInvoke();

        }

        #endregion
    }
    public void shootBullets()
    {
        GameObject hiddenBullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        Rigidbody instBulletRigidbody = hiddenBullet.GetComponent<Rigidbody>();
        instBulletRigidbody.AddForce(-transform.forward * speed);
    }
    public void toggleParticleSystem()
    {
        if (particleSystemActiviated)
        {
            this.GetComponentInChildren<ParticleSystem>().Play();
            //this.GetComponent<AudioSource>().Play();
            Debug.Log("sound play!");
            ClipSource.Play();
        }
        else
        {
            this.GetComponentInChildren<ParticleSystem>().Stop();
           //this.GetComponent<AudioSource>().Stop();
            ClipSource.Stop();

        }
    }

    private void BoolTest(SteamVR_Action_Boolean action, SteamVR_Input_Sources source)
    {

    }
}
