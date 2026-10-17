using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    InputAction leftMouse;
    
    public GameObject sweetObj;
    public GameObject meatObj;
    public List<GameObject> allFood = new List<GameObject>();
    
    public Collider2D sweetButton;
    public Collider2D meatButton; 
    
    enum FoodType { Sweet, Meat }
    FoodType current = FoodType.Sweet;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftMouse = InputSystem.actions.FindAction("MouseClick");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftMouse.WasReleasedThisFrame())
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            pos.z = 0;

            //hit the buttons or not?
            if (sweetButton.OverlapPoint(pos))
            {
                current = FoodType.Sweet;
                return;
            }
            if (meatButton.OverlapPoint(pos))
            {
                current = FoodType.Meat;
                return;
            }

            MakeFood(pos);
        }
    }

    void MakeFood(Vector3 pos)
    {
        if (current == FoodType.Sweet)
        {
            allFood.Add(Instantiate(sweetObj, pos, Quaternion.identity));
        }
        else
        {
            Instantiate(meatObj, pos, Quaternion.identity);
        }
    }
}
