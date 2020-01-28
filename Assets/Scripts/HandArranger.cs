using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandArranger : MonoBehaviour
{
public GridLayoutGroup gridLayoutGroup;
public Vector2 vector;
public float zValue = 1f;

void setGrid()
{


}

private void Start() {

        Arrange();

}

private void Update() {
    // Arrange();
}
public void Arrange()
{
        GameObject go = GameObject.Find("CardsInHandPanel");
        Debug.Log(go.name + " has " + go.transform.childCount + " children");
        zValue = 1f;
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).position = new Vector3(go.transform.GetChild(i).position.x, go.transform.GetChild(i).position.y, zValue);
            zValue++;
        }
        //SetSpacing(go.transform);
   
}

public void SetSpacing(Transform go) 
{
        switch (go.transform.childCount)
        {
            case 4:
                gridLayoutGroup.spacing = new Vector2(-0.50f, 0.0f);
                break;
            case 5:
                gridLayoutGroup.spacing = new Vector2(-0.55f, 0.0f);
                break;
            case 6:
                gridLayoutGroup.spacing = new Vector2(-0.6f, 0.0f);
                break;
            case 7:
                gridLayoutGroup.spacing = new Vector2(-0.65f, 0.0f);
                break;
            default:
                break;
        }
}

}
