using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateRotation : MonoBehaviour
{
    public float amplitude = 15f;
    public float speed = 1f;
    public float syncTime = 0.5f;
    public bool bumpin = false;
    float musicSync = 0f;

    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private void Start()
    {
        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        float oscillationX = amplitude * Mathf.Sin(Time.time * speed);
        float oscillationY = amplitude * Mathf.Cos(Time.time * speed);
        musicSync += Time.deltaTime*2f;
        if (musicSync >= syncTime)
        {
            musicSync -= syncTime;
        }
        //float musicSync = amplitude * 0.02f * Mathf.Pow(Mathf.Sin(Time.time * syncTime * Mathf.PI * 4f), 7f);
        //Mathf.Sin(Time.time * syncTime * Mathf.PI * 4f));
        //float musicSync = amplitude * 0.02f * (Mathf.Pow(Mathf.Sin(Time.time * syncTime * Mathf.PI * 4f), 2f));

        Quaternion newRotation = Quaternion.Euler(oscillationX, oscillationY, 0);
        Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y+musicSync*amplitude*0.1f, initialPosition.z);

        if (bumpin)
        {
            transform.position = newPosition;
        }
        transform.rotation = initialRotation * newRotation;
    }
}
