using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
/*#if UNITY_IOS || UNITY_ANDROID



#endif*/
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Vector3 newZoom;
    public Quaternion newRotation;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    public List<Transform> moveLimits;
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        HandleMovemntImput();
        HandleMouseInput();
        
        #if UNITY_IOS || UNITY_ANDROID
            HandleTouchInput();
        #endif
    }
    //For touch input
    void HandleTouchInput()
    {
        if (Input.touchCount == 2 && Input.GetTouch(0).phase != TouchPhase.Stationary && Input.GetTouch(1).phase != TouchPhase.Stationary)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector3 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector3 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float diffrence = currentMagnitude - prevMagnitude;
            newZoom += new Vector3(0, diffrence * zoomAmount.y / 10, diffrence * zoomAmount.z / 10);
        }
        if (Input.touchCount >= 2)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                rotateStartPosition = Input.GetTouch(0).position;
            }
            if (Input.GetTouch(1).phase == TouchPhase.Stationary)
            {
                rotateStartPosition = Input.GetTouch(0).position;
            }
        }
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                rotateStartPosition = Input.GetTouch(0).position;
            }
        }

        if (Input.touchCount == 2 && ((Input.GetTouch(0).phase != TouchPhase.Stationary && Input.GetTouch(1).phase == TouchPhase.Stationary) || (Input.GetTouch(0).phase == TouchPhase.Stationary && Input.GetTouch(1).phase != TouchPhase.Stationary)))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                //rotateStartPosition = Input.GetTouch(0).position;
                rotateCurrentPosition = Input.GetTouch(1).position;
                Vector3 difference = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition;
                if (difference.x > 0)
                {
                    newRotation *= Quaternion.Euler(Vector3.up * (difference.x / 500f));
                }
                else
                {
                    newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 500f));
                }
            }
            if (Input.GetTouch(1).phase == TouchPhase.Stationary)
            {
                //rotateStartPosition = Input.GetTouch(0).position;
                rotateCurrentPosition = Input.GetTouch(0).position;
                Vector3 difference = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition;
                if (difference.x < 0)
                {
                    newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 500f));
                }
                else
                {
                    newRotation *= Quaternion.Euler(Vector3.up * (difference.x / 500f));
                }
            }

        }
    }
    //For mouse input
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            
            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
        
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount * 5;
        }



        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }     
        


    }
    void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }
    }
    //For move the camera
    void HandleMovemntImput()
    {
        //Applys change amount to transform
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

        //Checking for map limits
        if (transform.position.x < moveLimits[0].position.x)
        {
            newPosition.x = moveLimits[0].position.x;
        }
        if (transform.position.x > moveLimits[1].position.x)
        {
            newPosition.x = moveLimits[1].position.x;
        }
        if (transform.position.z > moveLimits[2].position.z)
        {
            newPosition.z = moveLimits[2].position.z;
        }
        if (transform.position.z < moveLimits[3].position.z)
        {
            newPosition.z = moveLimits[3].position.z;
        }

        if (moveLimits[4].localPosition.y >= cameraTransform.localPosition.y)
        {
            newZoom = new Vector3(cameraTransform.localPosition.x, moveLimits[4].localPosition.y + 10, moveLimits[4].localPosition.z - 10);
        }
        else if (moveLimits[5].localPosition.y <= cameraTransform.localPosition.y)
        {
            newZoom = new Vector3(cameraTransform.localPosition.x, moveLimits[5].localPosition.y - 10, moveLimits[5].localPosition.z + 10);
        }
        
    }
}
