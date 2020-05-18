using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [Range(1,100)]
    public int timeScale = 1;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timeScale;
    }
}
