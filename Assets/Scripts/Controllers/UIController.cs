using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
    public static UIController instance;

    private Text numberOfNodes;
    private Text graphType;
    private Text heuristicType;

    public void SetNumberOfNodes(int number) {
        SetValue(numberOfNodes, number.ToString());
    }

    public void SetGraphType(string type) {
        SetValue(graphType, type);
    }

    public void SetHeuristicType(string type) {
        SetValue(heuristicType, type);
    }

    public void SetValue(Text t, string value) {
        int index = t.text.LastIndexOf(':');
        string key = t.text;

        if (index + 1 < t.text.Length)
            key = t.text.Remove(t.text.LastIndexOf(':') + 1);

        t.text = key + ' ' + value;
    }

    protected void Awake() {
        instance = this;

        numberOfNodes = GameObject.FindGameObjectWithTag("NumberOfNodes").GetComponent<Text>();
        graphType = GameObject.FindGameObjectWithTag("GraphType").GetComponent<Text>();
        heuristicType = GameObject.FindGameObjectWithTag("HeuristicType").GetComponent<Text>();
    }

    protected void OnDestroy() {
        if (instance != null) {
            instance = null;
        }
    }
}
