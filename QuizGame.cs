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
        // Verileri çekmek için GetData fonksiyonunu baþlat
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        // API'den veri çekmek için UnityWebRequest kullanýlýr
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        // Ýstek hata verirse, hata mesajýný kaydet ve hata mesajýný göster
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // API'den gelen JSON verisini çözümle
            APIResponse responseData = JsonUtility.FromJson<APIResponse>(request.downloadHandler.text);
            QuestionData questionData = responseData.results[0];

            categoryText.text = "Category: " + questionData.category;
            difficulty.text = "Difficulty: " + questionData.difficulty; 
            quizText.text = "Quiz: " + questionData.question;

            // Þýklarý düðmelere yerleþtir
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
        // Týklanan düðmeyi al
        Button clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        // Týklanan düðmenin etiketini kontrol et
        if (clickedButton.CompareTag("correct"))
        {
            // Doðru cevap verildiðinde, yeni bir soru almak için GetData fonksiyonunu çaðýr
            Debug.Log("DOÐRU CEVAP");
            StartCoroutine(GetData());
        }
        else
        {
            // Yanlýþ cevap verildiðinde hata mesajýný göster
            Debug.Log("YANLIÞ CEVAP");
        }
    }
}
