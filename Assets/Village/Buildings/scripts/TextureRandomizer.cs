using UnityEngine;

public class TextureRandomizer : MonoBehaviour
{
    [SerializeField]MeshRenderer[] meshRenderers;
    [SerializeField]Texture[] textures;
    int p = 0;
    //debug
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Paint();
        }
    }
    public void Paint()
    {
        int r;
       
        do
        {
            r = Random.Range(0, textures.Length);
        }
        while (r == p);

        p = r;

        foreach (var renderer in meshRenderers)
        {
            renderer.material.mainTexture = textures[r];
        }
    }
}
