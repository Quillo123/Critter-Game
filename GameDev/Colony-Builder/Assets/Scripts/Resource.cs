using UnityEngine;

public class Resource : MonoBehaviour
{
    public float breakTime = 2f;

    private float startTime = 0;

    private void OnMouseOver()
    {
        if(Input.GetMouseButton(0))
        {
            if(startTime == 0) startTime = Time.time;
            if(Time.time - startTime >= breakTime)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            startTime = 0;
        }
    }
}
