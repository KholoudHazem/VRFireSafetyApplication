using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleasePin : MonoBehaviour
{
    public GameObject pin;

    public void unFreezePin()
    {
        pin.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }


}
