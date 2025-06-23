using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.05f;

    public List<AudioSource> audios;

    private Vector3 initialPosition;

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        initialPosition = transform.localPosition;

        float elapsed = 0f;

        AudioSource audioSource = audios[Random.Range(0, audios.Count)];

        audioSource.Play();

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = initialPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}