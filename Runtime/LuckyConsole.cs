using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Lucky44.LuckyConsole;
using UnityEngine.EventSystems;

namespace Lucky44.LuckyConsole
{
    public class LuckyConsole : MonoBehaviour
    {
        //UI components
        [Header("UI")]
        [SerializeField]
        private TextMeshProUGUI consoleOutput;
        [SerializeField]
        private TMP_InputField consoleInput;

        [SerializeField]
        private TextMeshProUGUI autoCompleteText;
        [SerializeField]
        private GameObject autoCompletePanel;

        private CommandManager commandManager;
        private static CommandManager cM;
        private static LuckyConsole inst;

        //Tweening
        [Header("Settings")]
        [SerializeField]
        private float timeToMove = .5f;
        private new RectTransform transform;
        private Vector2 firstPos;
        private Vector2 finalPos;
        private bool move = false;
        private int moveDirection = 1;
        private float tmpTime = 0;
        [SerializeField]
        private bool enablePremadeCommands = true;
        [SerializeField]
        private KeyCode consoleToggle = KeyCode.Tab;

        //History
        private List<string> last = new List<string>();
        private int cInd = -1;

        private void Start()
        {
            inst = this;

            tmpTime = timeToMove;

            transform = GetComponent<RectTransform>();

            firstPos = transform.anchoredPosition;
            finalPos = new Vector2(-GetComponentInParent<Canvas>().pixelRect.size.x / 2, firstPos.y);

            transform.anchoredPosition = finalPos;

            commandManager = new CommandManager();
            commandManager.subscribeMethods();

            consoleInput.onSubmit.AddListener(onFinish);
            consoleInput.onValueChanged.AddListener(onTextChange);

            cM = commandManager;

            if(enablePremadeCommands)
                cM.forceRegisterMethod(typeof(LuckyConsole));
        }

        public void Update()
        {
            if (move)
            {
                tmpTime += Time.deltaTime * moveDirection;
                if(moveDirection < 0 && tmpTime <= 0)
                {
                    tmpTime = 0;
                    move = false;
                }
                else if (moveDirection > 0 && tmpTime >= timeToMove)
                {
                    tmpTime = timeToMove;
                    move = false;
                    EventSystem.current.SetSelectedGameObject(consoleInput.gameObject);
                }

                transform.anchoredPosition = Vector2.Lerp(firstPos, finalPos, tmpTime / timeToMove);
            }
            else
            {
                if (Input.GetKeyDown(consoleToggle))
                {
                    move = true;
                    moveDirection = -moveDirection;
                }
                else if (consoleInput.isFocused)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        cInd++;

                        if(cInd >= last.Count)
                            cInd = last.Count - 1;

                        consoleInput.text = last[cInd];
                    }
                }
            }
        }

        public void onFinish(string input)
        {
            last.Reverse();
            last.Add(input);
            last.Reverse();
            cInd = -1;

            consoleInput.text = "";
            consoleOutput.text += $"> {input}\n";
            consoleOutput.text += commandManager.executeCommand(input) + "\n";
            autoCompletePanel.gameObject.SetActive(false);
        }

        public void onTextChange(string input)
        {
            autoCompletePanel.gameObject.SetActive(true);
            List<string> autoComplete = commandManager.getAutocompletion(input.Split(' ')[0]);
            string output = "";
            foreach (string s in autoComplete)
                output += $"{s}\n";
            autoCompleteText.text = output;
        }

        [Command("showBuffer","Show the current buffer of the console")]
        public static string showBuffer()
        {
            string output = string.Format("<b>BUFFER</b>\n");
            for(int i = 0; i < inst.last.Count; i++)
            {
                output += $"{i}: {inst.last[i]}\n";
            }
            return output;
        }

        [Command("help", "displays all registered commands")]
        public static string help()
        {
            string output = string.Format("<b>HELP</b>\n");
            foreach (Command c in cM.getCommands())
            {
                output += $" - {c}\n";
            }
            return output;
        }
    }
}
