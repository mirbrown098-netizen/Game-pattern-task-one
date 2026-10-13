
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(10000)]
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private float strength = 0.12f;

    private Coroutine shakeRoutine;
    private Vector3 shakeOffset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            enabled = false;
            return;
        }

        Instance = this;
    }

    public void Shake()
    {
        if (!isActiveAndEnabled)
            return;

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float fade = 1f - elapsed / duration;

            shakeOffset =
                Random.insideUnitSphere * strength * fade;

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
        shakeRoutine = null;
    }

    private void LateUpdate()
    {
        if (shakeOffset != Vector3.zero)
        {
            transform.position += shakeOffset;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
