using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var graph = new RuntimeGraph("Assets/NodeGraphFrame/GraphFiles/New Extension Node Graph.json");
        graph.RunGraph(1, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
