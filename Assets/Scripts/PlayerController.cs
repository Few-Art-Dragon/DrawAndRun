using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    

    public static UnityEvent<Vector3[]> SetNewPositionPlayerEvent = new UnityEvent<Vector3[]>();
    [SerializeField] private GameObject[] _players;
    [SerializeField] private float _speed;

    private void Start()
    {
        SetNewPositionPlayerEvent.AddListener(SetNewPositionPlayer);
    }
    private void Update()
    {
        MovePlayer();
    }
    private void SetNewPositionPlayer(Vector3[] position)
    {
        //if (position.Length % 2 == 0)
        //{

        //}
        //else
        //{
            
        //}
        
        for (int i = 0; i < _players.Length; i++)
        {
            if (position.Length > i)
            {
                //if ()
                //{
                    
                //}
                _players[i].transform.position = new Vector3(position[i+2].x, _players[i].transform.position.y, _players[i].transform.position.z);
            }
            else
            {
                break;
            }
            
        }

    }

    private void MovePlayer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + _speed * Time.deltaTime);  
    }

    
}
