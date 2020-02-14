using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardFlyIn : MonoBehaviour
{
    public static Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayCardFly()
    {
        animator.Play("newAnim");
    }
}
