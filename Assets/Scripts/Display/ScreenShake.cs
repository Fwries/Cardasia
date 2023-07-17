using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public float duration = 0.2f;
    public int strength;

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    public void Shake(int _strength)
    {
        strength = _strength * 10;
        StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 StartPos = this.transform.position;
        Vector3 ShakePos;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            ShakePos = Random.insideUnitSphere * strength / 100;
            transform.position += ShakePos;
            yield return null;
        }

        transform.position = StartPos;
    }
}
