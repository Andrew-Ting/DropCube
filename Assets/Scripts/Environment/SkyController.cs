using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> skyTextures;
    private int lastTextureIndex = 0;
    private GameObject sky1;
    private GameObject sky2;
    private float fadeTime = 1f;
    void Start() {
        sky1 = transform.Find("Sky").gameObject;
        sky2 = transform.Find("Sky2").gameObject;
        sky2.SetActive(false);
    }

    public void RefreshSkyTexture()
    {
        int newTextureIndex;
        do {
            newTextureIndex = Random.Range(0, skyTextures.Count - 1);
        } while (newTextureIndex == lastTextureIndex);
        if (sky1.activeSelf) {
            sky2.GetComponent<Renderer>().material.mainTexture = skyTextures[newTextureIndex];
            StartCoroutine(ToggleOpacity(sky1));
            StartCoroutine(ToggleOpacity(sky2));
        }
        else {
            sky1.GetComponent<Renderer>().material.mainTexture = skyTextures[newTextureIndex];
            StartCoroutine(ToggleOpacity(sky1));
            StartCoroutine(ToggleOpacity(sky2));
        }
        lastTextureIndex = newTextureIndex;

    }
    IEnumerator ToggleOpacity(GameObject skyType) {
        if (skyType.activeSelf) {
            float startTime = Time.time;
            while (Time.time < startTime + fadeTime)
            {
                Color originalColor = skyType.GetComponent<Renderer>().material.color;
                originalColor.a = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeTime);
                skyType.GetComponent<Renderer>().material.color = originalColor;
                yield return null;
            }
            skyType.SetActive(false);
        } else {
            skyType.SetActive(true);
            float startTime = Time.time;
            while (Time.time < startTime + fadeTime)
            {
                Color originalColor = skyType.GetComponent<Renderer>().material.color;
                originalColor.a = Mathf.Lerp(0, 1, (Time.time - startTime) / fadeTime);
                skyType.GetComponent<Renderer>().material.color = originalColor;
                yield return null;
            }
        }
    }
}
