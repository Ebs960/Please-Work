using UnityEngine;

public class Tribe : MonoBehaviour
{
    public string tribeName;
    public int population;
    public int defense;
    public int production;
    public int gold;
    public int food;
    public int science;
    public int culture;
    public int manpower;

    public void Initialize(string name, Vector3 position, Vector2 populationRange, int defense, int production, int gold, int food, int science, int culture, int manpower)
    {
        tribeName = name;
        population = Random.Range((int)populationRange.x, (int)populationRange.y);
        this.defense = defense;
        this.production = production;
        this.gold = gold;
        this.food = food;
        this.science = science;
        this.culture = culture;
        this.manpower = manpower;

        transform.position = position;
        transform.name = tribeName;
    }
}