
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerNecessities : MonoBehaviour
{
    public bool _hasKey = false;
    public int _numOfProjectors = 0;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _mouseSensitivity = 2f;
    private float _inputScaleX;
    private float _inputScaleZ;
    private float _yaw;
    private float _pitch;
    private Rigidbody _rb;
    private GameObject _camera;
    private Light _spot;
    public AudioSource _pickup;
    public AudioSource _walk;
    public AudioSource _flashlight;
    private TextMeshProUGUI _numP;
    private UnityEngine.UI.Image _keyImg;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        _spot = _camera.GetComponent<Light>();
        _keyImg = GameObject.FindGameObjectWithTag("Keyimg").GetComponent<UnityEngine.UI.Image>();
        _numP = GameObject.FindGameObjectWithTag("Pnumber").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _inputScaleX = Input.GetAxisRaw("Horizontal");
        _inputScaleZ = Input.GetAxisRaw("Vertical");
        _yaw = Input.GetAxisRaw("Mouse X") * _mouseSensitivity;
        _pitch = Input.GetAxisRaw("Mouse Y") * _mouseSensitivity * (-1);

        var _cameraForwardVector = _camera.transform.forward * _inputScaleZ * _movementSpeed;
        var _cameraSidewaysVector = _camera.transform.right.normalized * _inputScaleX * _movementSpeed;
        var _movementVector = _cameraForwardVector + _cameraSidewaysVector;

        _rb.velocity = new Vector3(_movementVector.x, _rb.velocity.y, _movementVector.z);
               
        _camera.transform.eulerAngles = new Vector3(_camera.transform.eulerAngles.x + _pitch, _camera.transform.eulerAngles.y + _yaw, _camera.transform.eulerAngles.z);

        if(Input.GetKeyDown(KeyCode.F))
        {
            _flashlight.Play();
            if(_spot.enabled)
            {
                _spot.enabled = false;
            }
            else
            {
                _spot.enabled = true;
            }
        }

        if(_hasKey && !_keyImg.enabled)
        {
            _keyImg.enabled = true;
        }
        else if(!_hasKey && _keyImg.enabled)
        {
            _keyImg.enabled = false;
        }

        if(_numP.text != _numOfProjectors.ToString())
        {
            _numP.text = _numOfProjectors.ToString();
        }

        if(_inputScaleX != 0 || _inputScaleZ != 0)
        {
            if(!_walk.isPlaying)
            {
                _walk.Play();
            }
        }
        else if(_inputScaleX == 0 && _inputScaleZ == 0 && _walk.isPlaying)
        {
            _walk.Stop();
        }
    }


}
