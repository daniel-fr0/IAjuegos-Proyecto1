using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Separation.targets.Add(GetComponent<Kinematic>());
    }
}
