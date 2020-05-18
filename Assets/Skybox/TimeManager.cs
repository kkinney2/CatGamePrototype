using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public Vector2 starOffsetStep;
    private Vector2 currentStarOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentStarOffset += (starOffsetStep * Time.deltaTime);
        Shader.SetGlobalVector("_StarsOffset", currentStarOffset);
    }
}
