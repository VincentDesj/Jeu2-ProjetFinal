using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightController : MonoBehaviour
{
    public enum Activation { On, Off, Flickering }
    private float timeDelay;

    public Activation LightState;

    public Light[] listLights;

    // Update is called once per frame
    void Update()
    {
        if (LightState == Activation.On)
        {
            StartCoroutine(LightsOn(listLights));
        }
        else if (LightState == Activation.Off)
        {
            StartCoroutine(LightsOff(listLights));
        }
        else
        {
            StartCoroutine(LightsFlickering(listLights));
        }
    }

    private IEnumerator LightsOn(Light[] listLights)
    {
        for (int i = 0; i < listLights.Length; i++) 
        {
            if (listLights[i].enabled == false)
                listLights[i].enabled = true;
        }
        yield return null;
    }

    private IEnumerator LightsOff(Light[] listLights)
    {
        for (int i = 0; i < listLights.Length; i++)
        {
            if (listLights[i].enabled == true)
                listLights[i].enabled = false;
        }
        yield return null;
    }

    private IEnumerator LightsFlickering(Light[] listLights)
    {
        foreach (Light light in listLights)
            light.enabled = false;
        timeDelay = Random.Range(0.2f, 0.3f);
        yield return new WaitForSeconds(timeDelay);
        foreach (Light light in listLights)
            light.enabled = true;
        timeDelay = Random.Range(0.3f, 0.5f);
        yield return new WaitForSeconds(timeDelay);
    }
}
