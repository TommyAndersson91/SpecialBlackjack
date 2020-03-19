using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateHelper
{
  public static float GetFraction(float cur, float time, string easetype)
  {
    float fraction;
    switch (easetype)
    {
      case "Linear":
        fraction = cur / time;
        break;
      case "ExpoOut":
        fraction = 1f - Mathf.Pow(2, -10 * cur / time);
        break;
      case "SquareIn":
        fraction = Mathf.Pow(cur / time, 2f);
        break;
      case "CubicOut":
        float t = 1f - cur / time;
        fraction = 1f - Mathf.Pow(t, 3f);
        break;
      case "CubicOutBack":
        float s = 1.5f;
        float v = Mathf.Pow((cur/time),2f) - 1f;
        fraction = Mathf.Pow(v, 2) * (s + (s + 1) * v) + 1;
        break;
      case "Spring":
        fraction = 1 - Mathf.Exp(-6 * cur / time) * Mathf.Cos(cur / time * 2.5f * Mathf.PI);
        break;
      default:
        fraction = cur / time;
        break;
    }
    return fraction;
  }
}
