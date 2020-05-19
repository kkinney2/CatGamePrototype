using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLights : MonoBehaviour
{
    public float powerDelay = 1f;

    public void SwitchStreetLights(bool enabled)
    {
        foreach (Light light in GetComponentsInChildren<Light>())
        {
            light.enabled = enabled;
        }

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.materials)
            {
                if (enabled)
                {
                    material.EnableKeyword("_EMISSION");
                }
                else
                {
                    material.DisableKeyword("_EMISSION");
                }
            }
        }
    }

    public void DelaySwitchStreetLights(bool enabled)
    {
        StartCoroutine(ToggleLights(enabled));
        StartCoroutine(ToggleEmission(enabled));
    }

    IEnumerator ToggleLights(bool enabled)
    {
        yield return new WaitForSeconds(TimeManager.instance.cycleInMinutes * powerDelay);
        foreach (Light light in GetComponentsInChildren<Light>())
        {
            light.enabled = enabled;
        }

        yield return new WaitForSeconds(TimeManager.instance.cycleInMinutes / 24 / 60 / 60 * powerDelay / 10f);
        foreach (Light light in GetComponentsInChildren<Light>())
        {
            light.enabled = !enabled;
        }

        yield return new WaitForSeconds(TimeManager.instance.cycleInMinutes / 24 / 60 / 60 * powerDelay / 2f);
        foreach (Light light in GetComponentsInChildren<Light>())
        {
            light.enabled = enabled;
        }
    }

    IEnumerator ToggleEmission(bool enabled)
    {
        yield return new WaitForSeconds(TimeManager.instance.cycleInMinutes / 24 / 60 / 60 * powerDelay);
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.materials)
            {
                if (enabled)
                {
                    material.EnableKeyword("_EMISSION");
                }
                else
                {
                    material.DisableKeyword("_EMISSION");
                }
            }
        }

        yield return new WaitForSeconds(TimeManager.instance.cycleInMinutes / 24 / 60 / 60 * powerDelay / 10f);
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.materials)
            {
                if (enabled)
                {
                    material.EnableKeyword("_EMISSION");
                }
                else
                {
                    material.DisableKeyword("_EMISSION");
                }
            }
        }

        yield return new WaitForSeconds(TimeManager.instance.cycleInMinutes / 24 / 60 / 60 * powerDelay / 2f);
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            foreach (Material material in renderer.materials)
            {
                if (enabled)
                {
                    material.EnableKeyword("_EMISSION");
                }
                else
                {
                    material.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}