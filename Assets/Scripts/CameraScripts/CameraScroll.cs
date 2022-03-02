using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CameraScroll : MonoBehaviour, IDragHandler
{
    [SerializeField] private float      scrollSpeed        = 1.0F;
    [SerializeField] private float      parallaxMultiplier = 0.1F;
    [SerializeField] private GameObject DayScrollSprite    = null;
    [SerializeField] private GameObject NightScrollSprite  = null;

    private Camera     _camera;
    private Vector3    _cameraInitialPosition = Vector3.zero;
    private GameObject _background;
    private Vector3    _backgroundInitialPosition;
    private GameObject _bound;
    private float      _boundLeft;
    private float      _boundRight;

    /*
     * Mono Behavior.
     */

    private void Awake()
    {
        _camera = Camera.main;
        _cameraInitialPosition = _camera.transform.position;
        
        _background = DayScrollSprite.transform.parent.gameObject.activeSelf ? DayScrollSprite : NightScrollSprite;
        _backgroundInitialPosition = _background.transform.position;
    }

    /*
     * Public.
     */

    public void Initialize()
    {
        _camera.transform.position = _cameraInitialPosition;

        _bound = GameObject.Find("Land_Foreground_0");
        _background = DayScrollSprite.transform.parent.gameObject.activeSelf ? DayScrollSprite : NightScrollSprite;
        _background.transform.position = _backgroundInitialPosition;

        var verExtent = _camera.orthographicSize;
        var horExtent = verExtent * Screen.width / Screen.height;
        var sprite = _bound.GetComponent<SpriteRenderer>().sprite;

        _boundLeft = horExtent - sprite.bounds.size.x / 2.0f;
        _boundRight = sprite.bounds.size.x / 2.0f - horExtent;
    }

    /*
     * Drag.
     */

    public void OnDrag(PointerEventData eventData)
    {
        var xDeltaPercentage = eventData.delta.x / _camera.pixelWidth;

        var width = Math.Abs(_boundLeft) + Math.Abs(_boundRight) * scrollSpeed;
        var move = new Vector3(width * xDeltaPercentage, 0, 0);

        _camera.transform.Translate(-move, Space.World);

        if (!ClampPosition())
            _background.transform.Translate(-move * parallaxMultiplier, Space.World);
    }

    /*
     * Private.
     */

    private bool ClampPosition()
    {
        var position = _camera.transform.position;
        var posX = position.x;
        if (posX < _boundLeft)
        {
            _camera.transform.position = new Vector3(_boundLeft, position.y, position.z);
            return true;
        }

        if (posX > _boundRight)
        {
            _camera.transform.position = new Vector3(_boundRight, position.y, position.z);
            return true;
        }

        return false;
    }
}