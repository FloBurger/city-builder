using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GridSystem grid;

    public Material red, green;
    public GameObject cubeShadow, cube, pathX, pathY, pathOpen, pathBot, pathTop, pathLeft, pathRight;
    public int gridX, gridZ;
    public float cellSize;
    public Vector3 originVec;
    private GameObject cubeShadowInstance;
    private Vector3 storeVec;
    private int storeDist = 0;
    private List<GameObject> goList = new List<GameObject>();
    private bool closeToX = true;

    private void Awake()
    {
        cubeShadowInstance = GameObject.Instantiate(cubeShadow);
        cubeShadowInstance.transform.position = new Vector3(0, -10, 0);
    }
    private void Start()
    {
        grid = new GridSystem(gridX, gridZ, cellSize, originVec);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StoreStartPos(GetBuildVec(GetMouseWorldPosition()));
            //   Debug.Log(grid.getValue(GetMouseWorldPosition()));
        }
        if (Input.GetMouseButton(0))
        {
            GetAxes(GetBuildVec(GetMouseWorldPosition()));
            if (closeToX)
            {
                HoverStreetOnX(GetBuildVec(GetMouseWorldPosition()));
            }
            else
            {
                HoverStreetOnZ(GetBuildVec(GetMouseWorldPosition()));
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            goList.Clear();
            //BuildCube(GetBuildVec(GetMouseWorldPosition()));
        }
    }

    public static Vector3 GetMouseWorldPosition()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //GetMouseWorldPositionWityY(hitInfo.point, Camera.main)
            Vector3 vec = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
            vec.y = 0f;
            //Debug.Log(vec);
            return vec;
        }
        return new Vector3(0, 0, 0);
    }
    public static Vector3 GetMouseWorldPositionWityY(Vector3 screenPos, Camera worldCamera)
    {
        Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPos);
        return worldPos;
    }

    private Vector3 GetBuildVec(Vector3 clickPoint)
    {
        Vector3 finalPosition = grid.GetGridPosition(new Vector3(clickPoint.x, clickPoint.y, clickPoint.z));
        //finalPosition = new Vector3(finalPosition.x + 0.5f, finalPosition.y + 0.5f, finalPosition.z + 0.5f);
        finalPosition = new Vector3(finalPosition.x + cellSize / 2, finalPosition.y, finalPosition.z + cellSize / 2);
        return finalPosition;
    }

    private void HoverCube(Vector3 clickPoint)
    {

        Vector3 finalPosition = new Vector3(clickPoint.x, clickPoint.y, clickPoint.z);
        cubeShadowInstance.transform.position = finalPosition;
        if (grid.getValue(GetMouseWorldPosition()) != 0)
            cubeShadowInstance.transform.GetComponent<Renderer>().material = red;
        else
            cubeShadowInstance.transform.GetComponent<Renderer>().material = green;

    }

    private void BuildCube(Vector3 clickPoint)
    {
        if (grid.getValue(GetMouseWorldPosition()) == 0)
        {
            Vector3 finalPosition = new Vector3(clickPoint.x, clickPoint.y, clickPoint.z);
            GameObject c = GameObject.Instantiate(cube);
            c.transform.position = finalPosition;
            grid.SetValue(finalPosition, 1);
        }
    }

    /*********************************************
    Start Node == 9
    X Node == 1
    Z Node == 2
    **********************************************/

    private void HoverStreetOnX(Vector3 clickPoint)
    {
        Vector3 distVec = clickPoint - storeVec;
        int tempSize = (int)cellSize;
        //Debug.Log(clickPoint + ":" + storeVec);
        if (grid.getValue(GetMouseWorldPosition()) == 0)
        {
            if (distVec.x >= 0)
            {
                //Debug.Log(distVec + "=" + clickPoint + "-" + storeVec);
                int dist = (int)distVec.x;
                if (storeDist != dist || storeDist == 0)
                {
                    storeDist = dist;
                    //Debug.Log(dist);
                    Vector3 finalPosition = new Vector3(clickPoint.x, clickPoint.y, clickPoint.z);

                    if (dist == 0)
                    {
                        GameObject c = GameObject.Instantiate(pathOpen);
                        //Debug.Log(finalPosition);
                        c.transform.position = finalPosition;
                        grid.SetValue(finalPosition, 9);
                        goList.Add(c);
                    }
                    else
                    {
                        if (grid.getValue(storeVec) == 9 || grid.getValue(storeVec) == 1)
                        {
                            for (int i = 0; i < dist; i++)
                            {
                                if ((i % tempSize == 0))
                                {

                                    Vector3 tempVec = new Vector3(storeVec.x + i + tempSize, storeVec.y, storeVec.z);
                                    if (grid.getValue(tempVec) == 0)
                                    {
                                        GameObject c = GameObject.Instantiate(pathX);
                                        c.transform.position = tempVec;
                                        grid.SetValue(tempVec, 1);
                                        goList.Add(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (distVec.x < 0)
            {

                int dist = ((int)distVec.x) * -1;
                if (storeDist != dist || storeDist == 0)
                {
                    storeDist = dist;

                    Vector3 finalPosition = new Vector3(clickPoint.x, clickPoint.y, clickPoint.z);

                    if (dist == 0)
                    {
                        GameObject c = GameObject.Instantiate(pathX);

                        c.transform.position = finalPosition;
                        grid.SetValue(finalPosition, 1);
                        goList.Add(c);
                    }
                    else
                    {
                        if (grid.getValue(storeVec) == 9 || grid.getValue(storeVec) == 1)
                        {
                            for (int i = 0; i < dist; i++)
                            {
                                Vector3 tempVec = new Vector3(storeVec.x - i - 1, storeVec.y, storeVec.z);

                                if (grid.getValue(tempVec) == 0)
                                {

                                    GameObject c = GameObject.Instantiate(pathX);
                                    c.transform.position = tempVec;
                                    grid.SetValue(tempVec, 1);
                                    goList.Add(c);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (distVec.x >= 0)
        {
            Debug.Log(distVec.x + tempSize + ";" + goList.Count * tempSize);
            if (distVec.x + tempSize < goList.Count * tempSize)
            {
                grid.SetValue(goList[goList.Count - 1].transform.position, 0);
                Destroy(goList[goList.Count - 1]);
                goList.RemoveAt(goList.Count - 1);
            }
        }
        if (distVec.x < 0)
        {
            int dist = ((int)distVec.x) * -1;
            if (dist + tempSize < goList.Count * tempSize)
            {
                grid.SetValue(goList[goList.Count - 1].transform.position, 0);
                Destroy(goList[goList.Count - 1]);
                goList.RemoveAt(goList.Count - 1);
            }

        }

    }

    private void HoverStreetOnZ(Vector3 clickPoint)
    {
        Vector3 distVec = clickPoint - storeVec;
        int tempSize = (int)cellSize;
        if (grid.getValue(GetMouseWorldPosition()) == 0)
        {
            if (distVec.z >= 0)
            {
                //Debug.Log(distVec + "=" + clickPoint + "-" + storeVec);
                int dist = (int)distVec.z;
                if (storeDist != dist || storeDist == 0)
                {
                    storeDist = dist;
                    //Debug.Log(dist);
                    Vector3 finalPosition = new Vector3(clickPoint.x, clickPoint.y, clickPoint.z);

                    if (dist == 0)
                    {
                        GameObject c = GameObject.Instantiate(pathY);
                        //Debug.Log(finalPosition);
                        c.transform.position = finalPosition;
                        grid.SetValue(finalPosition, 9);
                        goList.Add(c);
                    }
                    else
                    {
                        if (grid.getValue(storeVec) == 9 || grid.getValue(storeVec) == 2)
                        {
                            for (int i = 0; i < dist; i++)
                            {
                                if ((i % tempSize == 0))
                                {

                                    Vector3 tempVec = new Vector3(storeVec.x, storeVec.y, storeVec.z + i + tempSize);
                                    if (grid.getValue(tempVec) == 0)
                                    {
                                        GameObject c = GameObject.Instantiate(pathY);
                                        c.transform.position = tempVec;
                                        grid.SetValue(tempVec, 2);
                                        goList.Add(c);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (distVec.z < 0)
            {
                //Debug.Log(distVec + "=" + clickPoint + "-" + storeVec);
                int dist = ((int)distVec.z) * -1;
                if (storeDist != dist || storeDist == 0)
                {
                    storeDist = dist;
                    //Debug.Log(dist);
                    Vector3 finalPosition = new Vector3(clickPoint.x, clickPoint.y, clickPoint.z);

                    if (dist == 0)
                    {
                        GameObject c = GameObject.Instantiate(pathY);
                        //Debug.Log(finalPosition);
                        c.transform.position = finalPosition;
                        grid.SetValue(finalPosition, 1);
                        goList.Add(c);
                    }
                    else
                    {
                        if (grid.getValue(storeVec) == 9 || grid.getValue(storeVec) == 2)
                        {
                            for (int i = 0; i < dist; i++)
                            {
                                Vector3 tempVec = new Vector3(storeVec.x, storeVec.y, storeVec.z - i - 1);
                                //Debug.Log(tempVec);
                                if (grid.getValue(tempVec) == 0)
                                {
                                    //Debug.Log(tempVec);
                                    GameObject c = GameObject.Instantiate(pathY);
                                    c.transform.position = tempVec;
                                    grid.SetValue(tempVec, 2);
                                    goList.Add(c);
                                }
                            }
                        }
                    }
                }
            }
        }
        //Debug.Log(distVec.x + ":" + goList.Count);
        if (distVec.z >= 0)
        {
            if (distVec.z + tempSize < goList.Count * tempSize)
            {
                grid.SetValue(goList[goList.Count - 1].transform.position, 0);
                Destroy(goList[goList.Count - 1]);
                goList.RemoveAt(goList.Count - 1);
            }
        }
        if (distVec.z < 0)
        {
            int dist = ((int)distVec.z) * -1;
            if (dist + tempSize < goList.Count * tempSize)
            {
                grid.SetValue(goList[goList.Count - 1].transform.position, 0);
                Destroy(goList[goList.Count - 1]);
                goList.RemoveAt(goList.Count - 1);
            }

        }

    }

    void GetAxes(Vector3 clickPoint)
    {
        Vector3 distVec = clickPoint - storeVec;
        int distz = (int)distVec.z;
        int distx = (int)distVec.x;
        if (distx < 0)
            distx = distx * -1;
        if (distz < 0)
            distz = distz * -1;

        //Debug.Log(scalarX);
        if (distx >= distz)
        {
            closeToX = true;
        }
        else
        {
            closeToX = false;
        }
        Debug.Log(closeToX);
    }

    private void StoreStartPos(Vector3 clickPoint)
    {
        storeVec = clickPoint;
        Debug.Log("start" + clickPoint);
    }

    private Vector3[] getSurrPos()
    {
        Vector3 topLeft = new Vector3(storeVec.x, storeVec.y, storeVec.z + 1);
        Vector3 topRight = new Vector3(storeVec.x + 1, storeVec.y, storeVec.z + 1);
        Vector3 botRight = new Vector3(storeVec.x + 1, storeVec.y, storeVec.z);
        Vector3[] positionArray = new[] { topLeft, topRight, botRight };
        return positionArray;
    }

}
