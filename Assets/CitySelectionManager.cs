using UnityEngine;

public class CitySelectionManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the camera to the mouse position.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has a City component.
                City city = hit.transform.GetComponent<City>();
                if (city != null)
                {
                    Debug.Log("City selected: " + city.cityName);
                    if (GameUI.Instance != null)
                    {
                        GameUI.Instance.ShowCityPanel(city);
                    }
                    else
                    {
                        Debug.LogError("GameUI instance is null!");
                    }
                }
            }
        }
    }
}
