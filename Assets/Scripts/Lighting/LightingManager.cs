using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField]
    private Light directionalLight;
    
    [SerializeField]
    private LightingPreset preset;
    
    //Variables
    [SerializeField, Range(0, 24)]
    private float timeOfTheDay = 9;
    
    [SerializeField]
    private int lengthOfADayInMinutes;



    private void Update()
    {
        if (preset == null)
	    { 
            return;
	    }

        if (Application.isPlaying)
        {
            timeOfTheDay += Time.deltaTime / 3600 * 60 * 24 / lengthOfADayInMinutes;
            timeOfTheDay %= 24; // Modulus to ensure always between 0-24
            UpdateLighting(timeOfTheDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfTheDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);

            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}