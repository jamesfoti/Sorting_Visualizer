﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingVisualizer : MonoBehaviour {
    /* SOURCES:
     * This project is heavily inspired and based off of Dr. Shahin Rostami's youtube tutorial (link below)
     * Dr. Rostami's Youtube tutorial: https://www.youtube.com/playlist?list=PLwWiU_ClpuYpp2-voD9On2eKvcOa27A7E
     * Dr. Rostami's website: http://shahinrostami.com
     */

    /* WISHLIST:
     * 1) The UI for the different kind of sorting buttons are hard coded. I might just simply turn them into a dropdown menu.
     * 2) More sorting algorithms to be implemented
     */

    public GameObject[] cubes;
    public GameObject cubesParentGameObject;
    public Slider sizeSlider;
    public Slider speedSlider;
    public Button[] sortOptionButtons;
    public GameObject pausePanel;
    public Camera mainCamera;

    private int numberOfCubes = 4;
    private int cubeHeightMin = 2;
    private int cubeHeightMax = 10;
    private float sortingSpeed = .01f;

    private void Start() {
        Debug.Log("Start!");
        GenerateRandomArray();
    }

    private void Update() {
        sortingSpeed = Mathf.Abs(speedSlider.value - 1f); // Sorting speed is based on UI slider
    }

    public void GenerateRandomArray() {
        Debug.Log("Generate Random Array");
        ResetArray(); // Reset array incase one already exists before generating a new one!

        numberOfCubes = (int)sizeSlider.value; // Number of cubes is based on UI slider
        cubes = new GameObject[numberOfCubes]; 
        
        for (int i = 0; i < numberOfCubes; i++) {
            int randomNumber = Random.Range(cubeHeightMin, cubeHeightMax + 1);

            // Experiment with the x values of cubes[i] x-scale and x-position!
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(.5f, randomNumber, .5f);
            cube.transform.position = new Vector3(i*.6f, randomNumber / 2f, 0);
            cube.transform.parent = cubesParentGameObject.transform; // Cubes = parent
            
            cubes[i] = cube;
        }
        // Experiment with the x values of Cubes.position based off the x-scale and x-position of each cube[i]!
        cubesParentGameObject.transform.position = new Vector3(-numberOfCubes / 3.5f, -cubeHeightMax / 2f, 0);
    }

    public void ResetArray() {
        Debug.Log("Reset Arrray");

        DestroyCubesArray(); // Destroys previous array of cubes
        ActivateSortButtons(); // Reactivate buttons

        cubesParentGameObject.transform.position = new Vector3(0, 0, 0); // Reset position of Cubes Parent gameObject
    }

    private void DestroyCubesArray() {
        for (int i = 0; i < cubes.Length; i++) {
            if (cubes[i] != null) {
                Destroy(cubes[i]);
            }
        }
    }

    public void SelectionSort() {      
        Debug.Log("Selection Sort Chosen!");       
        DeactivateSortButtons();
        ChangeDisabledButtonColorToGreen(0);
        StartCoroutine(SelectionSort(cubes));
    }

    public void BubbleSort() {      
        Debug.Log("Bubble Sort Chosen!");    
        DeactivateSortButtons();
        ChangeDisabledButtonColorToGreen(1);
        StartCoroutine(BubbleSort(cubes));
    }

    public void SwitchTo2D(bool to2D) {
        Camera.main.orthographic = to2D;
    }

    public void SwitchTo3D(bool to3D) {
        Camera.main.orthographic = to3D;
    }

    public void Pause() {
        Debug.Log("Pause");
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Resume() {
        Debug.Log("Resume");
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void ResetProgram() {
        Debug.Log("Reset Program");
        GenerateRandomArray();
    }

    public void ExitProgram() {
        Debug.Log("Eixt Program");
        Application.Quit();
    }

    private void DeactivateSortButtons() {
        for (int i = 0; i < sortOptionButtons.Length; i++) {
            sortOptionButtons[i].interactable = false;
        }
    }

    public void ActivateSortButtons() {
        for (int i = 0; i < sortOptionButtons.Length; i++) {
            sortOptionButtons[i].interactable = true;
            ChangeDisabledButtonColorToGrey(i); // Changes disable color back to grey for all buttons
        }
    }

    private void ChangeDisabledButtonColorToGreen(int buttonIndex) {
        var newColorBlock = sortOptionButtons[buttonIndex].colors;
        newColorBlock.disabledColor = Color.green;
        sortOptionButtons[buttonIndex].colors = newColorBlock;
    }

    private void ChangeDisabledButtonColorToGrey(int buttonIndex) {
        var newColorBlock = sortOptionButtons[buttonIndex].colors;
        newColorBlock.disabledColor = Color.grey;
        sortOptionButtons[buttonIndex].colors = newColorBlock;
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
        ActivateSortButtons();
    }
    
    private IEnumerator BubbleSort(GameObject[] unsortedList) {
        GameObject temp;
        Vector3 tempPosition;
  
        for (int i = 0; i < unsortedList.Length - 1; i++) {
            yield return new WaitForSeconds(sortingSpeed);

            for (int j = 0; j < unsortedList.Length - i - 1; j++) {
                LeanTween.color(unsortedList[j], Color.red, sortingSpeed);
                LeanTween.color(unsortedList[j + 1], Color.red, sortingSpeed);
                yield return new WaitForSeconds(sortingSpeed);

                if (unsortedList[j].transform.localScale.y > unsortedList[j + 1].transform.localScale.y) {
                    // swap unsortedList[j] and unsortedList[j+1] 
                    temp = unsortedList[j];
                    unsortedList[j] = unsortedList[j + 1];
                    unsortedList[j + 1] = temp;

                    tempPosition = unsortedList[j].transform.localPosition;

                    LeanTween.moveLocalX(unsortedList[j], unsortedList[j + 1].transform.localPosition.x, sortingSpeed);
                    LeanTween.moveLocalZ(unsortedList[j], -3f, sortingSpeed / 2f).setLoopPingPong(1);

                    LeanTween.moveLocalX(unsortedList[j + 1], tempPosition.x, sortingSpeed);
                    LeanTween.moveLocalZ(unsortedList[j + 1], 3f, sortingSpeed / 2f).setLoopPingPong(1);

                    yield return new WaitForSeconds(sortingSpeed);
                }

                LeanTween.color(unsortedList[j], Color.white, sortingSpeed);
                LeanTween.color(unsortedList[j+1], Color.white, sortingSpeed);
            }
            LeanTween.color(unsortedList[unsortedList.Length - 1 - i], Color.green, sortingSpeed);
        }
        yield return new WaitForSeconds(sortingSpeed);
        LeanTween.color(unsortedList[0], Color.green, sortingSpeed);

        ActivateSortButtons();
    }


}