using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMover : MonoBehaviour
{
    // Frequency of the hovering motion
    [SerializeField] private float freq = 0.6f;
    // Amplitude of the hovering motion
    [SerializeField] private float amplitude = 0.2f;

    // Offset to make each item's motion unique
    private float floatOffset;
    private Vector3 startPos;
    private Vector3 currentPos;

    void Start()
    {
        // Generate a random offset to vary the motion
        floatOffset = Random.Range(0, 180);
        startPos = transform.position;
    }

    void Update()
    {
        Hover();
    }

    // Calculate the new Y position using a sine wave to create the hovering effect
    private void Hover()
    {
        currentPos = startPos;
        currentPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * freq + floatOffset) * amplitude;
        transform.position = currentPos;
    }
}