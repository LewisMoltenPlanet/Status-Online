using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStart : MonoBehaviour {

    private SkinnedMeshRenderer skin;
    private MeshRenderer mesh;


    void Start ()
    {
        skin = GetComponent<SkinnedMeshRenderer>();
        mesh = GetComponent<MeshRenderer>();

        if (skin)
        {

        }else if (mesh)
        {

        }
	}
}
