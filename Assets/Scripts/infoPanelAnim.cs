using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoPanelAnim : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void playAnim()
    {
        anim.Play("infoPanelAnimation");
    }

    public void closeAnim()
    {
        anim.Play("infoPanelAnimationReverse");
    }
}
