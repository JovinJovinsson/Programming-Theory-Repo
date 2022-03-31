using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] GameObject fillBar;
    
    private void Awake()
    {
        AlignCamera();
    }

    /// <summary>
    /// Ensure the HealthBar is always looking at the camera
    /// </summary>
    private void AlignCamera()
    {
        // Just to ensure no errors are produced, make sure the camera is valid
        if (Camera.main != null)
        {
            // Get the transform of the camera
            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = transform.position - cameraTransform.position;
            forward.Normalize();
            Vector3 up = Vector3.Cross(forward, cameraTransform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }

    /// <summary>
    /// Updates how full the filled bar should be based on a float of 0..1
    /// </summary>
    /// <param name="fill"></param>
    public void UpdateFill(float fill)
    {
        // Make sure fill never goes negative or overfull
        if (fill < 0)
        {
            fill = 0;
        } else if (fill > 1)
        {
            fill = 1;
        }

        fillBar.transform.localScale = new Vector3(fill, 1, 1);
    }
}
