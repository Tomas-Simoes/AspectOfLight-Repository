using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fpsText;
    private float frameCount = 0f;
    private float deltaTime = 0f;
    private float fps = 0f;
    private float updateRate = 4f;

    private void Update()
    {
        frameCount++;
        deltaTime += Time.deltaTime;

        if (deltaTime >= 1f / updateRate)
        {
            fps = frameCount / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";

            frameCount = 0;
            deltaTime -= 1f / updateRate;
        }
    }
}
