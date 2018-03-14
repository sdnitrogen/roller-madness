using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class UpDown : MonoBehaviour
{
    public bool autoStart;
    public float minimum;
    public float maximum;

    public bool useTimeFactor;
    
    // hide in the inspector but make visible to other script files
    [HideInInspector]
    public float timeFactor = 4.0f;
    
    public void Awake()
    {
        if (autoStart)
        {
            StartPingPong();
        }
    }

    public void StartPingPong()
    {
        if (minimum != maximum)
        {
            float rng = UnityEngine.Random.Range(minimum, maximum);
            transform.position = new Vector3(transform.position.x, rng, transform.position.z);
            StartCoroutine(PingPong(minimum, maximum));
        }
    }

    public void StartPingPong(float min, float max)
    {
        if (min != max)
        {
            float rng = UnityEngine.Random.Range(min, max);
            transform.position = new Vector3(transform.position.x, rng, transform.position.z);
            StartCoroutine(PingPong(min, max));
        }
    }

    public void StartPingPong(float min, float max, float speed)
    {
        if (min != max)
        {
            if (speed < 1.0f)
                speed = 1.0f;
            else if (speed > 10.0f)
                speed = 10.0f;
            useTimeFactor = true;
            timeFactor = speed;
            float rng = UnityEngine.Random.Range(min, max);
            transform.position = new Vector3(transform.position.x, rng, transform.position.z);
//            StartCoroutine(MoveObject(transform.gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(transform.position.x, max, transform.position.z), timeFactor));
            StartCoroutine(PingPong(min, max));
        }
    }

    IEnumerator PingPong(float min, float max)
    {
        while (true)
        {
            if (useTimeFactor)
            {
                yield return StartCoroutine(MoveObject(transform.gameObject, new Vector3(transform.position.x, max, transform.position.z), new Vector3(transform.position.x, min, transform.position.z), timeFactor));
                yield return StartCoroutine(MoveObject(transform.gameObject, new Vector3(transform.position.x, min, transform.position.z), new Vector3(transform.position.x, max, transform.position.z), timeFactor));
            }
            else
            {
                yield return StartCoroutine(MoveObject(transform.gameObject, new Vector3(transform.position.x, max, transform.position.z), new Vector3(transform.position.x, min, transform.position.z)));
                yield return StartCoroutine(MoveObject(transform.gameObject, new Vector3(transform.position.x, min, transform.position.z), new Vector3(transform.position.x, max, transform.position.z)));
            }
        }
    }

    IEnumerator MoveObject(GameObject objectTomove, Vector3 startPosition, Vector3 endPosition)
    {
        float timer = Time.time;
        objectTomove.transform.position = startPosition;
        
        while (objectTomove.transform.position != endPosition)
        {
            objectTomove.transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - timer) / 4);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator MoveObject(GameObject objectTomove, Vector3 startPosition, Vector3 endPosition, float time)
    {
        float timer = Time.time;
        objectTomove.transform.position = startPosition;
        
        while (objectTomove.transform.position != endPosition)
        {
            objectTomove.transform.position = Vector3.Lerp(startPosition, endPosition, (Time.time - timer) / time);
            yield return new WaitForSeconds(0.01f);
        }
    }

/*
    IEnumerator MoveObject(GameObject objectTomove, Vector3 startPosition, Vector3 endPosition, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;

        while (startPosition != endPosition)
        {
            i += Time.deltaTime * rate;
            objectTomove.transform.position = Vector3.Lerp(startPosition, endPosition, i);
            yield return new WaitForSeconds(0.01f);
        }
    }
*/
}