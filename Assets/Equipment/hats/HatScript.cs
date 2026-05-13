using UnityEngine;

public class HatScript : MonoBehaviour
{
    [SerializeField] GameObject[] hats;
    [SerializeField] Texture2D[] textures;

    private void Update()
    {
        //hat debug
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetHat();
            ColorHat();
        }
    }

    public void GetHat()
    {
        for (int i = 0; i < hats.Length; i++)
        {
            hats[i].SetActive(false);
        }

        int r = Random.Range(0,hats.Length);
        hats[r].gameObject.SetActive(true);
    }

    public void ColorHat()
    {
        GameObject activeHat = null;

        for (int i = 0; i < hats.Length; i++)
        {
            if (hats[i].activeInHierarchy)
            {
                activeHat = hats[i];
                break;
            }
        }
        if (activeHat == null) return;

        MeshRenderer renderer = activeHat.GetComponent<MeshRenderer>();

        if (renderer == null) return;

        int r = Random.Range(0, textures.Length);
        renderer.material.mainTexture = textures[r];
    }
}
