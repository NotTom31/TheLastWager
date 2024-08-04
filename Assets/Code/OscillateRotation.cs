using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateRotation : MonoBehaviour
{
    public float amplitude = 15f;
    public float speed = 1f;
    public float syncTime = 1f;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    bool bop = false;
    private void Start()
    {
        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        float oscillationX = amplitude * Mathf.Sin(Time.time * speed);
        float oscillationY = amplitude * Mathf.Cos(Time.time * speed);
        float musicSync = amplitude * 0.02f * Mathf.Sin(Time.time * syncTime * Mathf.PI * 4f);

        Quaternion newRotation = Quaternion.Euler(oscillationX, oscillationY, 0);
        Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y+musicSync, initialPosition.z);

        if (bop)
        {
            transform.position = newPosition;
        }
        transform.rotation = initialRotation * newRotation;
    }
}
