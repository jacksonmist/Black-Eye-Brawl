using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    Transform mainCamera;
    public Transform cameraTarget;

    [SerializeField] float cameraSpeed;
    void Start()
    {
        mainCamera = transform;
        mainCamera.position = cameraTarget.position;

    }

    void Update()
    {
        LerpCamera();
    }

    void LerpCamera()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, cameraSpeed * Time.deltaTime);
    }
}
