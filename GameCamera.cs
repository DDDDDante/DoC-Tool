using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{

    public GameObject gridPlane;
    public float gridPlaneY;
    public float moveSpeed;
    public float cameraHeight;// Camera from terrain value
    public float cameraY; // Where I want the camera to end up
    public float maxCameraHeight;
    public float minCameraHeight;
    private float desiredPositon;
    private float desiredGridPosition;
    public float gridX;
    public float gridZ;
    public float zoom = 18f;

    // Use this for initialization
    void Start()
    {
        cameraY = transform.position.y;

        gridPlaneY = gridPlane.transform.position.y;

        //Grid X && Z Position
        gridX = gridPlane.transform.position.x;
        gridZ = gridPlane.transform.position.z;

    }


    void Update()
    {
        Vector3 pos = transform.position;
        //Camera Movement
        if (Input.GetKey("w"))
        {
            pos.z += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        transform.position = pos;

        //Rotation Script set to q & e

        //Camera Zoom



        //Raycast to measure distance between camera and Terrain
        //void LateUpdate() ??? Lack of LateUpdate causes constant re-do every moment the camera moves aka rapidfire particle shooting and cameraY shaking
        // errors on void LateUpdate () check documentaion for updates
        {

            RaycastHit hit;
            Ray cameraRay = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(cameraRay, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Environment")
                {
                    float rayDistance = cameraY - hit.point.y;
                    this.transform.position = new Vector3(transform.position.x, cameraY, transform.position.z);
                    desiredPositon = hit.point.y + cameraHeight;
                    desiredGridPosition = desiredPositon - cameraHeight + 0.1F;

                    if (rayDistance < desiredPositon)
                    {
                        transform.position = new Vector3(transform.position.x, desiredPositon, transform.position.z);
                        gridPlane.transform.position = new Vector3(gridX, desiredGridPosition, gridZ);
                    }
                    if (rayDistance > desiredPositon)
                    {
                        transform.position = new Vector3(transform.position.x, desiredPositon, transform.position.z);
                        gridPlane.transform.position = new Vector3(gridX, desiredGridPosition, gridZ);
                    }

                    //if (cameraY > maxCameraHeight)
                    //return;

                    //if (cameraY < minCameraHeight)
                    // return;
                }

                if (cameraY > maxCameraHeight)
                return;

                 if (cameraY < minCameraHeight)
                return;

                //smoothdamp
                float smoothTime = 0.2f;
                float yVelocity = 0f;

                float newPositionY = Mathf.SmoothDamp(transform.position.y, desiredPositon, ref yVelocity, smoothTime);
                transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);

                //Grid Y adjustment

                float newGridPlaneY = Mathf.SmoothDamp(transform.position.y, desiredGridPosition, ref yVelocity, smoothTime);
                transform.position = new Vector3(transform.position.x, newGridPlaneY, transform.position.z);
            }


            Debug.DrawRay(this.transform.position, Vector3.down * 1000, Color.red);

            //Free Camera Mode
        }
    }
}

