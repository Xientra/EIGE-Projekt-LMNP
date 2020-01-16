using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextadventureMode : CameraManagement, GameMode {

    // GameObjects for displaying text
    [SerializeField]
    private Text inputText;
    [SerializeField]
    private Text pageText;
    [SerializeField]
    private Text answersText;

    // set page-limits from inspector
    [SerializeField]
    private int firstPage = 1;
    [SerializeField]
    private int lastPage = 5;

    // other variables
    private string inputString = "";

    private string pageString;
    private TextAsset page;

    private string answersString;
    Dictionary<string, int> answers = new Dictionary<string, int>();

    // used in processInput()
    List<KeyCode> validKeys = new List<KeyCode>() {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
        KeyCode.Space, KeyCode.Period, KeyCode.Comma, KeyCode.Minus, KeyCode.Exclaim, KeyCode.Question
    };

    private void updateCanvas(string textobj) {
        switch (textobj) {
            case "input":
                inputText.text = inputString;
                break;
            case "page":
                pageText.text = pageString;
                break;
            case "answers":
                answersText.text = answersString;
                break;
        }
    }

    public void addLetter(char letter) {
        inputString += letter;
        updateCanvas("input");
    }
    public void removeLetter() {
        if (inputString.Length >= 1) {
            inputString = inputString.Remove(inputString.Length - 1);
            updateCanvas("input");
        }
    }

    // returns next page for correct input, otherwise -1
    private int evaluateInput() {
        foreach (KeyValuePair<string, int> answer in answers) {
            if (inputString == answer.Key) {
                return answer.Value;
            }
        }
        return -1;
    }

    public void nextPage() {
        int nextPage = evaluateInput();
        inputString = "";

        // valid next page
        if ((nextPage >= 0) && (nextPage < lastPage)) {
            Debug.Log("here");
            readFile(nextPage);
        }
    }

    private void readFile(int nr) {
        page = Resources.Load("Textadventure/page" + nr) as TextAsset;

        // split contents of file
        string[] content = page.text.Split('#');

        pageString = content[0];
        content = content.Skip(1).ToArray();

        // reset answers
        answers.Clear();
        answersString = "";

        foreach (string answer in content) {
            string[] parts = answer.Split('>');

            int nextPage;
            if (!int.TryParse(parts[1].Trim(), out nextPage)) {
                Debug.Log("unparseable nextPage");
            }
            answers.Add(parts[0].ToUpper().Trim(), nextPage);

            answersString += parts[0] + "\n";
        }

        // update entire canvas
        updateCanvas("input");
        updateCanvas("page");
        updateCanvas("answers");
    }

    public void ProcessInput(KeyCode keyCode) {
        if (keyCode == KeyCode.Return) {
            nextPage();
        }
        else if (keyCode == KeyCode.Backspace) {
            removeLetter();
        }
        else if (validKeys.Contains(keyCode)) {

            if (keyCode == KeyCode.Space) {
                addLetter(' ');
            }
            if (keyCode == KeyCode.Question) {
                addLetter('?');
            } else {
                char letter = char.Parse(keyCode.ToString());
                addLetter(letter);
            }

            // KeyCode.Period, KeyCode.Comma, KeyCode.Minus, KeyCode.Exclaim
        }
    }

    public void SetupScene() {
        TurnOnCamera();
        readFile(firstPage);
    }

    public void CloseScene() {
        TurnOffCamera();
    }

    override public string ToString() {
        return "TextadventureMode";
    }
}
