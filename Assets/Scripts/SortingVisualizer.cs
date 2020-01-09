using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingVisualizer : MonoBehaviour {

    public int numberOfCubes = 10;
    public int cubeHeightMax = 10;
    public GameObject[] cubes;
    public Slider slider;
    public Button[] sortOptionButtons;

    private bool selectionSort = false;

    private void Start() {
        GenerateRandomArray();
        //StartCoroutine(SelectionSort(cubes));
    }

    public void GenerateRandomArray() {
        // Delete if already initialized
        ResetArray();

        numberOfCubes = (int)slider.value;
        cubes = new GameObject[numberOfCubes];
        
        for (int i = 0; i < numberOfCubes; i++) {
            int randomNumber = Random.Range(1, cubeHeightMax + 1);

            // Experiment with the x values of cubes[i] x-scale and x-position!
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(.5f, randomNumber, .5f);
            cube.transform.position = new Vector3(i*.6f, randomNumber / 2f, 0);

            cube.transform.parent = this.transform; // Cubes = parent

            cubes[i] = cube;
        }
        // Experiment with the x values of Cubes.position based off the x-scale and x-position of each cube[i]!
        transform.position = new Vector3(-numberOfCubes / 3.5f, -cubeHeightMax / 2f, 0);
    }

    public void ResetArray() {
        for (int i = 0; i < cubes.Length; i++) {
            if (cubes[i] != null) {
                Destroy(cubes[i]);
            }
        }
        transform.position = new Vector3(0, 0, 0); // Reset position of Cubes gameObject
    }

    public void Sort() {
        if (selectionSort) {
            StartCoroutine(SelectionSort(cubes));
        }
        selectionSort = false;
        sortOptionButtons[0].interactable = true;
    }

    public void ChooseSelectionSort() {
        selectionSort = true;
        sortOptionButtons[0].interactable = false;
    }

    private IEnumerator SelectionSort(GameObject[] unsortedList) {
        int min;
        GameObject temp;
        Vector3 tempPosition;

        for (int i = 0; i < unsortedList.Length; i++) {
            min = i;
            yield return new WaitForSeconds(1); // Delay for 1 second
            for (int j = i + 1; j < unsortedList.Length; j++) {
                
                if (unsortedList[j].transform.localScale.y < unsortedList[min].transform.localScale.y) {
                    min = j;
                }
            }
            if (min != i) {
                yield return new WaitForSeconds(1f); // Experiment with this!

                temp = unsortedList[i];
                unsortedList[i] = unsortedList[min];
                unsortedList[min] = temp;

                tempPosition = unsortedList[i].transform.localPosition;

                /*  
                 * Use these 2 lines if you are not using LeanTween!
                 * unsortedList[i].transform.localPosition = new Vector3(unsortedList[min].transform.localPosition.x, tempPosition.y, tempPosition.z);
                 * unsortedList[min].transform.localPosition = new Vector3(tempPosition.x, unsortedList[min].transform.localPosition.y, unsortedList[min].transform.localPosition.z);
                */
                
                LeanTween.moveLocalX(unsortedList[i], unsortedList[min].transform.localPosition.x, 1);
                LeanTween.moveLocalZ(unsortedList[i], -3, .5f).setLoopPingPong(1);

                LeanTween.moveLocalX(unsortedList[min], tempPosition.x, 1);
                LeanTween.moveLocalZ(unsortedList[min], 3, .5f).setLoopPingPong(1);
            }

            LeanTween.color(unsortedList[i], Color.green, 1f);
            
        }
        
    }
}