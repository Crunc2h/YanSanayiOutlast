using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    private BoxCollider _boxColl;
    private GameObject _player;
    public bool _locked = true;
    private bool _playerIn = false;
    [SerializeField] private GameObject _transportPoint;
    private GameObject _interumPanel;
    [SerializeField] private AudioSource _lockedSFX;
    [SerializeField] private AudioSource _unlockedSFX;
    void Start()
    {
        _boxColl = GetComponent<BoxCollider>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _interumPanel = GameObject.FindGameObjectWithTag("InterumPanel");
    }

    
    void Update()
    {
        if(gameObject.tag != "PermaLock")
        {
            if (!_locked && Input.GetKeyDown(KeyCode.E) && _playerIn)
            {
                StartInterum();
            }
            else if (Input.GetKeyDown(KeyCode.E) && _playerIn && _player.GetComponent<PlayerNecessities>()._hasKey)
            {
                StartInterum();
            }
            else if (Input.GetKeyDown(KeyCode.E) && _playerIn && !_player.GetComponent<PlayerNecessities>()._hasKey)
            {
                _lockedSFX.Play();
            }
        }
        else
        {
            
            if(_player.GetComponent<PlayerNecessities>()._numOfProjectors == 3 &&
                Input.GetKeyDown(KeyCode.E) && _playerIn) { StartInterum(); }
            else if (Input.GetKeyDown(KeyCode.E) && _playerIn)
            {
                _lockedSFX.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") _playerIn = true;
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") _playerIn = false;
    }

    private void Transport(GameObject objectToTransport)
    {
        objectToTransport.transform.position = new Vector3(_transportPoint.transform.position.x, objectToTransport.transform.position.y, 
            _transportPoint.transform.position.z) ;
    }


    public void StartInterum() => StartCoroutine(DoInterum());

    private IEnumerator DoInterum()
    {
        float originalA = GetColor().a;
        float currentAlpha = GetColor().a;
        _unlockedSFX.Play();
        if(_locked)
        {
            _player.GetComponent<PlayerNecessities>()._hasKey = false;
            _locked = false;
        }
        while (currentAlpha < 1)
        {
            var currentCol = GetColor();
            var currentA = currentCol.a;
            var targetA = currentA + 0.01f;
            var newCol = new Color(currentCol.r, currentCol.g, currentCol.b, targetA);
            SetA(newCol);
            currentAlpha = targetA;
            yield return new WaitForSeconds(0.01f);
        }
        Transport(_player);
        while (currentAlpha > originalA)
        {
            var currentCol = GetColor();
            var currentA = currentCol.a;
            var targetA = currentA - 0.01f;
            var newCol = new Color(currentCol.r, currentCol.g, currentCol.b, targetA);
            SetA(newCol);
            currentAlpha = targetA;
            yield return new WaitForSeconds(0.01f);
        }

    }


    private Color GetColor() => _interumPanel.GetComponent<Image>().color;
    private void SetA(Color newColor) => _interumPanel.GetComponent<Image>().color = newColor;
}
