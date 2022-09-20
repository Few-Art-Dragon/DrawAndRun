using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Pen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField] private float _angleCamera;
    [SerializeField] private Vector3[] _positionDots;
    [SerializeField] private Camera _maincamera;
    [SerializeField] private GameObject _brush;

    [SerializeField] private LineRenderer _currentLineRenderer;

    private Vector3 _lastPosition;

    private void Draw(Vector3 position)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {

            position.z = 7;
            Vector3 mousePos = _maincamera.ScreenToWorldPoint(position);

            if (mousePos != _lastPosition)
            {
                AddPoint(mousePos);
                _lastPosition = mousePos;
            }
            else
            {
                //_currentLineRenderer = null;
            }
        }


    }

    private void CreateBrush(Vector3 position)
    {

        GameObject brushInstance = Instantiate(_brush);
        _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        position.z = 7;
        Vector3 mousePosition = _maincamera.ScreenToWorldPoint(position);

        _currentLineRenderer.SetPosition(0, mousePosition);
        _currentLineRenderer.SetPosition(1, mousePosition);
    }

    private void DeleteLine()
    {
        _positionDots = new Vector3[_currentLineRenderer.positionCount];
        _currentLineRenderer.GetPositions(_positionDots);
        SearchCameraAngle();
        Vector3 smallDot;
        smallDot = _positionDots[0];

        foreach (var item in _positionDots)
        {
            if (item.y < smallDot.y)
            {
                smallDot = item;
            }
        }


        foreach (var item in _positionDots)
        {
            
            Debug.Log(SearchNewPoint(smallDot , item ));

        }
        
        PlayerController.SetNewPositionPlayerEvent.Invoke(_positionDots);
        
        //Destroy(_currentLineRenderer.gameObject);
        //_currentLineRenderer = null;
    }

    private void AddPoint(Vector3 pointPos)
    {
        _currentLineRenderer.positionCount++;
        int positionIndex = _currentLineRenderer.positionCount - 1;
        _currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CreateBrush(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       DeleteLine();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Draw(eventData.position);
    }

    private void SearchCameraAngle()
    {
        _angleCamera = -(90f - _maincamera.transform.rotation.x) / 180;
    }

    private Vector3 SearchNewPoint(Vector3 anchorPoint, Vector3 position)
    {
        Vector3 relativePosition = position - anchorPoint;
        Vector3 columnX = new Vector3(1,0,0);
        Vector3 columnY = new Vector3(0, Mathf.Cos(_angleCamera), -Mathf.Sin(_angleCamera) );
        Vector3 columnZ = new Vector3(0, Mathf.Sin(_angleCamera), Mathf.Cos(_angleCamera));
        Vector3 relativeDistance = new Vector3(anchorPoint.x, 2f, -7f);
        //Vector3 relativeDistance = new Vector3(anchorPoint.x, -0.194f, -0.86f) - anchorPoint;

        Vector3 newPoint =  (relativePosition.x * columnX) + (relativePosition.y * columnY) + (relativePosition.z * columnZ);


        newPoint += relativeDistance;
        return newPoint;
    }

}
