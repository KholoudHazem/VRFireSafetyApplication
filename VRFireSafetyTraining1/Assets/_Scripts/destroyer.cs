using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour {

    public GameObject go;

	public void dest()
    {
        Destroy(go);
    }
}
