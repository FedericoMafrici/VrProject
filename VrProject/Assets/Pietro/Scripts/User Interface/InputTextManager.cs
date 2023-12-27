using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

enum InputTextType {
    PET,
    REMOVE,
    INIT
}

public class InputTextManager : MonoBehaviour {
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_FontAsset _fontAsset;
    private Dictionary<InputTextType, Stack<TextMeshProUGUI>> _activeTexts = new Dictionary<InputTextType, Stack<TextMeshProUGUI>>();
    private float _horizontalSpacing = 10f;
    private float _offsetFromCenter = 20f;
    private void Start() {
        if (_canvas == null) {
            Debug.LogError(transform.name + " no canvas set");
        }

        //needed to initialize TextMeshPro and avoid lagging during gameplay
        AddText("Init", InputTextType.INIT);
        RemoveText(InputTextType.INIT);

        Petter.InPetRange += WhenInPetRange;
        Petter.OutOfPetRange += WhenOutOfPetRange;
        AnimalPartRemover.InRange += WhenInRemovalRange;
        AnimalPartRemover.OutOfRange += WhenOutOfRemovalRange;
    }

    private void WhenInPetRange(object sender, EventArgs args) {
        AddText(sender.ConvertTo<Petter>().GetPetText(), InputTextType.PET);
    }

    private void WhenOutOfPetRange(object sender, EventArgs args) {
        RemoveText(InputTextType.PET);
    }

    private void WhenInRemovalRange(object sender, EventArgs args) {
        AddText(sender.ConvertTo<AnimalPartRemover>().GetActionText(), InputTextType.REMOVE);
    }

    private void WhenOutOfRemovalRange(object sender, EventArgs args) {
        RemoveText(InputTextType.REMOVE);
    }

    private void AddText(string text, InputTextType type) {
        GameObject textBoxObject = new GameObject();
        textBoxObject.transform.SetParent(_canvas.transform, false);
        TextMeshProUGUI textBox = textBoxObject.AddComponent<TextMeshProUGUI>();
        textBox.text = text;
        textBox.color = Color.black;
        if (_fontAsset != null) {
            textBox.font = _fontAsset;
        }
        textBox.alignment = TextAlignmentOptions.Center;

        if (_activeTexts.ContainsKey(type)) {
            Stack<TextMeshProUGUI> texts= _activeTexts[type];
            texts.Push(textBox);
        } else {
            _activeTexts.Add(type, new Stack<TextMeshProUGUI>());
            _activeTexts[type].Push(textBox);
        }

        RepositionTextBoxes();
    }

    private void RemoveText(InputTextType type) {
        if (_activeTexts.ContainsKey(type)) {
            TextMeshProUGUI removed = _activeTexts[type].Pop();
            Destroy(removed);

            if (_activeTexts[type].Count == 0 ) { 
                _activeTexts.Remove(type);
            }

            RepositionTextBoxes();
        }
    }

    private void RepositionTextBoxes() {
        if (_activeTexts.Count == 1) {
            TextMeshProUGUI textBox = _activeTexts.Values.ElementAt(0).Peek();
            RectTransform rt = textBox.transform.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -(rt.sizeDelta.y+_offsetFromCenter)); //-rt.sizeDelta.x/2.0f

        } else {
            float maxYSize = 0f;
            float totalWidth = 0f;
            totalWidth += _horizontalSpacing * (_activeTexts.Count);
            foreach (Stack<TextMeshProUGUI> texts in _activeTexts.Values) {
                RectTransform rt = texts.Peek().GetComponent<RectTransform>();

                if (maxYSize < rt.sizeDelta.y)
                    maxYSize = rt.sizeDelta.y;

                totalWidth += rt.sizeDelta.x;
            }

            float xPos = -(totalWidth/(2.0f * _activeTexts.Count)); //leftmost position, check if this actually works

            foreach (Stack<TextMeshProUGUI> texts in _activeTexts.Values) {
                RectTransform rt = texts.Peek().GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(xPos, -(maxYSize*2.0f + _offsetFromCenter));
                xPos += rt.sizeDelta.x + _horizontalSpacing;
            }
        }
    }
}


