using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] private RectTransform crosshair;  // Drag your crosshair UI Image here
    [SerializeField] private float normalSize = 100f;    // Default size
    [SerializeField] private float expandedSize = 170f;  // Size when shooting
    [SerializeField] private float expandSpeed = 10f; // Time before shrinking back

    private Coroutine currentRoutine;

    /// Call this method whenever the player shoots.
    public void ExpandCrosshair()
    {
        // Stop previous expansion if it's still running
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ExpandRoutine()); 
    }

    private IEnumerator ExpandRoutine()
    {
        // Smoothly expand to expandedSize
        while (crosshair.sizeDelta.x < expandedSize)
        {
            float newSize = Mathf.MoveTowards(crosshair.sizeDelta.x, expandedSize, expandSpeed);
            crosshair.sizeDelta = new Vector2(newSize, newSize);
            yield return null;
        }

        // Smoothly shrink back to normalSize
        while (crosshair.sizeDelta.x > normalSize)
        {
            float newSize = Mathf.MoveTowards(crosshair.sizeDelta.x, normalSize, expandSpeed);
            crosshair.sizeDelta = new Vector2(newSize, newSize); // 
            yield return null;
        }

        currentRoutine = null;
    }
}