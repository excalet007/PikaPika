using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

    Vector3 cloudMove = new Vector3(1f * Time.deltaTime, 0, 0);
    
    float beforeY;
    float beforeX;
    float x;
    float y;
    float scaleFactor;
    

    void Start()
    {
        beforeX = beforeX = x = y = 0f;

        scaleFactor = Random.Range(0.6f, 1.4f);
    }

	void Update()
    {
        scaleFactor += Time.deltaTime * 0.5f;
        if (scaleFactor > 1.5f)
        {
            scaleFactor = 0.5f;
        }

        this.transform.localScale = new Vector3(2.7f+ scaleFactor,3.8f + scaleFactor,1) ;

        transform.position += cloudMove;
        if (transform.position.x > PlayManager.mapInfo[0] / 2 + 1f)
            Relocate();
    }



    void Relocate()
    {
        float y = Random.Range(3.0f, 8f);
        float x = Random.Range(0f, 5f);

        if (Mathf.Abs(beforeX - x) < 1f || Mathf.Abs(beforeY - y) < 1f )
        {
            y = Random.Range(3.0f, 8f);
            x = Random.Range(0f, 5f);
        }

        transform.position = new Vector3(- (PlayManager.mapInfo[0] / 2 + x), y, 1f);
    }
}
