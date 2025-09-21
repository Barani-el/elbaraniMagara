using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [SerializeField] CinemachineCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin noise;

    float shakeTimer;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float intensity, float freq,float time)
    {
        noise.AmplitudeGain = intensity;
        shakeTimer = time;
        noise.FrequencyGain = freq;
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
