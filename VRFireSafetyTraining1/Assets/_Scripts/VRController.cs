using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRController : MonoBehaviour {
    public float m_sensitvity=0.1f;
    public float m_MaxSpeed=1.0f;

    public SteamVR_Action_Boolean m_MovePress = null;
    public SteamVR_Action_Vector2 m_MoveValue = null;
    private float m_speed = 0.0f;
    private CharacterController m_CharacterController = null;

    public Transform m_CameraRig = null;
    public Transform m_Head = null;

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();

    }
    // Use this for initialization
    private void Start () {
        //m_CameraRig = SteamVR_Render.Top().origin;
        //m_Head = SteamVR_Render.Top().head;
	}
	
	// Update is called once per frame
	private void Update () {

       // HandelHead();
        HandelHight();
        CalculateMovment();
	}
    private void HandelHead()
    {
        // store current
        Vector3 oldPosition = m_CameraRig.position;
        Quaternion oldRotation = m_CameraRig.rotation;

        // rotation
        transform.eulerAngles = new Vector3(0.0f, m_Head.rotation.eulerAngles.y, 0.0f);
        // restore
        m_CameraRig.position = oldPosition;
        m_CameraRig.rotation = oldRotation;
    }
    private void CalculateMovment()
    {

        // figure movement orintiation 
        Vector3 OrintationEuler = new Vector3(0, transform.eulerAngles.y, 0);
        Quaternion orinatation = Quaternion.Euler(OrintationEuler);
        Vector3 movement = Vector3.zero;

        //if not moving
        if (m_MovePress.GetStateUp(SteamVR_Input_Sources.Any))
        {
            m_speed = 0;
        }
        // if button pressed
        if (m_MovePress.state)
        {
            // Add, Clamp
            m_speed += m_MoveValue.axis.y * m_sensitvity;
            m_speed = Mathf.Clamp(m_speed, -m_MaxSpeed, m_MaxSpeed);

            // Orintation
            movement += orinatation * (m_speed * Vector3.forward) * Time.deltaTime;
        }

        // Apply
        m_CharacterController.Move(movement);
    }
    private void HandelHight()
    {
        // Get the head in local space 
        float headHeight = Mathf.Clamp(m_Head.localPosition.y, 1, 2);
        m_CharacterController.height = headHeight;

        // Cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = m_CharacterController.height / 2;
        newCenter.y += m_CharacterController.skinWidth;

        // Move capsule in local space
        newCenter.x = m_Head.localPosition.x;
        newCenter.z = m_Head.localPosition.z;

        // Rotate
        newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;
        // Apply
        m_CharacterController.center = newCenter;

    }
}
