using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    private float interpVelocity;
    public GameObject target;
    public float maxY;
    public float minY;
    public float maxX;
    public float minX;
    public float buildCamSize = 15;
    public float playCamSize = 15;
    public float panSpeed = 4.0f;		// Speed of the camera when being panned
    public bool isBuildPhase;
    private Vector2 worldUnitsInCamera;
    private bool camHasPanned = false;
    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
    private bool isPanning;		// Is the camera being panned?

    Vector3 targetPos;
    Vector3 mousePos;

    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
        worldUnitsInCamera.y = Camera.main.orthographicSize;
        worldUnitsInCamera.x = worldUnitsInCamera.y * (Camera.main.pixelRect.width / Camera.main.pixelRect.height);

    }

    public void ChangeCamToBuild()
    {
        if (target)
        {
            targetPos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
            Camera.main.transform.position = targetPos;
            Camera.main.orthographicSize = buildCamSize;
        }
    }

    public void ChangeCamToPlay()
    {
        if (target)
        {
            camHasPanned = false;
            Camera.main.orthographicSize = playCamSize;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            if (isBuildPhase)
            {
                float camSizeDifference = buildCamSize - playCamSize;

                if (Input.GetMouseButtonDown(1))
                {
                    // Get mouse origin
                    mouseOrigin = Input.mousePosition;
                    isPanning = true;
                }
                else if (Input.GetMouseButtonUp(1))
                    isPanning = false;
                if (isPanning)
                {
                    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

                    Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
                    transform.Translate(move, Space.Self);
                    transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX - camSizeDifference, maxX + camSizeDifference),
                    Mathf.Clamp(transform.position.y, minY - camSizeDifference, maxY + camSizeDifference), -10f);
                }


            }
            else
            {
                if (!camHasPanned)
                {
                    Vector3 posNoZ = transform.position;
                    posNoZ.z = target.transform.position.z;

                    Vector3 targetDirection = (target.transform.position - posNoZ);

                    interpVelocity = targetDirection.magnitude * 1.1f;

                    targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

                    transform.position = Vector3.Lerp(transform.position, targetPos, 20f);
                    transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX, maxX),
                    Mathf.Clamp(transform.position.y, minY, maxY), -10f);
                    camHasPanned = true;
                }
                else
                {
                    Vector3 posNoZ = transform.position;
                    posNoZ.z = target.transform.position.z;

                    Vector3 targetDirection = (target.transform.position - posNoZ);

                    interpVelocity = targetDirection.magnitude * 5f;

                    targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

                    transform.position = Vector3.Lerp(transform.position, targetPos, 0.75f);
                    transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, minX, maxX),
                    Mathf.Clamp(transform.position.y, minY, maxY), -10f);
                }
            }
        }

    }
}