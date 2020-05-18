using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sun : MonoBehaviour
{
    // Start is called before the first frame update
    [ExecuteInEditMode]
    void Start()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }

    // Update is called once per frame
    [ExecuteInEditMode]
    void Update()
    {
        Shader.SetGlobalVector("_SunDirection", transform.forward);
    }
}
