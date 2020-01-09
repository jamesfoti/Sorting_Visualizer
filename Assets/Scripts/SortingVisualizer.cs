using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingVisualizer : MonoBehaviour {

    public int numberOfCubes = 10;
    public int cubeHeightMax = 10;
    public GameObject[] cubes;
    public Slider sizeSlider;
    public Slider speedSlider;
    public Button[] sortOptionButtons;
    private float sortingSpeed;

    private bool selectionSort = false;
    private bool bubbleSort = false;

    private void Start() {
        GenerateRandomArray();
        //StartCoroutine(SelectionSort(cubes));
    }

    public void GenerateRandomArray() {
        // Delete if already initialized
        ResetArray();

        numberOfCubes = (int)sizeSlider.value;
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
        sortingSpeed = Mathf.Abs(speedSlider.value - 1f); // Slider values: .01 to 1.01

        if (selectionSort) {
            StartCoroutine(SelectionSort(cubes));
        }
        else if (bubbleSort) {
            StartCoroutine(BubbleSort(cubes));
        }
        selectionSort = false;
        bubbleSort = false;
        sortOptionButtons[0].interactable = true;
        sortOptionButtons[1].interactable = true;
    }

    public void Pause() {
        Time.timeScale = 0f;
    }

    public void Resume() {
        Time.timeScale = 1f;
        sortingSpeed = Mathf.Abs(speedSlider.value - 1f); // Slider values: .01 to 1.01
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
            LeanTween.color(unsortedList[i], Color.blue, sortingSpeed);
            min = i;
            yield return new WaitForSeconds(sortingSpeed); // Delay for 1 second

            for (int j = i + 1; j < unsortedList.Length; j++) {
                LeanTween.color(unsortedList[j], Color.red, sortingSpeed);
                yield return new WaitForSeconds(sortingSpeed);
                LeanTween.color(unsortedList[j], Color.white, sortingSpeed);

                if (unsortedList[j].transform.localScale.y < unsortedList[min].transform.localScale.y) {
                    min = j;
                }
            }
            if (min != i) {
                yield return new WaitForSeconds(sortingSpeed); // Experiment with this!

                temp = unsortedList[i];
                unsortedList[i] = unsortedList[min];
                unsortedList[min] = temp;

                tempPosition = unsortedList[i].transform.localPosition;

                /*  
                 * Use these 2 lines if you are not using LeanTween!
                 * unsortedList[i].transform.localPosition = new Vector3(unsortedList[min].transform.localPosition.x, tempPosition.y, tempPosition.z);
                 * unsortedList[min].transform.localPosition = new Vector3(tempPosition.x, unsortedList[min].transform.localPosition.y, unsortedList[min].transform.localPosition.z);
                */

                LeanTween.moveLocalX(unsortedList[i], unsortedList[min].transform.localPosition.x, sortingSpeed);
                LeanTween.moveLocalZ(unsortedList[i], -3, sortingSpeed / 2f).setLoopPingPong(1);


                LeanTween.moveLocalX(unsortedList[min], tempPosition.x, sortingSpeed);
                LeanTween.moveLocalZ(unsortedList[min], 3, sortingSpeed / 2f).setLoopPingPong(1);
                LeanTween.color(unsortedList[min], Color.white, sortingSpeed);

            }
            LeanTween.color(unsortedList[i], Color.green, sortingSpeed); // Turn sorted items green
        }
    }
    public void ChooseBubbleSort() {
        bubbleSort = true;
        sortOptionButtons[1].interactable = false;
    }


    private IEnumerator BubbleSort(GameObject[] unsortedList) {
        int i, j;
        GameObject temp;
        Vector3 tempPosition;
        bool swapped;

        for (i = 0; i < unsortedList.Length - 1; i++) {
            swapped = false;
            for (j = 0; j < unsortedList.Length - i - 1; j++) {
                if (unsortedList[j].transform.localScale.y > unsortedList[j + 1].transform.localScale.y) {
                    yield return new WaitForSeconds(sortingSpeed);
                    // swap arr[j] and arr[j+1] 
                    temp = unsortedList[j];
                    unsortedList[j] = unsortedList[j + 1];
                    unsortedList[j + 1] = temp;

                    tempPosition = unsortedList[j].transform.localPosition;

                    LeanTween.moveLocalX(unsortedList[j], unsortedList[j + 1].transform.localPosition.x, sortingSpeed);
                    LeanTween.moveLocalZ(unsortedList[j], -3, sortingSpeed / 2f).setLoopPingPong(1);

                    LeanTween.moveLocalX(unsortedList[j + 1], tempPosition.x, sortingSpeed);
                    LeanTween.moveLocalZ(unsortedList[j + 1], 3, sortingSpeed / 2f).setLoopPingPong(1);

                    /*
                     * 
                    temp = unsortedList[i];
                    unsortedList[i] = unsortedList[min];
                    unsortedList[min] = temp;

                    tempPosition = unsortedList[i].transform.localPosition;

                    LeanTween.moveLocalX(unsortedList[i], unsortedList[min].transform.localPosition.x, sortingSpeed);
                    LeanTween.moveLocalZ(unsortedList[i], -3, sortingSpeed / 2f).setLoopPingPong(1);


                    LeanTween.moveLocalX(unsortedList[min], tempPosition.x, sortingSpeed);
                    LeanTween.moveLocalZ(unsortedList[min], 3, sortingSpeed / 2f).setLoopPingPong(1);
                     */

                    swapped = true;
                }
            }

            // IF no two elements were  
            // swapped by inner loop, then break 
            if (swapped == false)
                break;
        }

    }
           
}