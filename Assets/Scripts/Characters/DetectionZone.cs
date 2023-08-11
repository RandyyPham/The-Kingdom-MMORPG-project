using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private string tagTarget;
    public List<GameObject> DetectedObjects { get; set; } = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(tagTarget))
        {
            DetectedObjects.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(tagTarget))
        {
            DetectedObjects.Remove(collider.gameObject);
        }
    }
}
