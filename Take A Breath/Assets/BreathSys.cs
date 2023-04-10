using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathSys : MonoBehaviour
{
    public List<BreathUI> breathUIs;
    public int activeBreathIndex = 0;
    public Image playerBreathImg;

    public float breathTime = 0;

    public BreathDataContainer breathDataSCO;

    private void Awake()
    {
        SettingDefault();
        InitializeBreathPt();
        UpdateBreathPtUI();
    }

    private void Update()
    {
        playerBreathImg.fillAmount = breathTime;
    }

    public void SettingDefault()
    {
        activeBreathIndex = 0;
        ActiveBreathByIndex(0);

    }

    public void InitializeBreathPt()
    {
        breathDataSCO.firePt = 0;
        breathDataSCO.waterPt = 0;
        breathDataSCO.bugPt = 0;
    }

    public void ChangeBreath()
    {
        activeBreathIndex += 1;
        if(activeBreathIndex > breathUIs.Count - 1)
        {
            activeBreathIndex = 0;
        }

        ActiveBreathByIndex(activeBreathIndex);
    }


    public void ActiveBreathByIndex(int index)
    {
        foreach(BreathUI ui in breathUIs)
        {
            if(ui != breathUIs[index])
            {
                ui.breathHu.gameObject.SetActive(false);
            }
        }

        breathUIs[index].breathHu.gameObject.SetActive(true);
    }

    public void AddBreathPt()
    {
        if(activeBreathIndex == 0)
        {
            breathDataSCO.firePt += 1;
        }
        else if(activeBreathIndex == 1)
        {
            breathDataSCO.waterPt += 1;
        }
        else if(activeBreathIndex == 2)
        {
            breathDataSCO.bugPt += 1;
        }

        UpdateBreathPtUI();

    }

    public void UpdateBreathPtUI()
    {
        breathUIs[0].breathIntUI.text = breathDataSCO.firePt.ToString();
        breathUIs[1].breathIntUI.text = breathDataSCO.waterPt.ToString();
        breathUIs[2].breathIntUI.text = breathDataSCO.bugPt.ToString();
    }
}
