    using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public GameObject clueObject;

    private bool collected = false;
    // Start is called before the first frame update
    void Awake() {
     transform.Rotate(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
     clueObject.transform.Rotate(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected == false)
        {
            collected = true;

            Debug.Log("Triggered!");

            var clueText = GameObject.FindGameObjectWithTag("ClueCollected");
            var text = clueText.GetComponents<Text>().First();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.9f);
            StartCoroutine(FadeOut(text, 2.8f));

            var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach(var r in renderers)
            {
                StartCoroutine(FadeOut(r, 1.5f));
            }
            StartCoroutine(DestroySelf(10.0f));
        }
        
    }

    private IEnumerator FadeOut(Text t, float duration=4.0f)
    {
        
        float counter = 0;
        
        //Get current color
        Color spriteColor = t.color;
        var startAlpha = t.color.a;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(startAlpha, 0, counter / duration);

            //Change alpha only
            t.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }
    }

    private IEnumerator FadeOut(Renderer r, float duration=2.0f)
    {
        
        float counter = 0;
        
        //Get current color
        Color spriteColor = r.material.color;
        var startAlpha = r.material.color.a;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(startAlpha, 0, counter / duration);

            //Change alpha only
            r.material.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }
    }

    private IEnumerator DestroySelf(float duration=10.0f)
    {
        yield return new WaitForSeconds(5.5f);
        Destroy(gameObject);
        yield return null;
    }

}
