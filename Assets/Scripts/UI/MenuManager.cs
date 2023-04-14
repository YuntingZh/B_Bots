using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject[] pages;  // Array of pages to switch between
    public float transitionTime = 1f;  // Time it takes to fade in/out a page

    private int currentPageIndex = 0;  // Index of the current page

    // Use this for initialization
    void Start () {
        // Disable all pages except the first one
        for (int i = 1; i < pages.Length; i++) {
            pages[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {

    }

    // Call this method to switch to the next page
    public void NextPage() {
        StartCoroutine(FadePage(currentPageIndex, (currentPageIndex + 1) % pages.Length));
        currentPageIndex = (currentPageIndex + 1) % pages.Length;
    }

    // Call this method to switch to the previous page
    public void PreviousPage() {
        int previousPageIndex = currentPageIndex - 1;
        if (previousPageIndex < 0) {
            previousPageIndex = pages.Length - 1;
        }
        StartCoroutine(FadePage(currentPageIndex, previousPageIndex));
        currentPageIndex = previousPageIndex;
    }
    // Call this method to switch to a specific page by index
    public void SwitchToPage(int pageIndex) {
        if (pageIndex >= 0 && pageIndex < pages.Length && pageIndex != currentPageIndex) {
            StartCoroutine(FadePage(currentPageIndex, pageIndex));
            currentPageIndex = pageIndex;
        }
    }


    // Coroutine to fade in/out a page
    IEnumerator FadePage(int pageIndexToFadeOut, int pageIndexToFadeIn) {
        // Fade out the current page
        CanvasGroup currentCanvasGroup = pages[pageIndexToFadeOut].GetComponent<CanvasGroup>();
        while (currentCanvasGroup.alpha > 0) {
            currentCanvasGroup.alpha -= Time.deltaTime / transitionTime;
            yield return null;
        }
        currentCanvasGroup.interactable = false;
        currentCanvasGroup.blocksRaycasts = false;
        pages[pageIndexToFadeOut].SetActive(false);

        // Fade in the new page
        CanvasGroup nextCanvasGroup = pages[pageIndexToFadeIn].GetComponent<CanvasGroup>();
        pages[pageIndexToFadeIn].SetActive(true);
        while (nextCanvasGroup.alpha < 1) {
            nextCanvasGroup.alpha += Time.deltaTime / transitionTime;
            yield return null;
        }
        nextCanvasGroup.interactable = true;
        nextCanvasGroup.blocksRaycasts = true;
    }
}
