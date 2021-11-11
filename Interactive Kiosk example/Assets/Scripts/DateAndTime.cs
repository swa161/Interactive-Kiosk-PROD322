using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class DateAndTime : MonoBehaviour
{

    //public Text largeText;
    public TextMeshProUGUI timedateText;

    // Start is called before the first frame update
    void Start()
    {

        

      
    }

    // Update is called once per frame
    private void Update()
    {
        string time = System.DateTime.UtcNow.ToLocalTime().ToString("hh:mm tt   dd/MM/yyyy   ");
        timedateText.text = time;
    }
}
