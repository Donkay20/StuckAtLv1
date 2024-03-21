using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VirtualCameraStates : MonoBehaviour
{
    public Volume v;
    private Vignette vg;

    // Start is called before the first frame update
    void Start()
    {
        //v = GetComponent<Volume>();
        v.profile.TryGet(out vg);

        SetRuins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRuins()
    {
        //vg.color.Override(new Color(.25f, .05f, .05f, 1.0f));
    }

    public void SetForest()
    {
        vg.color.Override(new Color(.1f, .1f, .4f, 1.0f));
    }

    
}
