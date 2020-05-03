using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<GameObject> healthObjs;
    public GameObject healthIconPrefab;
    public Transform healthIconTransform;
    public Image coughMeter;
    private Color meterColor;
    public Text meterText;
    public Text scoreDisplay;
    public GameObject endScreen;

    // Start is called before the first frame update
    void Start()
    {
        meterColor = coughMeter.color;
        healthObjs = new List<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (coughMeter.transform.localScale.x == 1)
        {
            coughMeter.color = Color.green;
            //meterText.text = "'SPACE'";
        }
        else
        {
            meterText.text = "COUGH";
            coughMeter.color = meterColor;
        }
    }

    public void InitializeHealthUI(int hp)
    {
        for (int i = 0; i < hp; i++)
        {
            GameObject spawnedObj = (GameObject)Instantiate(healthIconPrefab, healthIconTransform);
            healthObjs.Add(spawnedObj);
           
        }
    }
}
