using UnityEngine;
using UnityEngine.UI;

public class TabScrollingExample : MonoBehaviour {
    public Scrollbar scrollBar; //The scrollbar which will be effected.
    public float scrollDistance;//Set between -1 and 1.
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        scrollBar.value += scrollDistance;
    }
}
