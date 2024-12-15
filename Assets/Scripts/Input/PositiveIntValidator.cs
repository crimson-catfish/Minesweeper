using System;
using TMPro;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "InputValidator - TwoDigits.asset", menuName = "TextMeshPro/Input Validators/TwoDigits",
    order = 100)]
public class TMP_DigitValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        if (ch is < '0' or > '9' || text.Length == 2)
            return '0';

        text += ch;
        pos += 1;

        return ch;
    }
}