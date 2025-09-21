using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxFactor; // 0 = sabit, 1 = kamera ile ayný hýz

    Transform cam;
    Vector3 lastCamPos;

    void Start()
    {
        cam = Camera.main.transform;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - lastCamPos;
        transform.position += delta * parallaxFactor;
        lastCamPos = cam.position;
    }
}