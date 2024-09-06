using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    public float timeBetweenSteps;
    public TextMeshProUGUI text;
    private float frames;
    private float time;
    void Update()
    {
        time += Time.deltaTime;
        frames++;

        if (time >= timeBetweenSteps)
        {
            int currentFPS = Mathf.RoundToInt(frames / time);
            text.text = currentFPS.ToString();
            time -= timeBetweenSteps;
            frames = 0;
        }
    }
}
