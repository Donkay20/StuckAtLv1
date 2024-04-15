using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    private float interval = 5f;
    private float timer;

    public class Marker
    {
        public Vector3 position;
        public Quaternion rotation;

        public Marker(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    public List<Marker> markerList = new List<Marker>();

    void Update()
    {
        UpdateMarkerList();

    }
    
    public void UpdateMarkerList()
    {
        markerList.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkerList()
    {
        markerList.Clear();
        markerList.Add(new Marker(transform.position, transform.rotation));
    }

    public void OnCollisionExit2D(Collision2D col)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
