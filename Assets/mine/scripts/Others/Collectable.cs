using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum FruitType
    {
        Apple,
        Banana,
        Kiwi,
        Orange,
        Cherry,
        Melon,
        Pineapple,
        Strawberry
    }

    public FruitType fruitType;
    private int fruitScore;

    private void Start()
    {
        switch (fruitType)
        {
            case FruitType.Apple:
                fruitScore = 1;
                break;
            case FruitType.Banana:
                fruitScore = 2;
                break;
            case FruitType.Kiwi:
                fruitScore = 3;
                break;
            case FruitType.Orange:
                fruitScore = 4;
                break;
            case FruitType.Cherry:
                fruitScore = 5;
                break;
            case FruitType.Melon:
                fruitScore = 6;
                break;
            case FruitType.Pineapple:
                fruitScore = 7;
                break;
            case FruitType.Strawberry:
                fruitScore = 8;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.AddScore(fruitScore);

            // Later: play animation or sound
            Destroy(gameObject);
        }
    }
}
