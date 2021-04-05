using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : BaseObject
{
    public static ColorManager instance;
    public Color[] colors;
    string[] colorNumberArray = new string[] 
    {
        "B10", "C2", "C3", "C13", "D16", "D17", "B6", "C4", "C10", "C17", "D1", "D11", "C15", "C11", "C5", "C6", "C7", "D2", "B19", "B7", "C8", "C9", "D3", "C16",
        "E12", "E2", "E8", "D19", "D8", "D9", "E6", "E4", "E3", "E9", "D12", "D6", "E5", "E10", "D5", "D13", "D20", "D18", "E7", "E13", "D21", "D14", "D7", "D15",
        "C14", "B20", "C1", "B18", "M5", "M6", "B3", "B16", "B13", "B1", "G13", "F10", "B5", "B4", "B2", "B14", "G7", "F11", "B15", "B12", "B8", "B17", "B11", "G8",
        "A15", "A3", "A11", "A9", "F14", "F12", "A4", "A13", "A6", "F1", "F2", "F3", "A5", "A10", "A7", "F13", "F9", "F6", "A8", "A14", "F4", "F5", "F8", "F7",
        "E15", "E1", "E14", "E11", "H2", "H1", "A12", "G3", "G2", "G1", "A1", "H12", "G6", "G5", "G9" ,"M9" , "H3", "H4", "G14", "M12", "G17", "H5", "H6", "H7",
        "H8", "G15", "A2", "H13", "G16", "H9", "H10", "M1", "G11", "G4", "M4", "H14", "M10", "M2", "G12", "M13", "M7", "H11", "M11", "M3", "G10", "M14", "M8", "M15",
        "P18", "P16", "P3", "P12", "P1", "T1", "P7", "P17", "P6", "P13", "P9", "P11", "P4", "P5", "P15", "P14", "P2", "R12", "P23", "P22", "P21", "E17", "P19", "P8",
        "P10", "R11", "Y2", "Y3", "Q2", "Y4", "Y5", "Y1", "R3", "R4", "R5", "R8", "R9", "R2", "R1", "R10", "R6", "R7", "D10", "H6", "Q5", "B9", "C12", "D4",
    };
    public struct PindouDesc
    {
        public Color color;
        public string colorNumber;
    }

    protected override void OnReadyAwake()
    {
        if(instance == null)
            instance = this;
        DontDestroyOnLoad(this);
    }

    public PindouDesc GetNearColor(Color color)
    {
        int nearIndex = 0;
        var nearColorHSV = Tool.RGBTOHSV(colors[nearIndex]);
        var TargetHsv = Tool.RGBTOHSV(color);
        for (int i = 1; i < colors.Length; i++)
        {
            var colorsHSV = Tool.RGBTOHSV(colors[i]);
            if (Quaternion.Angle(Quaternion.Euler(0,0, TargetHsv.H),Quaternion.Euler(0,0, colorsHSV.H)) + Mathf.Abs(TargetHsv.S - colorsHSV.S) + Mathf.Abs(TargetHsv.V - colorsHSV.V) <
                Quaternion.Angle(Quaternion.Euler(0,0, TargetHsv.H),Quaternion.Euler(0,0, nearColorHSV.H)) + Mathf.Abs(TargetHsv.S - nearColorHSV.S) + Mathf.Abs(TargetHsv.V - nearColorHSV.V))
            {
                nearIndex = i;
                nearColorHSV = Tool.RGBTOHSV(colors[nearIndex]);
            }
        }
        PindouDesc pindouDesc;
        pindouDesc.color = colors[nearIndex];
        pindouDesc.colorNumber = colorNumberArray[nearIndex];
        return pindouDesc;
    }
}
