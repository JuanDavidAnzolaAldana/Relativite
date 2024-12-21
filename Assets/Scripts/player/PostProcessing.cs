using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    public Material[] cheap = new Material[4],
        beauty = new Material[4];
    public bool[] filter = new bool[] { false, false, false, true };
    public static int quality;
    private void Start(){
        this.GetComponent<Camera>().depthTextureMode=DepthTextureMode.Depth;
        quality = QualitySettings.GetQualityLevel();
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dst){
        if(quality > 0) {
            RenderTexture part = RenderTexture.GetTemporary(src.width, src.height);
            Graphics.Blit(src, part);
            for(int i = 0; i < filter.Length; i++) {
                if(filter[i]) {
                    RenderTexture par = RenderTexture.GetTemporary(part.width, part.height);
                    Graphics.Blit(part, par, (quality>1?beauty:cheap)[i]);
                    RenderTexture.ReleaseTemporary(part);
                    part = par;
                }
            }
            Graphics.Blit(part, dst);
            RenderTexture.ReleaseTemporary(part);
        } else {
            Graphics.Blit(src, dst);
        }
    }
}
