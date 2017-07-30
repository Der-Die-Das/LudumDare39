using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysRotate : MonoBehaviour
{
    public Vector3 Axis;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.AngleAxis(Axis.magnitude, Axis);
        transform.rotation = Quaternion.Lerp(startRotation, startRotation * endRotation, Time.fixedDeltaTime);

        //transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + (Axis * Time.deltaTime));
    }
}
