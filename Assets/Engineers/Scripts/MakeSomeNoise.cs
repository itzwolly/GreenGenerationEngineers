using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSomeNoise : MonoBehaviour {

    public float power = 3;
    public float scale = 1;
    public float timeScale = 1;

    private float xoffset;
    private float yoffset;
    private MeshFilter mf;

	// Use this for initialization
	void Start () {
        mf = GetComponent<MeshFilter>();
        MakeNoise();
	}
	
	// Update is called once per frame
	void Update () {
        MakeNoise();
        xoffset += Time.deltaTime * timeScale;
        yoffset += Time.deltaTime * timeScale;
	}
    void MakeNoise()
    {
        Vector3[] verticies = mf.mesh.vertices;

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z) * power;
        }

        mf.mesh.vertices = verticies;
    }

    float CalculateHeight(float x, float y)
    {
        float xCord = x * scale + xoffset;
        float yCord = y * scale + yoffset;

        return Mathf.PerlinNoise(xCord, yCord);
    }
}
