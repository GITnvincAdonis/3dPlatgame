using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderTest : MonoBehaviour
{

    [SerializeField] private ComputeShader shader;
    [SerializeField] private RenderTexture renderTexture;
    // Start is called before the first frame update
    void Start()
    {
        OnRenderImage(renderTexture,renderTexture);


        
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest){
        if(renderTexture == null){
            renderTexture = new RenderTexture(256,256,24);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }
        
        shader.SetTexture(0,"Result",renderTexture);
        shader.SetFloat("Resolution", renderTexture.width);
        shader.Dispatch(0,renderTexture.width/8,renderTexture.height/8,1);

        Graphics.Blit(renderTexture,dest);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
