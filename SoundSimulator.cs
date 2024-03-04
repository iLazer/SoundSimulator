using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundSimulator : MonoBehaviour
{
    [SerializeField] List<GameObject> soundSources;
    [SerializeField] GameObject listener;
    [SerializeField] EditableProperty power, amplitude, frequency, speedOfSound, time;
    [SerializeField] TMP_Text intensityText, currentAmplitudeText, em1, em2;
    [SerializeField] Button returnEmitters, returnListener;
    [Header("Unmodifiable")]
    public float intensity;
    public float currentAmplitude;
    Vector3 originalPos1, originalPos2, listPos;
    private void Start()
    {
        //for easy reset of sound source and listener positions
        originalPos1 = soundSources[0].transform.position;
        originalPos2 = soundSources[1].transform.position;
        listPos = listener.transform.position;
        returnEmitters.onClick.AddListener(() => { soundSources[0].transform.position = originalPos1; soundSources[1].transform.position = originalPos2; });
        returnListener.onClick.AddListener(() => { listener.transform.position = listPos; });
    }
    private void Update()
    {
        intensity = 0; currentAmplitude = 0; //reset values
        int i = 0;
        foreach (GameObject s in soundSources)
        {
            float distance = CalculateDistance(s.transform.position, listener.transform.position); //calculate distance
            intensity += power.value / (4 * Mathf.PI * distance * distance); //calculate added intensity, using the formula 
            //I = P / (4 * pi * r^2), where I is intensity, P is power, and r is distance.
            float waveLength = speedOfSound.value / frequency.value; //calculate wavelength
            float addedAmplitude = amplitude.value * Mathf.Sin(2 * Mathf.PI / waveLength * (distance - (speedOfSound.value * time.value))); //calculate added amplitude
            //since this is a sine wave added across all sound sources, satisfies final project requirement of added sine waves.
            currentAmplitude += addedAmplitude; //calculate added amplitude

            //visuals
            if (i == 0) em1.text = addedAmplitude.ToString("F2");
            else em2.text = addedAmplitude.ToString("F2");
            i++;
        }
        //visuals
        intensityText.text = intensity.ToString("F2");
        currentAmplitudeText.text = currentAmplitude.ToString("F2");
    }

    private float CalculateDistance(Vector3 a, Vector3 b)
    {
        Vector3 differenceVector = Vector3.Scale(a - b, a - b); //calculate difference vector squared
                                                                //Vector3.Scale() is used to multiply each component of the vector by itself.
        return Mathf.Sqrt(differenceVector.x + differenceVector.y + differenceVector.z); //calculate distance using the formula sqrt(x^2 + y^2 + z^2).
    }
}
