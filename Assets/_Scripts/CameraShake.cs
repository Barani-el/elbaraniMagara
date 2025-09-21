using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin noise;

    float shakeTimer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float time)
    {
        noise.AmplitudeGain = intensity;
        shakeTimer = time;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                noise.AmplitudeGain = 0f; 
            }
        }
    }
}
