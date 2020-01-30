using UnityEngine;
using Valve.VR;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 trackpad;
    private Vector3 moveDirection;
    private int GroundCount;
    private CapsuleCollider CapCollider;

    public SteamVR_Input_Sources MovementHand;//Set Hand To Get Input From
    public SteamVR_Action_Vector2 TrackpadAction;
    public SteamVR_Action_Boolean JumpAction;
    public float jumpHeight;
    public float MovementSpeed;
    public float Deadzone;//the Deadzone of the trackpad. used to prevent unwanted walking.
    public GameObject Head;
    public GameObject AxisHand;//Hand Controller GameObject
    public PhysicMaterial NoFrictionMaterial;
    public PhysicMaterial FrictionMaterial;
    private void Start()
    {
        CapCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        updateInput();
        updateCollider();
        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + AxisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;//get the angle of the touch and correct it for the rotation of the controller
        Rigidbody RBody = GetComponent<Rigidbody>();
        Vector3 velocity = new Vector3(0, 0, 0);
        if (trackpad.magnitude > Deadzone)
        {//make sure the touch isn't in the deadzone and we aren't going to fast.
            CapCollider.material = NoFrictionMaterial;
            velocity = moveDirection;
            if (JumpAction.GetStateDown(MovementHand) && GroundCount > 0)
            {
                float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * 9.81f);
                RBody.AddForce(0, jumpSpeed, 0, ForceMode.VelocityChange);
            }
            RBody.AddForce(velocity.x * MovementSpeed - RBody.velocity.x, 0, velocity.z * MovementSpeed - RBody.velocity.z, ForceMode.VelocityChange);

            Debug.Log("Velocity" + velocity);
            Debug.Log("Movement Direction:" + moveDirection);
        }
        else if (GroundCount > 0)
        {
            CapCollider.material = FrictionMaterial;
        }
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    private void updateCollider()
    {
        CapCollider.height = Head.transform.localPosition.y;
        CapCollider.center = new Vector3(Head.transform.localPosition.x, Head.transform.localPosition.y / 2, Head.transform.localPosition.z);
    }

    private void updateInput()
    {
        trackpad = TrackpadAction.GetAxis(MovementHand);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GroundCount++;
    }
    private void OnCollisionExit(Collision collision)
    {
        GroundCount--;
    }
}