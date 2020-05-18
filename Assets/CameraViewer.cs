using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewer : MonoBehaviour
{
    public Camera[] cameras;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameras = FindObjectsOfType<Camera>();

        foreach (Camera camera in cameras)
        {
            camera.enabled = false;
        }

        cameras[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                SwapCamera(currentIndex + 1);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                SwapCamera(currentIndex - 1);
            }
        }
    }

    private void SwapCamera(int index)
    {
        if (index < 0)
        {
            index = 0;
            return;
        }
        else if (index > cameras.Length - 1)
        {
            index = cameras.Length - 1;
            return;
        }

        cameras[index].enabled = true;
        cameras[currentIndex].enabled = false;

        currentIndex = index;
    }
}
