using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sun : MonoBehaviour
{
    public Light sun;
    // Start is called before the first frame update
    [ExecuteInEditMode]
    void Start()
    {
        sun = GetComponent<Light>();
        Shader.SetGlobalVector("_SunDirection", transform.forward);
        Shader.SetGlobalColor("_SunColor", sun.color);
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
        Shader.SetGlobalColor("_SunColor", sun.color);
    }
}
