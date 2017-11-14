using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //type for camera movement
    public enum eCameraState
    {
        FREE,
        TARGET,
    }

    //delegate type
    public delegate void VoidFunc();

    //reference to the camera
    public Camera cam = null;

    //instance of the custom type
    private eCameraState m_cameraState = eCameraState.FREE;

    //camera parameters
    public float distance = 0.0f;
    public float FOV = 0.0f;

    [Range(0, 90)]
    public float cameraAngle = 0.0f;

    public float cameraSpeed = 5.0f;

    [Range(0.0f, 5.0f)]
    public float smoothingSpeed = 0.0f;
    
    [Range(0.0f, 2.0f)]
    public float transitionTime = 0.0f;
    public AnimationCurve transitionCurve = null;

    //focal movement limits
    public Rect limits;

    //automated references
    private CustomInput input = null;

    //----FREE MOVEMENT VARIABLES----

    //smoothed vector
    private Vector2 m_smoothInput = Vector2.zero;

    //----TARGET MOVEMENT VARIABLES----

    //3D target vector
    private Vector3 m_start = Vector3.zero;
    private Vector3 m_target = Vector3.zero;

    private Vector3 m_eulerStart = Vector3.zero;
    private Vector3 m_eulerEnd = Vector3.zero;
    private float m_movementTimer = 0.0f;

    //callback to invoke when the target is reached
    private VoidFunc m_targetCallback = null;

    // Use this for initialization
    void Start ()
    {
        input = GameObject.FindObjectOfType<CustomInput>();
	}


    /*
    * ClampPosition 
    * 
    * sets the transform of the camera to be inside the limits rect
    * 
    * @returns void
    */
    void ClampPosition()
    {
        if (transform.position.x < limits.x)
        {
            transform.position = new Vector3(limits.x, transform.position.y, transform.position.z);
            m_smoothInput.x = 0.0f;
        }

        if (transform.position.x > limits.x + limits.width)
        {
            transform.position = new Vector3(limits.x + limits.width, transform.position.y, transform.position.z);
            m_smoothInput.x = 0.0f;
        }

        if (transform.position.z < limits.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limits.y);
            m_smoothInput.y = 0.0f;
        }

        if (transform.position.z > limits.y + limits.height)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limits.y + limits.height);
            m_smoothInput.y = 0.0f;
        }

    }


    // Update is called once per frame
    void Update ()
    {
        //set the distance through the local z of the camera because it is parented to this object
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, distance, cam.transform.localPosition.z);

        cam.fieldOfView = FOV;
        transform.eulerAngles = new Vector3(cameraAngle - 90.0f, transform.eulerAngles.y, transform.eulerAngles.z);

        //call the appropriate sub-update
        switch (m_cameraState)
        {
            case eCameraState.FREE: UpdateFreeMovement(); break;
            case eCameraState.TARGET: UpdateTargetMovement(); break;
        }
    }


    /*
    * UpdateFreeMovement 
    * 
    * sub-update that gets called when the camera is in FREE mode
    * 
    * @returns void
    */
    public void UpdateFreeMovement()
    {
        //smooth the input
        Vector2 relative = input.keyInput - m_smoothInput;

        if (relative.magnitude < smoothingSpeed * Time.deltaTime)
        {
            m_smoothInput = input.keyInput;
        }
        else
        {
            m_smoothInput += relative.normalized * smoothingSpeed * Time.deltaTime;
        }

        //trig calculations
        float sn = Mathf.Sin(Mathf.Deg2Rad * -transform.eulerAngles.y);
        float cs = Mathf.Cos(Mathf.Deg2Rad * -transform.eulerAngles.y);

        //rotate the vector by the camera's y rotation
        Vector2 rotatedInput = new Vector2(cs * m_smoothInput.x - sn * m_smoothInput.y, sn * m_smoothInput.x + cs * m_smoothInput.y);

        //moves the camera if input is given and snaps the y to 0
        transform.position = new Vector3(transform.position.x + rotatedInput.x * cameraSpeed * Time.deltaTime, 0.0f,
            transform.position.z + rotatedInput.y * cameraSpeed * Time.deltaTime);

        ClampPosition();
    }


    /*
    * UpdateFreeMovement 
    * 
    * sub-update that gets called when the camera is in TARGET mode
    * 
    * @returns void
    */
    public void UpdateTargetMovement()
    {
        m_movementTimer += Time.deltaTime;

        //the timer cannot tick over the maximum
        if (m_movementTimer >= transitionTime)
        {
            //set the timer above the limit
            m_movementTimer = transitionTime + 0.01f;

            //snap to the target
            transform.position = m_target;
            transform.eulerAngles = m_eulerEnd;
        }
        else
        {
            //evaluate the time curve
            float x = m_movementTimer / transitionTime;
            float y = transitionCurve.Evaluate(x);

            //apply a simple lerp to the position
           transform.position = Vector3.Lerp(m_start, m_target, y);
           //transform.eulerAngles = Vector3.Lerp(m_eulerStart, m_eulerEnd, y);
        }

        //check if the movement has finished
        if (m_movementTimer >= transitionTime)
        {
            //set the camera state back to free movement
            m_cameraState = eCameraState.FREE;

            //only invoke the call-back if one was specified
            if (m_targetCallback != null)
            {
                m_targetCallback.Invoke();
            }

        }
    }


    /*
    * Goto 
    * 
    * sets up the camera so that it moves towards
    * a target in future update calls
    * 
    * @param Vector3 target - the position to move towards
    * @param Vector3 targetEuler - the euler angle target
    * @param VoidFunc callback - function to call when the target is reached
    * @returns void
    */
    public void Goto(Vector3 target, Vector3 targetEuler, VoidFunc callback)
    {
        //remember the parameters
        m_cameraState = eCameraState.TARGET;
        m_targetCallback = callback;

        //set the lerp positions and euler angles
        m_start = transform.position;
        m_target = target;
        
        m_eulerStart = transform.eulerAngles;
        m_eulerEnd = targetEuler;

        //reset the input
        m_smoothInput = Vector2.zero;

        m_movementTimer = 0.0f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
