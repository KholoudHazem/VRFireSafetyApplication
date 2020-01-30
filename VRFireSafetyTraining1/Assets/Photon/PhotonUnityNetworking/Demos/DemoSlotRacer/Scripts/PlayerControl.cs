// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerControl.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in SlotRacer Demo
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

using System.Collections;

using Photon.Pun.Demo.SlotRacer.Utils;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.SlotRacer
{
    /// <summary>
    /// Player control. 
    /// Interface the User Inputs and PUN
    /// Handle the Car instance 
    /// </summary>
    [RequireComponent(typeof(SplineWalker))]
    public class PlayerControl : MonoBehaviourPun, IPunObservable
    {
        /// <summary>
        /// The car prefabs to pick depending on the grid position.
        /// </summary>
        public GameObject[] CarPrefabs;

        /// <summary>
        /// The maximum speed. Maximum speed is reached with a 1 unit per seconds acceleration
        /// </summary>
        public float MaximumSpeed = 20;

        /// <summary>
        /// The drag when user is not accelerating
        /// </summary>
        public float Drag = 5;

        /// <summary>
        /// The current speed.
        /// Only used for locaPlayer
        /// </summary>
        private float CurrentSpeed;

        /// <summary>
        /// The current distance on the spline
        /// Only used for locaPlayer
        /// </summary>
        private float CurrentDistance;

        private float ScaleFactor;

        private GameObject tree6;
        private Vector3 treeSize;
        /// <summary>
        /// The car instance.
        /// </summary>
        private GameObject CarInstance;

        /// <summary>
        /// The spline walker. Must be on this GameObject
        /// </summary>
        private SplineWalker SplineWalker;


        /// <summary>
        /// flag to force latest data to avoid initial drifts when player is instantiated.
        /// </summary>
        private bool m_firstTake = true;


        private float m_input;


        #region IPunObservable implementation

        /// <summary>
        /// this is where data is sent and received for this Component from the PUN Network.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="info">Info.</param>
        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // currently there is no strategy to improve on bandwidth, just passing the current distance and speed is enough, 
            // Input could be passed and then used to better control speed value
            //  Data could be wrapped as a vector2 or vector3 to save a couple of bytes
            if (stream.IsWriting)
            {
                //stream.SendNext(this.ScaleFactor);
                //stream.SendNext(this.tree6.transform);

                stream.SendNext(this.CurrentDistance);
                stream.SendNext(this.CurrentSpeed);
                stream.SendNext(this.m_input);
                if(this.m_input > 0.001)
                {
                    Debug.Log("sending input: " + this.m_input);
                    Debug.Log("sending speed: " + this.CurrentSpeed);
                    Debug.Log("sending distance: " + this.CurrentDistance);
                }
            }
            else
            {
                if (this.m_firstTake)
                {
                    this.m_firstTake = false;
                }
                
                //this.tree6.transform = (GameObject)stream.ReceiveNext();
                this.CurrentDistance = (float) stream.ReceiveNext();
                this.CurrentSpeed = (float) stream.ReceiveNext();
                this.m_input = (float) stream.ReceiveNext();
                
                //Debug.Log("recevie size " + this.tree6.transform);

                if(this.m_input > 0.001)
                {

                    Debug.Log("recieving input: " + this.m_input);
                    Debug.Log("recieving speed: " + this.CurrentSpeed);
                    Debug.Log("recieving distance: " + this.CurrentDistance);
                }
                    
            }
        }

        #endregion IPunObservable implementation


        #region private

        /// <summary>
        /// Setups the car on track.
        /// </summary>
        /// <param name="gridStartIndex">Grid start index.</param>
        private void SetupCarOnTrack(int gridStartIndex)
        {
            // Setup the SplineWalker to be on the right starting grid position.
            this.SplineWalker.spline = SlotLanes.Instance.GridPositions[gridStartIndex].Spline;
            this.SplineWalker.currentDistance = SlotLanes.Instance.GridPositions[gridStartIndex].currentDistance;
            this.SplineWalker.ExecutePositioning();

            // create a new car
            this.CarInstance = (GameObject) Instantiate(this.CarPrefabs[gridStartIndex], this.transform.position, this.transform.rotation);

            // We'll wait for the first serializatin to pass, else we'll have a glitch where the car is positioned at the wrong position.
            if (!this.photonView.IsMine)
            {
                this.CarInstance.SetActive(false);
            }

            // depending on wether we control this instance locally, we force the car to become active ( because when you are alone in the room, serialization doesn't happen, but still we want to allow the user to race around)
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                this.m_firstTake = false;
            }

            this.CarInstance.transform.SetParent(this.transform);
        }

        #endregion private


        #region Monobehaviour

        /// <summary>
        /// Cache the SplineWalker and flag context for clean serialization when joining late.
        /// </summary>
        private void Awake()
        {
            this.SplineWalker = this.GetComponent<SplineWalker>();
            this.m_firstTake = true;
        }

        /// <summary>
        /// Start this instance as a coroutine
        /// Waits for a Playernumber to be assigned and only then setup the car and put it on the right starting position on the lane.
        /// </summary>
        private IEnumerator Start()
        {
            tree6 = GameObject.FindGameObjectWithTag("tree6");
            // Wait until a Player Number is assigned
            // PlayerNumbering component must be in the scene.
            yield return new WaitUntil(() => this.photonView.Owner.GetPlayerNumber() >= 0);

            // now we can set it up.
            this.SetupCarOnTrack(this.photonView.Owner.GetPlayerNumber());
        }

        /// <summary>
        /// Make sure we delete instances linked to this component, else when user is leaving the room, its car instance would remain 
        /// </summary>
        private void OnDestroy()
        {
            Destroy(this.CarInstance);
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.SplineWalker == null || this.CarInstance == null)
            {
                return;
            }

            if (this.photonView.IsMine)
            {
                ScaleFactor = Input.GetAxis("Jump");
                if(ScaleFactor != 0)
                {
                    ScaleFactor =ScaleFactor * 0.1f;
                    treeSize = tree6.transform.localScale  + new Vector3(ScaleFactor,ScaleFactor,ScaleFactor);
                    Debug.Log("vector tree size: " + isPositiveVector(treeSize) + "\n value: " + treeSize);
                    if(isPositiveVector(treeSize))
                    {
                        tree6.transform.localScale =treeSize;
                    }
                    //Debug.Log(ScaleFactor);////////
                }
                
                this.m_input = Input.GetAxis("Horizontal");
                 if (this.m_input < 0f)
                {
                    Debug.Log(PhotonNetwork.PlayerListOthers.ToString());
                    //PhotonNetwork.PlayerListOthers[0];
                    //tree6.GetPhotonView().RequestOwnership();
                    //tree6.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
                    Debug.LogErrorFormat("the owner of the object is: " + this.photonView.Owner.UserId);

                    this.photonView.TransferOwnership(-1);

                    Debug.LogErrorFormat("the owner after user transfere of  object is: " + this.photonView.Owner.UserId);
                }
                this.m_input = Input.GetAxis("Vertical");
                if (this.m_input == 0f)
                {
                    this.CurrentSpeed -= Time.deltaTime * this.Drag;
                }
                else
                {
                    this.CurrentSpeed += this.m_input;
                }
                this.CurrentSpeed = Mathf.Clamp(this.CurrentSpeed, 0f, this.MaximumSpeed);
                this.SplineWalker.Speed = this.CurrentSpeed;

                this.CurrentDistance = this.SplineWalker.currentDistance;
            }
            else
            {
				if (this.m_input == 0f)
				{
					this.CurrentSpeed -= Time.deltaTime * this.Drag;
				}

				this.CurrentSpeed = Mathf.Clamp (this.CurrentSpeed, 0f, this.MaximumSpeed);
				this.SplineWalker.Speed = this.CurrentSpeed;



				if (this.CurrentDistance != 0 && this.SplineWalker.currentDistance != this.CurrentDistance)
				{
					//Debug.Log ("SplineWalker.currentDistance=" + SplineWalker.currentDistance + " CurrentDistance=" + CurrentDistance);
					this.SplineWalker.Speed += (this.CurrentDistance - this.SplineWalker.currentDistance) * Time.deltaTime * 50f;
				}

            }

            // Only activate the car if we are sure we have the proper positioning, else it will glitch visually during the initialisation process.
            if (!this.m_firstTake && !this.CarInstance.activeSelf)
            {
                this.CarInstance.SetActive(true);
				this.SplineWalker.Speed = this.CurrentSpeed;
				this.SplineWalker.SetPositionOnSpline (this.CurrentDistance);

            }
        }

        #endregion Monobehaviour
        
        public bool isPositiveVector(Vector3 v)
        {
            return (v.x > 0 && v.y > 0 && v.z > 0);
        }
        
    }
}