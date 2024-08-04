using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateRotation : MonoBehaviour
{
    public float amplitude = 15f;
    public float speed = 1f;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        float oscillationX = amplitude * Mathf.Sin(Time.time * speed);
        float oscillationY = amplitude * Mathf.Cos(Time.time * speed);

        Quaternion newRotation = Quaternion.Euler(oscillationX, oscillationY, 0);

        transform.rotation = initialRotation * newRotation;
    }
}
