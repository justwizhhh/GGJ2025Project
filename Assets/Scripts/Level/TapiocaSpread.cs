using UnityEngine;

public class TapiocaSpread : MonoBehaviour
{
    void Start()
    {
        transform.eulerAngles = new Vector3
            (Random.Range(-180, 180),
            Random.Range(-180, 180),
            Random.Range(-180, 180));
    }
}
