using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private bool _playerIn = false;
    private PlayerNecessities _playerNecessities;
    private void Start()
    {
        _playerNecessities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNecessities>();
    }

    private void Update()
    {
        if(_playerIn && Input.GetKeyDown(KeyCode.E))
        {
            if(gameObject.tag == "Key")
            {
                _playerNecessities._hasKey = true;
            }
            else if(gameObject.tag == "Projector")
            {
                _playerNecessities._numOfProjectors++;
            }
            _playerNecessities._pickup.Play();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerIn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        _playerIn = false;
    }
}
