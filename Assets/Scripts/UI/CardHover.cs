using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerClickHandler
{
    public bool magnify;
    public float zValue;
    
    private void Start() {
        magnify = true;
    }

    void OnMouseOver()
    { 
       
        if (magnify)
        {
            zValue = transform.position.z;
            transform.localScale += new Vector3(1.5F, 1.5f, 1.5f);
            transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    
            magnify = false;
        }
       // transform.parent.GetComponent<HandArranger>().SetSpacing(transform);
        // transform.parent.GetComponent<HandArranger>().Arrange();
    }

    void OnMouseExit()
    {
        GameObject go = GameObject.Find("CardsInHandPanel");
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).position = new Vector3(go.transform.GetChild(i).position.x, go.transform.GetChild(i).position.y, zValue);
            zValue++;
        }
        // transform.position = new Vector3(transform.position.x, transform.position.y, zValue);
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        magnify = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}