using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class QuizGame : MonoBehaviour
{
    private string url = "https://opentdb.com/api.php?amount=10&category=23&difficulty=medium&type=multiple";
    public TextMeshProUGUI categoryText;
    public TextMeshProUGUI difficulty;
    public TextMeshProUGUI quizText;
    public List<Button> buttonList = new List<Button>();

    [System.Serializable]
    public class QuestionData
    {
        public string category;
        public string type;
        public string difficulty;
        public string question;
        public string correct_answer;
        public List<string> incorrect_answers;
    }

    [System.Serializable]
    public class APIResponse
    {
        public List<QuestionData> results;
    }

    void Start()
    {
        // Verileri �ekmek i�in GetData fonksiyonunu ba�lat
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        // API'den veri �ekmek i�in UnityWebRequest kullan�l�r
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        // �stek hata verirse, hata mesaj�n� kaydet ve hata mesaj�n� g�ster
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // API'den gelen JSON verisini ��z�mle
            APIResponse responseData = JsonUtility.FromJson<APIResponse>(request.downloadHandler.text);
            QuestionData questionData = responseData.results[0];

            categoryText.text = "Category: " + questionData.category;
            difficulty.text = "Difficulty: " + questionData.difficulty; 
            quizText.text = "Quiz: " + questionData.question;

            // ��klar� d��melere yerle�tir
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (i < 3)
                {
                    buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = questionData.incorrect_answers[i];
                    buttonList[i].tag = "incorrect";
                }
                else
                {
                    buttonList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = questionData.correct_answer;
                    buttonList[i].tag = "correct";
                }
            }
        }
    }

    public void CorrectControl()
    {
        // T�klanan d��meyi al
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        // T�klanan d��menin etiketini kontrol et
        if (clickedButton.CompareTag("correct"))
        {
            // Do�ru cevap verildi�inde, yeni bir soru almak i�in GetData fonksiyonunu �a��r
            Debug.Log("DO�RU CEVAP");
            StartCoroutine(GetData());
        }
        else
        {
            // Yanl�� cevap verildi�inde hata mesaj�n� g�ster
            Debug.Log("YANLI� CEVAP");
        }
    }
}
