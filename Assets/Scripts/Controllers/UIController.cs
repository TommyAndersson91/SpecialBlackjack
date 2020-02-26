using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UIController : MonoBehaviour
{
public delegate void ChangePlayerScore(int handValue);
public static event ChangePlayerScore OnScoreChange;

private void Start() {


}
}
