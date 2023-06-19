using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    [SerializeField] private GameObject floor00;
    [SerializeField] private GameObject floor01;
    [SerializeField] private GameObject floor02;
    [SerializeField] private GameObject floor10;
    [SerializeField] private GameObject floor11;
    [SerializeField] private GameObject floor12;
    [SerializeField] private GameObject floor20;
    [SerializeField] private GameObject floor21;
    [SerializeField] private GameObject floor22;
    
    private GameObject[,] mxGrid = new GameObject[3,3];
    private int floorLen = 42;
    private void Start()
    {
        mxGrid[0, 0] = Instantiate(floor00, new Vector3(-floorLen, floorLen, 0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[0, 1] = Instantiate(floor01, new Vector3(0, floorLen, 0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[0, 2] = Instantiate(floor02, new Vector3(floorLen, floorLen, 0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[1, 0] = Instantiate(floor10, new Vector3(-floorLen, 0,0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[1, 1] = Instantiate(floor11, new Vector3(0, 0, 0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[1, 2] = Instantiate(floor12, new Vector3(floorLen, 0,0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[2, 0] = Instantiate(floor20, new Vector3(-floorLen, -floorLen, 0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[2, 1] = Instantiate(floor21, new Vector3(0, -floorLen, 0), Quaternion.Euler(0,0,0), gameObject.transform);
        mxGrid[2, 2] = Instantiate(floor22, new Vector3(floorLen, -floorLen, 0), Quaternion.Euler(0,0,0), gameObject.transform);
    }

    public void ChangeCenter(int x, int y)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                mxGrid[i, j].transform.position += new Vector3(x * floorLen, y * floorLen);
            }
        }
    }
}
