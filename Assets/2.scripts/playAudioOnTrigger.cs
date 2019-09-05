using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DentedPixel;

public class playAudioOnTrigger : MonoBehaviour
{
    AudioSource theAttachedAudioSource;
    Vector3 initialSizeText;
    Vector3 initialSizeImage;

    // Start is called before the first frame update
    void Start()
    {
        theAttachedAudioSource = this.GetComponent<AudioSource>();

        foreach (Transform child in transform)
        {
            if (child.name == "image")
            {
                initialSizeImage = child.localScale;
            }
            if (child.name == "text")
            {
                initialSizeText = child.localScale;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "museumObject")
        {
            /*LeanTween.value(gameObject, 1, 0.1f, 0.3f).setOnUpdate((float val) => {
                other.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, val);
            });*/
            //other.gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (other.tag == "museumObject" && theAttachedAudioSource.clip != null)
        {
            theAttachedAudioSource.Play();
        }

        if(other.tag == "museumObject" && transform.childCount > 0)
        {
            //transform.GetChild(0).transform.gameObject.SetActive(true);
            foreach (Transform child in transform)
            {
                //child.gameObject.SetActive(true);
                //child.gameObject.GetComponent<Renderer>().material.color = new Color(1,1,1,1);
                LeanTween.value(gameObject, 0.25f, 1, 0.3f).setOnUpdate((float val) => {
                    child.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, val);
                });

                if (child.gameObject.GetComponent<TextMeshPro>() != null)
                {
                    //child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                    LeanTween.value(gameObject, 0.15f, 1, 0.3f).setOnUpdate((float val) => {
                        child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, val);
                    });
                }
                //child.gameObject.transform.localScale = child.gameObject.transform.localScale * 2;
                LeanTween.scale(child.gameObject, child.gameObject.transform.localScale * 1.1f, 0.3f).setEase(LeanTweenType.linear);

                if (child.gameObject.GetComponent<LineRenderer>() != null)
                {
                    //child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                    LeanTween.value(gameObject, 0.15f, 1, 0.3f).setOnUpdate((float val) => {
                        child.gameObject.GetComponent<LineRenderer>().SetColors(new Color(0, 0, 0, val), new Color(1, 0, 0, val));
                    });
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "museumObject")
        {
            /*LeanTween.value(gameObject, 0.1f, 1, 0.3f).setOnUpdate((float val) => {
                other.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, val);
            });*/
            //other.gameObject.GetComponent<Renderer>().enabled = true;
        }

        if (other.tag == "museumObject" && theAttachedAudioSource.clip != null)
        {
            theAttachedAudioSource.Stop();
        }

        if (other.tag == "museumObject" && transform.childCount > 0)
        {
            //transform.GetChild(0).transform.gameObject.SetActive(false);
            foreach (Transform child in transform)
            {
                //child.gameObject.SetActive(false);
                //child.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.25f);
                LeanTween.value(gameObject, 1f, 0.25f, 0.3f).setOnUpdate((float val) => {
                    child.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, val);
                });
                if (child.gameObject.GetComponent<TextMeshPro>() != null)
                {
                    //child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 0.15f);
                    LeanTween.value(gameObject, 1, 0.15f, 0.3f).setOnUpdate((float val) => {
                        child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, val);
                    });
                }

                if (child.gameObject.GetComponent<LineRenderer>() != null)
                {
                    //child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                    LeanTween.value(gameObject, 1, 0.15f, 0.3f).setOnUpdate((float val) => {
                        child.gameObject.GetComponent<LineRenderer>().SetColors(new Color(0, 0, 0, val), new Color(1, 0, 0, val));
                    });
                }

                //child.gameObject.transform.localScale = child.gameObject.transform.localScale / 2;
                //LeanTween.scale(child.gameObject, child.gameObject.transform.localScale / 2, 0.3f).setEase(LeanTweenType.linear);

                if (child.name == "image")
                {
                    LeanTween.scale(child.gameObject, initialSizeImage, 0.3f).setEase(LeanTweenType.linear);
                }
                if (child.name == "text")
                {
                    LeanTween.scale(child.gameObject, initialSizeText, 0.3f).setEase(LeanTweenType.linear);
                }
            }
        }
    }
}
