using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180f, 0);
        }
    }
}
