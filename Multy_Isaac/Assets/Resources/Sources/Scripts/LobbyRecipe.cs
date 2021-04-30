using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRecipe : MonoBehaviour
{
    private bool isOpen = false;
    public Animator anim;
    public void OpenRecipe()
    {
        if (isOpen)
        {
            isOpen = false;
            anim.Play("InvenClose");
        }
        else
        {
            isOpen = true;
            anim.Play("InvenOpen");
        }
    }
}
