using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSelector : MonoBehaviour
{
    //Array created without specifc size to dynamically change size and store cars
    private GameObject[] _carList; //Naming is important. Do not use the same name for the instance as the original Game object, which is "cars" in this case.

    private int currentCar = 0; //Variable to keep track of the current car selected. usedto program the buttons.

    // Start is called before the first frame update
    void Start()
    {
        _carList = new GameObject[transform.childCount];

        //for loop created to assign each object to a an element in dynamic array named car
        for (int i = 0; i<transform.childCount; ++i)
        {
            _carList[i] = transform.GetChild(i).gameObject; //.gameObject used to convert transform type to game object type
            // tried - cars[i].SetActive(false); 
        }

        foreach (GameObject gameObj in _carList)
        {
            gameObj.SetActive(false); //Intentionally set all the elements to False
        }

        if (_carList[0]) //Executes if there is an object available in element 1, which is obvious.
        {
            _carList[0].SetActive(true); // The 1st element is assigned True, so only this appears by default.
        }
    }

    public void carToggle (string Direction)
    {
        _carList[currentCar].SetActive(false); //Disable the whatever car currently selected

        if (Direction == "Left")
        {
            currentCar--; //To moveleft in the array

            if (currentCar < 0) //array value -1 will return error because out of range
            {
                currentCar = _carList.Length - 1;
            }
        }

        else if (Direction == "Right")
        {
            currentCar++;

            if (currentCar > _carList.Length - 1) //array value is greater than array length this will return error because out of range
            {
                currentCar = 0;
            }
        }

        _carList[currentCar].SetActive(true); //Enable the next car applicable

        GameController.currentSelectedCar = _carList[currentCar].name; //".name" used because _carList[currentCar] is GameObject type variable. This cannot be assigned to static string.
    }
}