using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomZensibility;
    [SerializeField] Vector2 zoomLimits;

    [SerializeField] float moveSpeed;

    ScreenController screenController;
    bool draging;

    float targetZoom;

    private void Awake()
    {
        screenController = GetComponentInChildren<ScreenController>();
    }

    private void Update()
    {
        if (draging) Drag();
        else Move();


        //  if (Input.GetAxis("Mouse ScrollWheel") != 0)
        //  {
        //      Zoom(Input.GetAxis("Mouse ScrollWheel") * zoomZensibility * Time.deltaTime);
        //  }
        //
        //  transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, targetZoom, Time.deltaTime * zoomSpeed));
    }

    void Move()
    {
        float xKeys = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float yKeys = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 newPos = transform.position;

        newPos.x += xKeys;
        newPos.y += yKeys;

        transform.position = newPos;

        screenController.GetScreenSize();
    }

    void Drag()
    {
        float xMove = Input.GetAxisRaw("Mouse X");
        float yMove = Input.GetAxisRaw("Mouse Y");

        Vector3 newPos = transform.position;

        newPos.x -= xMove * CursorController.i.sensibility;
        newPos.y -= yMove * CursorController.i.sensibility;

        transform.position = newPos;

        screenController.GetScreenSize();
    }

    public void StartDrag()
    {
        draging = true;
    }
    public void EndDrag()
    {
        draging = false;
    }


    void Zoom(float value)
    {
        targetZoom += value;
        targetZoom = Mathf.Clamp(targetZoom, zoomLimits.x, zoomLimits.y);
    
    }
}


