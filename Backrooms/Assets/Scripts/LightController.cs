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

    private GameObject ceiling;
    private Light[] listLights;
    private ParticleSystem[] listSparkles;
    private List<GameObject> listMaterials = new List<GameObject>();

    public AudioSource fluorescent;
    public AudioSource fluorescentFlicker;

    public Material lightOn;
    public Material lightOff;

    void Start()
    {
        ceiling = GameObject.Find("ceiling");
        listLights = ceiling.GetComponentsInChildren<Light>();
        listSparkles = ceiling.GetComponentsInChildren<ParticleSystem>();

        foreach (Transform child in ceiling.transform)
        { 
            listMaterials.Add(child.GetChild(0).gameObject);
            
        }
        Debug.Log(listMaterials);
    }

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
        if (fluorescentFlicker.isPlaying == true)
        {
            fluorescentFlicker.Stop();
        }

        if (fluorescent.isPlaying == false)
            fluorescent.Play();

        for (int i = 0; i < listLights.Length; i++) 
        {
            if (listLights[i].enabled == false)
            { 
                listLights[i].enabled = true;
                listMaterials[i].GetComponent<Renderer>().material = lightOn;
            }
                

            listSparkles[i].Stop();
        }
        yield return null;
    }

    private IEnumerator LightsOff(Light[] listLights)
    {
        if (fluorescentFlicker.isPlaying == true)
        {
            fluorescentFlicker.Stop();
        }
            
        if (fluorescent.isPlaying == true)
            fluorescent.Stop();

        for (int i = 0; i < listLights.Length; i++)
        {
            if (listLights[i].enabled == true)
            { 
                listLights[i].enabled = false;
                listMaterials[i].GetComponent<Renderer>().material = lightOff;
            }

            listSparkles[i].Stop();
        }
        yield return null;
    }

    private IEnumerator LightsFlickering(Light[] listLights)
    {
        if (fluorescentFlicker.isPlaying == false)
        { 
            fluorescentFlicker.Play();
            foreach (ParticleSystem sparkles in listSparkles)
            {
                sparkles.Play();
            }
        }

        if (fluorescent.isPlaying == false)
            fluorescent.Play();

        for (int i = 0 ; i < listLights.Length; i++) 
        {
            listLights[i].enabled = false;
            listMaterials[i].GetComponent<Renderer>().material = lightOff;
        }

        timeDelay = Random.Range(0.4f, 0.5f);
        yield return new WaitForSeconds(timeDelay);

        for (int i = 0; i < listLights.Length; i++)
        {
            listLights[i].enabled = true;
            listMaterials[i].GetComponent<Renderer>().material = lightOn;
        }

        timeDelay = Random.Range(0.4f, 0.5f);
        yield return new WaitForSeconds(timeDelay);
    }
}

