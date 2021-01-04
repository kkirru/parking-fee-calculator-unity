using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaclControl : MonoBehaviour
{

    public Text EntTimeText_hour;
    public Text EntTimeText_minute;
    public Text EntTimeText_noon;
    public Text DepTimeText_hour;
    public Text DepTimeText_minute;
    public Text DepTimeText_noon;
    public Text priceText;

    public Text GuideText;

    public Button[] buttons;
    public Button[] noon_buttons;


    Text[] buttons_text = new Text[12];

    State state;

    int entHour;
    int entMin;
    int depHour;
    int depMin;

    bool ent_isAfternoon;
    bool dep_isAfternoon;


    void Start()
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons_text[i] = buttons[i].transform.GetChild(0).GetComponent<Text>();
        }
        init();
    }


    void init()
    {
        state = State.entHour;
        SetButtons(state);

        entHour = 0;
        entMin = 0;
        depHour = 0;
        depMin = 0;
        ent_isAfternoon = false;
        dep_isAfternoon = false;

        priceText.text = "0";
        SetTime();
    }

    public void AfternoonBtn(bool flag)
    {
        if (state == State.entHour)
        {
            ent_isAfternoon = flag;

        }
        else if (state == State.depHour)
        {
            dep_isAfternoon = flag;
        }

        SetTime();
    }

    void SetButtons(State state)
    {
        switch (state)
        {
            case State.entHour:
                GuideText.text = "입차 (시)";
                noon_buttons[0].gameObject.SetActive(true);
                noon_buttons[1].gameObject.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons_text[i].text = (i + 1).ToString() + "시";
                }
                break;
            case State.entMin:
                GuideText.text = "입차 (분)";
                noon_buttons[0].gameObject.SetActive(false);
                noon_buttons[1].gameObject.SetActive(false);

                for (int i = 0; i < buttons.Length; i++)
                {

                    if (i < 6)
                    {
                        buttons[i].gameObject.SetActive(false);
                        continue;
                    }

                    buttons[i].gameObject.SetActive(true);
                    buttons_text[i].text = ((i - 6) * 10).ToString() + "분";
                }
                break;
            case State.depHour:
                GuideText.text = "출차 (시)";
                noon_buttons[0].gameObject.SetActive(true);
                noon_buttons[1].gameObject.SetActive(true);
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons_text[i].text = (i + 1).ToString() + "시";
                }
                break;
            case State.depMin:
                GuideText.text = "출차 (분)";
                noon_buttons[0].gameObject.SetActive(false);
                noon_buttons[1].gameObject.SetActive(false);

                for (int i = 0; i < buttons.Length; i++)
                {

                    if (i < 6)
                    {
                        buttons[i].gameObject.SetActive(false);
                        continue;
                    }

                    buttons[i].gameObject.SetActive(true);
                    buttons_text[i].text = ((i - 6) * 10).ToString() + "분";
                }
                break;
            case State.Calc:

                for (int i = 3; i < buttons.Length; i++)
                {
                    buttons[i].gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    void SetTime()
    {
        EntTimeText_hour.text = entHour.ToString();
        EntTimeText_minute.text = entMin.ToString();
        DepTimeText_hour.text = depHour.ToString();
        DepTimeText_minute.text = depMin.ToString();

        if (dep_isAfternoon)
        {
            DepTimeText_noon.text = "오후";
        }
        else
        {
            DepTimeText_noon.text = "오전";
        }

        if (ent_isAfternoon)
        {
            EntTimeText_noon.text = "오후";
        }
        else
        {
            EntTimeText_noon.text = "오전";
        }
    }

    public void CalcPrice()
    {
        int hour = 0;
        int min = 0;
        if (ent_isAfternoon) // 입차 오후
        {
            if (!dep_isAfternoon) // 출차가 오전이면
            {
                priceText.text = "잘못 입력하셨습니다.";
                return;
            }
            else if (depHour < entHour) // 출차도 오후인데 입차 시간이 출차 보다 숫자 더 크면
            {
                priceText.text = "잘못 입력하셨습니다.";
                return;
            }

            entHour += 12;
        }
        else // 입차 오전
        {
            if (!dep_isAfternoon && depHour < entHour)
            {
                priceText.text = "잘못 입력하셨습니다.";
                return;
            }
        }
        
        if (dep_isAfternoon) // 출차 오후
        {
            depHour += 12;
        }

        hour = depHour - entHour;
        if (depMin >= entMin)
        {
            min = depMin - entMin;
        } else{
            hour --;
            min = depMin  + 60 - entMin;
        }


        int price = hour * 6000 + min/10 * 1000;
        priceText.text = price.ToString();
    }

    public void OnClickInitBtn()
    {
        init();
    }

    public void OnClickBtn(int index)
    {
        switch (state)
        {
            case State.entHour:
                entHour = index + 1;
                break;
            case State.entMin:
                entMin = (index - 6) * 10;
                break;
            case State.depHour:
                depHour = index + 1;
                break;
            case State.depMin:
                depMin = (index - 6) * 10;
                CalcPrice();
                break;
            case State.Calc:
                init();
                return;
            default:
                break;
        }

        state = state + 1;
        SetButtons(state);
        SetTime();
    }


}

enum State
{
    entHour, entMin, depHour, depMin, Calc
}
