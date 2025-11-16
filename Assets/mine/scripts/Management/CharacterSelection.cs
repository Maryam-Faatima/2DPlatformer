using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] charactersArray;
    public int currentCharacterIndex=0 ;
    public int previousCharacterIndex=-1;
    public static int selectedCharacterIndex=0;


    public void selectCharacter()
    {
        
        charactersArray[currentCharacterIndex].gameObject.SetActive(true);
        charactersArray[previousCharacterIndex].gameObject.SetActive(false);
        selectedCharacterIndex = currentCharacterIndex;
    }

    public void leftArrow()
    { 
        previousCharacterIndex = currentCharacterIndex;
        currentCharacterIndex--;
        if(currentCharacterIndex < 0)
        {
            currentCharacterIndex = charactersArray.Length - 1;
        }
        selectCharacter();
    }

    public void rightArrow()
    {
        previousCharacterIndex = currentCharacterIndex;
        currentCharacterIndex++;
        if(currentCharacterIndex >= charactersArray.Length-1)
        {
            currentCharacterIndex = 0;
        }
        selectCharacter();
    }

        // Start is called before the first frame update
    void Start()
    {
    //selectCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
