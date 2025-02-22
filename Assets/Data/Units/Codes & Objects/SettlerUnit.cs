using UnityEngine;

// Ensure SettlerUnit derives from Unit (which should be your common base class for all units).
public class SettlerUnit : Unit
{
    public GameObject cityPrefab; // Assign your City prefab in the Inspector.
    public bool isSelected = false; // Flag to indicate if the unit is selected.

    // Called when the settler unit chooses to found a city.
    public void FoundCity()
    {
        if (cityPrefab == null)
        {
            Debug.LogError("City prefab is not assigned in SettlerUnit!");
            return;
        }

        // Instantiate the city at the settler's current position.
        GameObject newCityGO = Instantiate(cityPrefab, transform.position, Quaternion.identity);
        City newCity = newCityGO.GetComponent<City>();

        if (newCity != null)
        {
            // Set up the new city using the owner from this unit.
            newCity.cityName = owner.baseCiv.civName + " City";
            newCity.owner = owner;

            // Founding the city naturally triggers the city's Start() method.
            // Remove the settler unit.
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("The city prefab does not have a City component attached!");
        }
    }

    // Update method to detect the 'F' key press.
    protected override void Update()
    {
        // Check if the 'F' key is pressed and the unit is selected.
        if (Input.GetKeyDown(KeyCode.F) && isSelected)
        {
            // Call the FoundCity method to replace the settler with a city.
            FoundCity();
        }
    }

    // Method to select the unit.
    public void SelectUnit()
    {
        isSelected = true;
    }

    // Method to deselect the unit.
    public void DeselectUnit()
    {
        isSelected = false;
    }
}