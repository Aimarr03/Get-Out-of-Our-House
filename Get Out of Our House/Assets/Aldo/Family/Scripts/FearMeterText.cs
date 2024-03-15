using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FearMeterText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fearMeterText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fearMeterText.text = GetComponentInParent<FearMeter>().fearMeter.ToString();
    }
}
