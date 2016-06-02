using UnityEngine;
using System.Collections;

public class GameCam : MonoBehaviour 
{
    public float scrollSpeed = 5f;
    public Vector3 startPosition;

    float currentZoom = 0f;
    public float zoomSpeed = 1f;

    public float MinZoomRange = -5;
    public float MaxZoomRange = 5;

    public int Border;

    public enum TargetAxis
    {
        X,
        Y,
        Z
    }

    /// <summary>
    /// Oś po której kamera będzie asdasię przybliżać
    /// (na ogół jest to równoległa oś do osi Scrolla)
    /// </summary>
    public TargetAxis ZoomAxis;
    public TargetAxis ScrollAxis;

    [SerializeField]
    Transform Player;

    Transform target;

    void Start()
    {
        startPosition = transform.position;
    }
    public void Call(Transform t)
    {
        target = t;
        if (ScrollAxis == TargetAxis.X)
        {
            transform.position = new Vector3(t.position.x, t.position.y, transform.position.z);
        }
    }

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, MinZoomRange, MaxZoomRange);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ScrollAxis == TargetAxis.X)
            {
                transform.position = new Vector3(Player.position.x, Player.position.y, transform.position.z);
            }
        }

        if (ZoomAxis == TargetAxis.X)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                transform.position -= new Vector3(transform.position.x - (startPosition.x + currentZoom), 0, 0);
            }
        }
        else if (ZoomAxis == TargetAxis.Y)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                transform.position -= new Vector3(0, transform.position.y - (startPosition.y + currentZoom), 0);
            }
        }
        else if (ZoomAxis == TargetAxis.Z)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                transform.position -= new Vector3(0, 0, transform.position.z - (startPosition.z + currentZoom));
            }
        }

        if (Input.mousePosition.x > Screen.width - Border) //Right Border
        {
            if (ScrollAxis == TargetAxis.X)
            {
                transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
            }
            else if (ScrollAxis == TargetAxis.Y)
            {
                transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
            }
            else if (ScrollAxis == TargetAxis.Z)
            {
                transform.position += new Vector3(0, 0, scrollSpeed * Time.deltaTime);
            }
        }

        if (Input.mousePosition.x < Border) //Left Border
        {
            if (ScrollAxis == TargetAxis.X)
            {
                transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
            }
            else if (ScrollAxis == TargetAxis.Y)
            {
                transform.position -= new Vector3(0, scrollSpeed * Time.deltaTime, 0);
            }
            else if (ScrollAxis == TargetAxis.Z)
            {
                transform.position -= new Vector3(0, 0, scrollSpeed * Time.deltaTime);
            }
        }

        if (Input.mousePosition.y > Screen.height - Border) //Top Border
        {
            if (ScrollAxis == TargetAxis.X)
            {
                transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
            }
            else if (ScrollAxis == TargetAxis.Y)
            {
                transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
            }
            else if (ScrollAxis == TargetAxis.Z)
            {
                transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
            }
        }

        if (Input.mousePosition.y < Border) //Bottom Border
        {
            if (ScrollAxis == TargetAxis.X)
            {
                transform.position -= new Vector3(0, scrollSpeed * Time.deltaTime, 0);
            }
            else if (ScrollAxis == TargetAxis.Y)
            {
                transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
            }
            else if (ScrollAxis == TargetAxis.Z)
            {
                transform.position -= new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
