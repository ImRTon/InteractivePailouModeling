using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Bounds _bound;
    public bool _isDebug;
    // Start is called before the first frame update
    void Start()
    {
        _bound = GetComponent<Renderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDebug)
        {
            DebugPrint();
            _isDebug = false;
        }
    }

    void DebugPrint()
    {
        _bound = GetComponent<Renderer>().bounds;
        Debug.Log("Pos:" + transform.localPosition + ", Scale:" + transform.localScale + ", lossScale:" + transform.lossyScale);
        Debug.Log("Bound:" + _bound.center + ", " + _bound.size);
    }
}
