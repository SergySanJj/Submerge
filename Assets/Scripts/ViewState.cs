using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewState : MonoBehaviour
{
    public static ViewState current;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        
    }
}
