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

    // set page-limits from Inspector
    [SerializeField]
    private int firstPage = 1;
    [SerializeField]
    private int lastPage = 5;

    // currently displayed
    private string inputString = "";
    private string pageString;
    private string answersString;

    // other Assets
    private TextAsset page;
    private Dictionary<string, int> answers = new Dictionary<string, int>();

    private bool backspacePressed = false;

    // used in ProcessInput()
    List<KeyCode> letterKeys = new List<KeyCode>() {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
        KeyCode.Space, KeyCode.Question, KeyCode.Exclaim, KeyCode.Period, KeyCode.Comma
    };

    KeyCode[] numberKeys = {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
    };

    private void UpdateCanvas(string textobj) {
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

    private void UpdateEntireCanvas() {
        UpdateCanvas("input");
        UpdateCanvas("page");
        UpdateCanvas("answers");
    }

    private void AddLetter(char letter) {
        inputString += letter;
        UpdateCanvas("input");
    }
    private void RemoveLetter() {
        if (inputString.Length >= 1) {
            inputString = inputString.Remove(inputString.Length - 1);
            UpdateCanvas("input");
        }
    }

    private void ClearInput() {
        inputString = "";
    }

    // returns next page for correct input, otherwise -1
    private int EvaluateInput() {
        foreach (KeyValuePair<string, int> answer in answers) {
            if (inputString == answer.Key) {
                return answer.Value;
            }
        }
        return -1;
    }

    private void NextPage() {
        int nextPage = EvaluateInput();
        ClearInput();

        // valid next page
        if ((nextPage >= 0) && (nextPage < lastPage)) {
            ReadFile(nextPage);
            UpdateEntireCanvas();
        }
    }

    private void ReadFile(int nr) {
        page = Resources.Load("Textadventure/page" + nr) as TextAsset;

        // split contents of file
        string[] content = page.text.Split('#');

        pageString = content[0];
        content = content.Skip(1).ToArray();

        // clear old answers
        answers.Clear();
        answersString = "";

        foreach (string answer in content) {
            string[] parts = answer.Split('>');

            // save new answer in Dictionary
            int nextPage;
            if (!int.TryParse(parts[1].Trim(), out nextPage)) {
                Debug.Log("unparseable nextPage");
            }
            answers.Add(parts[0].Trim().ToUpper(), nextPage);

            answersString += parts[0] + "\n";
        }

    }

    // calls method corresponding to KeyCode
    public void ProcessInput(KeyCode keyCode) {
        if (keyCode == KeyCode.Return) {
            NextPage();

        } else if (keyCode == KeyCode.Backspace) {
            if (backspacePressed) {
                ClearInput();
                backspacePressed = false;
            } else {
                RemoveLetter();
                backspacePressed = true;
            }

        } else if (letterKeys.Contains(keyCode)) {
            switch (keyCode) {
                case KeyCode.Space:
                    AddLetter(' ');
                    break;
                case KeyCode.Question:
                    AddLetter('?');
                    break;
                case KeyCode.Exclaim:
                    AddLetter('!');
                    break;
                case KeyCode.Period:
                    AddLetter('.');
                    break;
                case KeyCode.Comma:
                    AddLetter(',');
                    break;
                default:
                    char letter = char.Parse(keyCode.ToString());
                    AddLetter(letter);
                    break;
            }
        } else if (numberKeys.Contains(keyCode)) {
            for (int i = 0; i < 10; i++) {
                if (keyCode == numberKeys[i]) {
                    AddLetter(char.Parse("" + i));
                }
            }
        }
    }

    public void SetupScene() {
        TurnOnCamera();
        ReadFile(firstPage);
        UpdateEntireCanvas();
    }

    public void CloseScene() {
        TurnOffCamera();
    }
    
    override public string ToString() {
        return "TextadventureMode";
    }
}
