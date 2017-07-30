using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float zoomSpeed = 15f;
    public Transform target;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translate = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        translate *= moveSpeed * Time.deltaTime;
        transform.parent.position += translate;

        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 10f * Time.deltaTime;

        Vector3 newPos = -Vector3.forward + (target.position - transform.position).normalized;
        if (newPos.y < 0 && transform.localPosition.y >= 100 && zoom < 0)
        {
            return;
        }
        transform.position += newPos * zoom;
    }
    void LateUpdate()
    {

        transform.LookAt(target);
    }

    //show borders to easy edit them

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawCube()
    //}
}
