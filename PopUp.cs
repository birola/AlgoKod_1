using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PopUp : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> Popup_komut_buttons;

    public GameObject komut_buttons;
    //public Button km1;
    public GameObject Tut_img;
    public GameObject TutPopUp;
    public GameObject btn_tntm_img;

    public GameObject ileri, geri;

    public static int counter;
    public GameObject btn_sol, btn_sag, btn_ileri, btn_zipla, enerji_btn, teleport_btn, f1_btn, f2_btn;

    [SerializeField]
    public Sprite Sol_hlSprite, Sag_hlSprite, ileri_hlSprite, zipla_hlSprite, enerji_hlSprite, teleport_hlSprite, f1_hlSprite, f2_hlSprite;
    public Sprite Sol_nrmlSprite, Sag_nrmlSprite, ileri_nrmlSprite, zipla_nrmlSprite, enerji_nrmlSprite, teleport_nrmlSprite, f1_nrmlSprite, f2_nrmlSprite;
    public GameObject Sol_text, Sag_text, ileri_text, zipla_text, enerji_text, teleport_text, f1_text, f2_text;
    public GameObject Sol_text_baslik, Sag_text_baslik, ileri_text_baslik, zipla_text_baslik, enerji_text_baslik, teleport_text_baslik, f1_text_baslik, f2_text_baslik;
    public GameObject Sol_text_image, Sag_text_image, ileri_text_image, zipla_text_image, enerji_text_image, teleport_text_image, f1_text_image, f2_text_image;
    public GameObject Sol_img, Sag_img, ileri_img, zipla_img;
    public GameObject asel_sag_ileri, asel_sag_nrml, asel_sol, asel_teleport;
    public GameObject disable_kup_ileri;
    public GAME_CONTROLLER GC;
    public Button_tanitimi BT;

    public GameObject level0;
    //private int btn_cntr = 0;
    

    private void Start()
    {
        #region gereksiz
        // fades the image out when you click
        //StartCoroutine(FadeImage(true));
        //SpriteState ss = new SpriteState();
        //ss.highlightedSprite = hlSprite;
        //btn.GetComponent<Button>().spriteState = ss;
        #endregion
    }


    //public void Level0_Popup()
    //{
    //     btn_cntr = 1;
        
    //}

    public void PopUp_TurnOn()
    {

        foreach (GameObject obj in Popup_komut_buttons)
        {
            obj.SetActive(true);
        }

    }

    public void PopUp_TurnOff()
    {


        foreach (GameObject obj in Popup_komut_buttons)
        {
            obj.SetActive(false);
        }
        TutPopUp.SetActive(true);//animasyon çağır

    }

    public void PopUp_Next()
    {
        counter++;
        PopUp_if_Cases();
        #region deneme2
        //StartCoroutine(PopupMove_Up());
        //StartCoroutine(PositionOverTime(2f, komut_buttons));
        //StartCoroutine(RepeatLerp(Transform a, Transform b, float time)); //devam et (*y posizyonunda +130 birim yukarı çıkacak)
        //StartCoroutine(MoveToPosition(komut_buttons, new Vector3(410f, 286f,0f),2f));


        //if (GC.level == 0 && counter == 2)
        //{
        //int cntr = 0;
        //cntr++;
        //BT.Btn_tanit();
        //Debug.Log("btn_tanıt a girdi");
        //TutPopUp.SetActive(false);
        //Tut_img.SetActive(false);
        //if (btn_cntr == 1)
        //{
        //    counter = 0;
        //    PopUp_if_Cases();//eğer pop da counter 2 ise button a basılınca pop u kapat
        //    Debug.Log("btn_cntr a girdi");
        //}
        //if (GC.level> 0 && counter > 2)
        //{
        //    btn_tntm_img.SetActive(false);
        //    level0.SetActive(false);
        //}

        //}
        #endregion
    }

    public void PopUp_Previous()
    {
        if (counter > 0)
        {
            counter--;
            //  StartCoroutine(PopupMove_Down());
            PopUp_if_Cases();
        }

    }

    private IEnumerator PopupMove_Up()
    {

        var t = komut_buttons.transform.localPosition;
        var c = 0f;

        while (komut_buttons.transform.localPosition.magnitude > 2.501f)
        {
            komut_buttons.transform.localPosition = Vector3.Lerp(t, 2.5f * Vector3.up, c += 0.05f);
            //Debug.Log("magnitude value"+ t.magnitude);
            yield return new WaitForEndOfFrame();
        }


        komut_buttons.transform.localPosition = 2.5f * Vector3.up;
    }

    private IEnumerator PopupMove_Down()
    {

        var t = komut_buttons.transform.localPosition;
        var c = 0f;

        while (komut_buttons.transform.localPosition.magnitude > 2.501f)
        {
            komut_buttons.transform.localPosition = Vector3.Lerp(t, 2.5f * Vector3.down, c += 0.05f);
            //Debug.Log("magnitude value"+ t.magnitude);
            yield return new WaitForEndOfFrame();
        }


        komut_buttons.transform.localPosition = 2.5f * Vector3.down;
    }


    #region denemeler
    //private IEnumerator Fade_Buttons()
    //{
    //    while (Popup_komut_buttons.alpha > 0)
    //    {                   //use "< 1" when fading in
    //        img.al -= Time.deltaTime / 1;    //fades out over 1 second. change to += to fade in    
    //        yield return null;
    //    }
    //}


    //IEnumerator FadeImage(bool fadeAway)
    //{
    //    // fade from opaque to transparent
    //    if (fadeAway)
    //    {
    //        // loop over 1 second backwards
    //        for (float i = 1; i >= 0; i -= Time.deltaTime)
    //        {
    //            // set color with i as alpha
    //            img.color = new Color(1, 1, 1, i);
    //            yield return null;
    //        }
    //    }
    //    // fade from transparent to opaque
    //    else
    //    {
    //        // loop over 1 second
    //        for (float i = 0; i <= 1; i += Time.deltaTime)
    //        {
    //            // set color with i as alpha
    //            img.color = new Color(1, 1, 1, i);
    //            yield return null;
    //        }
    //    }
    //}


    //private void PopUp_Cases()
    //{
    //    switch (counter)
    //    {
    //        case 0://sol
    //            btn_sol.GetComponent<Image>().sprite = Sol_hlSprite;
    //            Sol_text.SetActive(true);
    //            break;
    //        case 1://sag
    //            btn_sag.GetComponent<Image>().sprite = Sag_hlSprite;
    //            break;
    //        case 2://ileri
    //            btn_ileri.GetComponent<Image>().sprite = ileri_hlSprite;
    //            break;
    //        case 3://zıpla
    //            btn_zipla.GetComponent<Image>().sprite = zipla_hlSprite;
    //            break;
    //        case 4://enerji
    //            enerji_btn.GetComponent<Image>().sprite = enerji_hlSprite;
    //            break;
    //        case 5://teleport
    //            teleport_btn.GetComponent<Image>().sprite = teleport_hlSprite;
    //            break;
    //        case 6://f1
    //            f1_btn.GetComponent<Image>().sprite = f1_hlSprite;
    //            break;
    //        case 7://f2
    //            f2_btn.GetComponent<Image>().sprite = f2_hlSprite;
    //            break;
    //    }
    //}


    //range ini ayarla
    //public static int Clamp(int value, int min, int max)
    //{
    //    return (value < min) ? min : (value > max) ? max : value;
    //}
    #endregion
    public void Popup_Lvl1()
    {
        //if (counter > 2) return;
        //counter = Mathf.Clamp(counter, 0, 2);
        if (counter >= 2)
        {
            Tut_img.SetActive(false);
            TutPopUp.SetActive(false);
           
        }
    }

    public void Popup_Lvl2()
    {
        //counter = Mathf.Clamp(counter, 0, 4);
        if (counter >= 4)
        {
            Tut_img.SetActive(false);
            TutPopUp.SetActive(false);
        }
    }

    public void Popup_Lvl3()
    {
        //counter = Mathf.Clamp(counter, 0, 5);
        if (counter >= 5)
        {
            Tut_img.SetActive(false);
            TutPopUp.SetActive(false);
        }
    }

    public void Popup_Lvl8()
    {
        //counter = Mathf.Clamp(counter, 0, 6);
        if (counter >= 6)
        {
            Tut_img.SetActive(false);
            TutPopUp.SetActive(false);
        }
    }
    public void Popup_Lvl14()
    {
        //counter = Mathf.Clamp(counter, 0, 7 );
        if (counter >= 7 )
        {
            Tut_img.SetActive(false);
            TutPopUp.SetActive(false);
        }
    }

    public void Popup_Lvl20()
    {
        //counter = Mathf.Clamp(counter, 0, 8);
        if (counter >= 8)
        {
            Tut_img.SetActive(false);
            TutPopUp.SetActive(false);
        }
    }

    IEnumerator ScaleOverTime(float time, GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        Vector3 destinationScale = new Vector3(4f, 4f, 4f);

        float currentTime = 0.0f;

        do
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }

    IEnumerator De_ScaleOverTime(float time, GameObject obj)
    {
        Vector3 originalScale = obj.transform.localScale;
        Vector3 destinationScale = new Vector3(2.7f, 2.7f, 2.7f);

        float currentTime = 0.0f;

        do
        {
            obj.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }
    #region denemeler_SİLME
    //IEnumerator PositionOverTime(float time, GameObject obj)
    //{
    //    Vector3 originalPos = obj.transform.position;
    //    float get_high = 0;
    //    Vector3 destinationPos = new Vector3(transform.position.x, get_high, transform.position.z);
    //    //transform.position = Vector3.Lerp(originalPos.x, originalPos.y + 2, 2f);

    //    float currentTime = 0.0f;

    //    do
    //    {
    //        //get_high = get_high + 800f;
    //        obj.transform.position = Vector3.Lerp(originalPos, destinationPos, currentTime / time);

    //        currentTime += Time.deltaTime;
    //        yield return null;
    //    } while (currentTime <= time);
    //}

    //    public void Move_Komuts()
    //{

    //}

    //IEnumerator AnimateMove(Vector3 origin, Vector3 target, float duration)
    //{
    //    float journey = 0f;
    //    while (journey <= duration)
    //    {
    //        journey = journey + Time.deltaTime;
    //        float percent = Mathf.Clamp01(journey / duration);

    //        transform.position = Vector3.Lerp(origin, target, percent);

    //        yield return null;
    //    }
    //}

    //private IEnumerator MoveToPosition(GameObject obj,Vector3 newPosition, float time)
    //{
    //    float elapsedTime   = 0;
    //    Vector3 startingPos   = transform.position;
    //    while (elapsedTime < time)
    //    {
    //        obj.transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
    //        elapsedTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    //public float speed = 1.0f;
    //public IEnumerator RepeatLerp(Transform a, Transform b, float time)
    //{
    //    float i = 0.0f;
    //    float rate = (1.0f / time) * speed;
    //    while (i < 1.0f)
    //    {
    //        i += Time.deltaTime * rate;
    //        this.transform.position = Vector3.Lerp(a.position, b.position, i);
    //        yield return null;
    //    }
    //}

    //8 de fonksiyon açılış ek olarak 26 da teleport açılması lazım
    //public void Popup_Lvl8()
    //{
    //    Clamp(counter, 0, 2);
    //}

    #endregion
    public void LevelController()
    {
        if (GC.level == 0)
        { 
            Popup_Lvl1();
            if(counter==2)
            BT.Btn_tanit();
        }
        else if (GC.level == 1)
        {
            //counter = 2;
            Popup_Lvl2();
        }
        else if (GC.level == 2)
        {
            //counter=
            Popup_Lvl3();
        }
        else if (GC.level == 7)
        {
            Popup_Lvl8();
        }
        else if (GC.level == 13)
        {
            Popup_Lvl14();
        }
        else if (GC.level == 20)
        {
            Popup_Lvl20();
        }
    }


    public void PopUp_if_Cases()
    {
        //lvl kontrolu burada yapılsın

        LevelController();//Not Clamp için ,counter değeri devamlı update ve check edilmelidir. 
        //counter =counter+1;
        
        if (counter == 2)//sol 0
        {
            
            btn_sol.GetComponent<Image>().sprite = Sol_hlSprite;
            //btn_sol.transform.localScale= new Vector3(3.2f, 3.2f, 3.2f);
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(ScaleOverTime(1, btn_sol));
               
            }

            Sol_text.SetActive(true);
            Sol_img.SetActive(true);
            asel_sol.SetActive(true);
            Sol_text_image.SetActive(true);
            Sol_text_baslik.SetActive(true);
        }
        else
        {
            btn_sol.GetComponent<Image>().sprite = Sol_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, btn_sol));
            }
            //btn_sol.GetComponent<Image>().sprite = Sol_nrmlSprite;
            Sol_text.SetActive(false);
            Sol_img.SetActive(false);
            asel_sol.SetActive(false);
            //asel_sag_nrml.SetActive(true);
            Sol_text_image.SetActive(false);
            Sol_text_baslik.SetActive(false);
        }

        if (counter == 3)//sag 1
        {
            //StartCoroutine(PopupMove_Up());
            StartCoroutine(ScaleOverTime(1, btn_sag));
            btn_sag.GetComponent<Image>().sprite = Sag_hlSprite;
            Sag_text.SetActive(true);
            Sag_img.SetActive(true);
            //Sol_text.SetActive(false);
            //Sol_img.SetActive(false);
            asel_sag_nrml.SetActive(true);
            Sag_text_image.SetActive(true);
            Sag_text_baslik.SetActive(true);
        }
        else
        {
            btn_sag.GetComponent<Image>().sprite = Sag_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, btn_sag));
            }
            //btn_sag.GetComponent<Image>().sprite = Sag_nrmlSprite;
            Sag_text.SetActive(false);
            Sag_img.SetActive(false);
            //asel_sag_nrml.SetActive(false);
            Sag_text_image.SetActive(false);
            Sag_text_baslik.SetActive(false);

        }

        if (counter == 0)//ileri
        {
            //if (TutPopUp.activeSelf == true)
            //{
            //    StartCoroutine(ScaleOverTime(1, btn_ileri));
            //}
            
            btn_ileri.GetComponent<Image>().sprite = ileri_hlSprite;
            ileri_text.SetActive(true);
            ileri_img.SetActive(true);
            asel_sag_ileri.SetActive(true);
            ileri_text_image.SetActive(true);
            ileri_text_baslik.SetActive(true);

        }
        else
        {
            btn_ileri.GetComponent<Image>().sprite = ileri_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, btn_ileri));
            }
            //btn_ileri.GetComponent<Image>().sprite = ileri_nrmlSprite;
            ileri_text.SetActive(false);
            ileri_img.SetActive(false);
            asel_sag_ileri.SetActive(false);
            ileri_text_image.SetActive(false);
            ileri_text_baslik.SetActive(false);

        }

        if (counter == 4)//zipla
        {
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(ScaleOverTime(1, btn_zipla));
            }
            btn_zipla.GetComponent<Image>().sprite = zipla_hlSprite;
            zipla_text.SetActive(true);
            zipla_img.SetActive(true);
            asel_sag_nrml.SetActive(true);
            zipla_text_image.SetActive(true);
            zipla_text_baslik.SetActive(true);

        }
        else
        {
            btn_zipla.GetComponent<Image>().sprite = zipla_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, btn_zipla));
            }
            //btn_zipla.GetComponent<Image>().sprite = zipla_nrmlSprite;
            zipla_text.SetActive(false);
            zipla_img.SetActive(false);
            //  asel_sag_nrml.SetActive(false);
            zipla_text_image.SetActive(false);
            zipla_text_baslik.SetActive(false);
        }

        if (counter == 1)//enerji
        {
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(ScaleOverTime(1, enerji_btn));
            }
            enerji_btn.GetComponent<Image>().sprite = enerji_hlSprite;
            enerji_text.SetActive(true);
            asel_sag_nrml.SetActive(true);
            enerji_text_image.SetActive(true);
            enerji_text_baslik.SetActive(true);
        }
        else
        {
            enerji_btn.GetComponent<Image>().sprite = enerji_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, enerji_btn));
            }
            //enerji_btn.GetComponent<Image>().sprite = enerji_nrmlSprite;
            enerji_text.SetActive(false);
            if(counter==0 || counter == 2)
            {
                asel_sag_nrml.SetActive(false);
            }
            enerji_text_image.SetActive(false);
            enerji_text_baslik.SetActive(false);
        }

        if (counter == 7)//teleport
        {
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(ScaleOverTime(1, teleport_btn));
            }
            teleport_btn.GetComponent<Image>().sprite = teleport_hlSprite;
            teleport_text.SetActive(true);
            asel_teleport.SetActive(true);
            teleport_text_image.SetActive(true);
            teleport_text_baslik.SetActive(true);
        }
        else
        {
            teleport_btn.GetComponent<Image>().sprite = teleport_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, teleport_btn));
            }
            //teleport_btn.GetComponent<Image>().sprite = teleport_nrmlSprite;
            teleport_text.SetActive(false);
            asel_teleport.SetActive(false);
            teleport_text_image.SetActive(false);
            teleport_text_baslik.SetActive(false);
        }

        if (counter == 5)//f1
        {
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(ScaleOverTime(1, f1_btn));
            }
            f1_btn.GetComponent<Image>().sprite = f1_hlSprite;
            f1_text.SetActive(true);
            asel_sag_nrml.SetActive(true);
            f1_text_image.SetActive(true);
            f1_text_baslik.SetActive(true);
        }
        else
        {
            f1_btn.GetComponent<Image>().sprite = f1_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, f1_btn));
            }
            //f1_btn.GetComponent<Image>().sprite = f1_nrmlSprite;
            f1_text.SetActive(false);
            //asel_sag_nrml.SetActive(false);
            f1_text_image.SetActive(false);
            f1_text_baslik.SetActive(false);
        }

        if (counter == 6)//f2
        {
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(ScaleOverTime(1, f2_btn));
            }
            f2_btn.GetComponent<Image>().sprite = f2_hlSprite;
            f2_text.SetActive(true);
            asel_sag_nrml.SetActive(true);
            f2_text_image.SetActive(true);
            f2_text_baslik.SetActive(true);
        }
        else
        {
            f2_btn.GetComponent<Image>().sprite = f2_nrmlSprite;
            if (TutPopUp.activeSelf == true)
            {
                StartCoroutine(De_ScaleOverTime(1, f2_btn));
            }
            //f2_btn.GetComponent<Image>().sprite = f2_nrmlSprite;
            f2_text.SetActive(false);
            //asel_sag_nrml.SetActive(false);
            f2_text_image.SetActive(false);
            f2_text_baslik.SetActive(false);
        }
        Debug.Log("case counter" + counter);
    }

    //public void Set_Popup_Counter(int popup_starting_point)
    //{
    //    if (popup_starting_point == 1)
    //    {
    //        counter = 0;
    //    }
    //    else if (popup_starting_point == 2)
    //    {
    //        counter = 2;

    //    }
    //    else if (popup_starting_point == 3)
    //    {
    //        counter = 4;

    //    }
    //    else if (popup_starting_point == 8)
    //    {
    //        counter = 5;
    //    }
    //    else if (popup_starting_point == 14)
    //    {
    //        counter = 6;
    //    }
    //    else if (popup_starting_point == 20)
    //    {
    //        counter = 7;
    //    }
    //}

    int false_counter1 = 0;
    int false_counter2 = 0;
    public void Set_Popup_Counter1()
    {
        
        false_counter1++;
        
        if (false_counter1>=2)
        {
            Tut_img.SetActive(true);
            TutPopUp.SetActive(true);
            
            counter = 0;
            Debug.Log("Set popup a girdi False_Counter1: " + false_counter1 + " counter: " + counter);
        }
        var currentSelection = EventSystem.current.currentSelectedGameObject;
    }

    public void Set_Popup_Counter2()
    {
       
        false_counter2++;
        if (false_counter2 >= 2)
        {
            counter = 2;
            Debug.Log("Set popup a girdi False_Counter2: " + false_counter2 + " counter: " + counter);
        }
    }


}
