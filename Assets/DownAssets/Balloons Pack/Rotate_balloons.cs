using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_balloons : MonoBehaviour
{
    [SerializeField] private Transform _transform;    
     
    private float _speed;

    private void Awake()
    {
        _speed = Random.Range(40f, 100f);
    }

    private void Update()
    {        
        _transform.Rotate(Vector3.up * _speed * Time.deltaTime);       

    }
}
