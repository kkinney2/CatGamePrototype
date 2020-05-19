using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [Header("Time")]
    public float cycleInMinutes = 1;

    // Fractional game time, range 0 <> 1. 0 is midnight, 0.5 is noon..
    private float decimalTime = 0.0f;
    // Get time from other scripts by using DayNightCycle.DecimalTime.
    public float DecimalTime { get { return decimalTime; } private set { decimalTime = value; } }



    [Header("Sun")]
    #region Sun
    public Transform sun;
    private Light sunLight;
    private float sunAngle;

    public Gradient sunColor = new Gradient()
    {
        colorKeys = new GradientColorKey[2]{
        new GradientColorKey(new Color(1, 0.75f, 0.3f), 0.45f),
        new GradientColorKey(new Color(0.95f, 0.95f, 1), 0.75f),
        },
        alphaKeys = new GradientAlphaKey[2]{
        new GradientAlphaKey(1, 0),
        new GradientAlphaKey(1, 1)
        }
    };

    public AnimationCurve sunBrightness = new AnimationCurve(
       new Keyframe(0, 0.01f),
       new Keyframe(0.15f, 0.01f),
       new Keyframe(0.35f, 1),
       new Keyframe(0.65f, 1),
       new Keyframe(0.85f, 0.01f),
       new Keyframe(1, 0.01f)
       );
    #endregion
    [Space(10)]

    [Header("Sky")]
    #region Sky

    [GradientUsage(true)]
    public Gradient skyColorDay = new Gradient()
    {
        colorKeys = new GradientColorKey[3]{
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 0),
            new GradientColorKey(new Color(0.7f, 1.4f, 3), 0.5f),
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 1),
        },
        alphaKeys = new GradientAlphaKey[2]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        }
    };

    [GradientUsage(true)]
    public Gradient skyColorNight = new Gradient()
    {
        colorKeys = new GradientColorKey[3]{
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 0),
            new GradientColorKey(new Color(0.44f, 1, 1), 0.5f),
            new GradientColorKey(new Color(0.75f, 0.3f, 0.17f), 1),
        },
        alphaKeys = new GradientAlphaKey[2]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        }
    };
    #endregion
    [Space(10)]

    [Header("Stars")]
    #region Stars

    public float starsSpeed = 8f;

    public AnimationCurve starBrightness = new AnimationCurve(
       new Keyframe(0, 1),
       new Keyframe(0.15f, 1),
       new Keyframe(0.35f, 0.01f),
       new Keyframe(0.65f, 0.01f),
       new Keyframe(0.85f, 1),
       new Keyframe(1, 1)
       );

    #endregion
    [Space(10)]

    [Header("Clouds")]
    public Vector2 cloudsSpeed = new Vector2(1, -1);
    [Space(10)]

    [Header("Fog")]
    #region Fog

    public Gradient fogColor = new Gradient()
    {
        colorKeys = new GradientColorKey[5]{
            new GradientColorKey(new Color(0.83f, 0.9f, 0.9f), 0),
            new GradientColorKey(new Color(1, 0.54f, 0.37f), 0.25f),
            new GradientColorKey(new Color(0.95f, 0.95f, 1), 0.5f),
            new GradientColorKey(new Color(1, 0.54f, 0.37f), 0.75f),
            new GradientColorKey(new Color(1, 0.9f, 0.9f), 1),
        },
        alphaKeys = new GradientAlphaKey[2]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        }
    };
    #endregion
    [Space(10)]

    [Header("Time Of Day Events")]
    #region TimeOfDayEvents

    public UnityEvent onMidnight;
    public UnityEvent onMorning;
    public UnityEvent onNoon;
    public UnityEvent onEvening;

    // enum value type data type
    private enum TimeOfDay { Night, Morning, Noon, Evening }

    // variables of enum type TimeOfDay
    private TimeOfDay timeOfDay = TimeOfDay.Night;
    private TimeOfDay TODMessageCheck = TimeOfDay.Night;

    #endregion


    private void Awake()
    {
        if (TimeManager.instance == null) instance = this;
        else Debug.Log("Warning; Multiples instances found of {0}, only one instance of {0} allowed.", this);

    }

    // Start is called before the first frame update
    void Start()
    {
        sun.rotation = Quaternion.Euler(0, -90, 0);
        sunLight = sun.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSunAngle();
        if (Application.isPlaying)
        {
            UpdateDecimalTime();
            UpdateTimeOfDay();
            RotateSun();
            MoveClouds();
        }
        SetSunBrightness();
        SetSunColor();
        SetSkyColor();
        SetStarsIntensity();
        MoveStars();
        SetFogColor();
    }

    #region SUN

    private void RotateSun()
    {
        // Rotate on the x-axis by one degree per second
        sun.Rotate(Vector3.right * Time.deltaTime * 6 / cycleInMinutes);
    }

    private void SetSunBrightness()
    {
        // Adjust sun brightness by the angle at which the sun is rotated.
        sunLight.intensity = sunBrightness.Evaluate(sunAngle);
    }

    void SetSunColor()
    {
        sunLight.color = sunColor.Evaluate(sunAngle);
    }

    private void UpdateSunAngle()
    {
        // range -180 <> 180 with linear progression, meaning -180 is midnight -90 is morning 0 is midday and 90 is sunset.
        sunAngle = Vector3.SignedAngle(Vector3.down, sun.forward, sun.right);
        sunAngle = sunAngle / 360 + 0.5f;
    }

    #endregion

    private void SetSkyColor()
    {
        if (sunAngle >= 0.25f && sunAngle < 0.75f)
        {
            RenderSettings.skybox.SetColor("_SkyColor2", skyColorDay.Evaluate(sunAngle * 2f - 0.5f));
        }
        else if (sunAngle > 0.75f)
        {
            RenderSettings.skybox.SetColor("_SkyColorNight2", skyColorNight.Evaluate(sunAngle * 2f - 1.5f));
        }
        else
        {
            RenderSettings.skybox.SetColor("_SkyColorNight2", skyColorNight.Evaluate(sunAngle * 2f + 0.5f));
        }
    }

    private void SetStarsIntensity()
    {
        RenderSettings.skybox.SetFloat("_StarsIntensity", starBrightness.Evaluate(sunAngle));
    }

    private void MoveStars()
    {
        RenderSettings.skybox.SetVector("_StarsOffset", new Vector2(sunAngle * starsSpeed, 0));
    }

    private void MoveClouds()
    {
        RenderSettings.skybox.SetVector("_CloudsOffset", (Vector2)RenderSettings.skybox.GetVector("_CloudsOffset") + Time.deltaTime * cloudsSpeed);
    }

    private void SetFogColor()
    {
        RenderSettings.fogColor = fogColor.Evaluate(sunAngle);
    }
    private void SetFogDensity()
    {
        //RenderSettings.fogDensity = fogDensity.Evaluate(sunAngle);
    }

    private void SetSkyIntensity()
    {
        // RenderSettings.skybox.SetFloat("_SkyIntensity",skyIntensity.Evaluate(sunAngle));
    }

    private void UpdateDecimalTime()
    {
        // 0.25 because the day starts at morning. Time.time times 6 because 360 degrees in a full rotation.
        // Modulo(%) 1 makes the value go from 0 to 1 repeatedly.
        decimalTime = (0.25f + Time.time * 6 / cycleInMinutes / 360) % 1;
        // Uncomment to see decimal time in the console
        // Debug.Log(decimalTime); 
    }

    private void UpdateTimeOfDay()
    {
        if (decimalTime > 0.25 && decimalTime < 0.5f)
        {
            timeOfDay = TimeOfDay.Morning;
        }
        else if (decimalTime > 0.5f && decimalTime < 0.75f)
        {
            timeOfDay = TimeOfDay.Noon;
        }
        else if (decimalTime > 0.75f)
        {
            timeOfDay = TimeOfDay.Evening;
        }
        else
        {
            timeOfDay = TimeOfDay.Night;
        }

        // Check if the timeOfDay has changed. If so, invoke the event.
        if (TODMessageCheck != timeOfDay)
        {
            InvokeTimeOfDayEvent();
            TODMessageCheck = timeOfDay;
        }
    }

    private void InvokeTimeOfDayEvent()
    {
        switch (timeOfDay)
        {
            case TimeOfDay.Night:
                if (onMidnight != null) onMidnight.Invoke();
                Debug.Log("OnMidnight");
                break;
            case TimeOfDay.Morning:
                if (onMorning != null) onMorning.Invoke();
                Debug.Log("OnMorning");
                break;
            case TimeOfDay.Noon:
                if (onNoon != null) onNoon.Invoke();
                Debug.Log("OnNoon");
                break;
            case TimeOfDay.Evening:
                if (onEvening != null) onEvening.Invoke();
                Debug.Log("OnEvening");
                break;
        }
    }

}
