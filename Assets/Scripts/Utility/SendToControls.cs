using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendToControls : MonoBehaviour
{
    public ChangePlayerControls playerControls;
    public void Send()
    {
        playerControls.AlterListening(gameObject.GetComponent<Button>());
    }
}
