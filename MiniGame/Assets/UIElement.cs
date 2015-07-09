using UnityEngine;
using System.Collections;

public class UIElement : MonoBehaviour {
    
    public enum UIElementState
    {
        activated,
        disabled
    }

    public Transform targetTransform;
    public Transform initialTransform;
    public float moveSpeed;

    [HideInInspector]
    public Vector3 currentPos;

    public UIElementState ElementState;

    void Start()
    {
        currentPos = this.transform.position;
    }
	// Update is called once per frame
	void Update () {
        if(ElementState== UIElementState.activated)
        {
            currentPos = Vector3.Lerp(currentPos, targetTransform.position, moveSpeed);
        }else
        {
            currentPos = Vector3.Lerp(currentPos, initialTransform.position, moveSpeed);
        }
        this.transform.position = currentPos;
    }

    public void SetElementState(UIElementState state)
    {
        ElementState = state;
    }
}
