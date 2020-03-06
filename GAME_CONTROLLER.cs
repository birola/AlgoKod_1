using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Serialization;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;

public class GAME_CONTROLLER : MonoBehaviour
{
    [SerializeField] private Animator[] komuts_anim;
    [SerializeField] private Animator[] kup_anim;
    public Image[] change_color;


    public Color color;
    public EventHandler e;
    private GameObject[] _squares;
    public GameObject pawn;
    public int pos_pointer { get; private set; } // 0-48, current position
    public int dir_pointer { get; private set; } //1 right -1 left +7 up -7 down
    private DIRECTIVE selected_directive = new DIRECTIVE();
    private DIRECTIVE selected_tail = new DIRECTIVE();
    private DIRECTIVE MAIN_directive = new DIRECTIVE();
    private DIRECTIVE MAIN_tail = new DIRECTIVE();
    [NotNull] private DIRECTIVE P1_directive = new DIRECTIVE();
    private DIRECTIVE P1_tail = new DIRECTIVE();
    private DIRECTIVE P2_directive = new DIRECTIVE();
    private DIRECTIVE P2_tail = new DIRECTIVE();
    public int selector { get; private set; }
    private int selectedCounter;
    private int mainCounter;
    private int p1Counter;
    private int p2Counter;
    private Coroutine turning_anim;
    public GameObject Function_panel;
    [SerializeField] private Transform GAME_GRID;
    [SerializeField] private Transform SELECTED_ACTION_BAR;
    [SerializeField] private Transform MAIN_ACTION_BAR;
    [SerializeField] private Transform p1_ACTION_BAR;
    [SerializeField] private Transform p2_ACTION_BAR;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject tilePrefab;
    private List<GameObject> selectedButtons = new List<GameObject>();
    private List<GameObject> mainButtons = new List<GameObject>();
    private List<GameObject> p1Buttons = new List<GameObject>();
    private List<GameObject> p2Buttons = new List<GameObject>();
    public int level { get; private set; }
    public AnimPlay Anim_Walk;
    public Image[] komutSprites;
    public GameObject anim_engel;
    public Animator[] yildiz;
    public GameObject[] yildiz_golge;
    public GameObject Popup, Popup_image;
    public PopUp Pop_Up;
    
    public AudioSource bitis_sound_src, teleport_sound_src, ziplama_sound_src, yurume_sound_src, yakma_sound_src,background_music_src;
    public AudioClip bitis_sound_clip, teleport_sound_clip, ziplama_sound_clip, yurume_sound_clip, yakma_sound_clip,background_music_clip;

    public Button ses_ac, ses_ac_1;
    public Sprite ses_kapat,ses_ac_img, ses_kapat_1, ses_ac_img_1;
    private int btn_cntr=0;

    private void updatedirective()
    {
        if (selector == 0)
        {
            MAIN_directive = selected_directive;
            MAIN_tail = selected_tail;
            mainCounter = selectedCounter;
            mainButtons = selectedButtons;
            MAIN_ACTION_BAR = SELECTED_ACTION_BAR;
            MAIN_PARENT_TEMP = SELECTED_PARENT_TEMP;
        }
        else if (selector == 1)
        {
            P1_directive = selected_directive;
            P1_tail = selected_tail;
            p1Counter = selectedCounter;
            p1Buttons = selectedButtons;
            p1_ACTION_BAR = SELECTED_ACTION_BAR;
            P1_PARENT_TEMP = SELECTED_PARENT_TEMP;
        }
        else if (selector == 2)
        {
            P2_directive = selected_directive;
            P2_tail = selected_tail;
            p2Counter = selectedCounter;
            p2Buttons = selectedButtons;
            p2_ACTION_BAR = SELECTED_ACTION_BAR;
            P2_PARENT_TEMP = SELECTED_PARENT_TEMP;
        }
    }

    public void SesChangeSprite()
    {
        
        
        //Debug.Log("btn counter" + btn_cntr);
        //if (btn_cntr == 1) { 
        //ses_ac.GetComponent<Image>().sprite = ses_kapat;
        //    background_music_src.Pause();
        //}
        
        if (btn_cntr % 2 == 0)
        {
            ses_ac.GetComponent<Image>().sprite = ses_kapat;
            ses_ac_1.GetComponent<Image>().sprite = ses_kapat_1;
            background_music_src.Pause();
        }
        else if(btn_cntr % 2 == 1)
        {
            ses_ac.GetComponent<Image>().sprite = ses_ac_img;
            ses_ac_1.GetComponent<Image>().sprite = ses_ac_img_1;
            background_music_src.Play();
        }
        btn_cntr++;
    }

    public Sprite[] function_panel_sprites;
    public void changeselected(int sel)
    {
        Function_panel.transform.GetChild(selector).GetComponent<Image>().sprite =
            selector > 0 ? function_panel_sprites[1] : function_panel_sprites[0];
        updatedirective();
        //////////////////////////////////////////////////////////////////////////
        if (sel == 0)
        {
            selected_directive = MAIN_directive;
            selected_tail = MAIN_tail;
            selectedCounter = mainCounter;
            selectedButtons = mainButtons;
            SELECTED_ACTION_BAR = MAIN_ACTION_BAR;
            SELECTED_PARENT_TEMP = MAIN_PARENT_TEMP;
        }
        else if (sel == 1)
        {
            selected_directive = P1_directive;
            selected_tail = P1_tail;
            selectedCounter =p1Counter;
            selectedButtons = p1Buttons;
            SELECTED_ACTION_BAR = p1_ACTION_BAR;
            SELECTED_PARENT_TEMP = P1_PARENT_TEMP;
        }
        else if (sel == 2)
        {
            selected_directive = P2_directive; 
            selected_tail = P2_tail;
            selectedCounter = p2Counter;
            selectedButtons = p2Buttons;
            SELECTED_ACTION_BAR = p2_ACTION_BAR;
            SELECTED_PARENT_TEMP = P2_PARENT_TEMP;
        }
        selector = sel;
        Function_panel.transform.GetChild(selector).GetComponent<Image>().sprite =
            selector > 0 ? function_panel_sprites[3] : function_panel_sprites[2];
    }
    private void Creator( int a) //// a => 0 null, 1 rotate left, 2 rotate right, 3 move, 4 jump, 5 light the bulb
    {
        GameObject go = Instantiate(buttonPrefab);
        selectedButtons.Add(go);
        var button = go.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => GeneratedOnClick(go));
        //go.GetComponentInChildren<Text>().text = a == 1 ? "Sol" : a == 2 ? "Sağ" : a == 3 ? "Yürü" : a == 4 ? "Zıpla" : a==5? "yak": a==11? "P1": a==22?"P2":"teleport";
        go.GetComponentInChildren<Text>().text = a == 1 ? "" : a == 2 ? "" : a == 3 ? "" : a == 4 ? "" : a == 5 ? "" : a == 11 ? "" : a == 22 ? "" : "";
        go.GetComponentInChildren<Image>().sprite= a == 1 ? komutSprites[0].sprite : a == 2 ? komutSprites[1].sprite : a==3 ? komutSprites[2].sprite : a == 4 ? komutSprites[3].sprite : a == 5 ? komutSprites[4].sprite : a == 11 ? komutSprites[5].sprite : a == 22 ? komutSprites[6].sprite : komutSprites[7].sprite;
        go.transform.SetParent(SELECTED_ACTION_BAR);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        updatedirective();
    }
    private void Creator(int a, int place) //// a => 0 null, 1 rotate left, 2 rotate right, 3 move, 4 jump, 5 light the bulb
    {
        GameObject go = Instantiate(buttonPrefab, SELECTED_ACTION_BAR, true);
        selectedButtons.Insert(place, go);
        var button = go.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => GeneratedOnClick(go));
        go.transform.SetSiblingIndex(place);
        //go.GetComponentInChildren<Text>().text = a == 1 ? "Sol" : a == 2 ? "Sağ" : a == 3 ? "Yürü" : a == 4 ? "Zıpla" : a==5? "yak": a==11? "P1": a==22?"P2":"teleport";
        go.GetComponentInChildren<Text>().text = a == 1 ? "" : a == 2 ? "" : a == 3 ? "" : a == 4 ? "" : a == 5 ? "" : a == 11 ? "" : a == 22 ? "" : "";
        go.GetComponentInChildren<Image>().sprite = a == 1 ? komutSprites[0].sprite : a == 2 ? komutSprites[1].sprite : a == 3 ? komutSprites[2].sprite : a == 4 ? komutSprites[3].sprite : a == 5 ? komutSprites[4].sprite : a == 11 ? komutSprites[5].sprite : a == 22 ? komutSprites[6].sprite : komutSprites[7].sprite;

        /*for (int i = place; i < buttons.Count; i++)
        {
            ACTION_BAR.GetChild(i).SetAsLastSibling();
        }*/
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        updatedirective();
    }
    private void GeneratedOnClick(GameObject go)
    {
        var s = selectedButtons;
        var txt = go.transform.parent.name;
        var h = txt.Equals("main") ? 0 : txt.Equals("p1") ? 1 : 2;
        changeselected(h);
        s = selector==0 ? mainButtons :selector==1 ? p1Buttons : p2Buttons;
        //s = selector == 0 ? mainButtons : selector == 1 ? p1Buttons : p2Buttons;
        directiveRemove(s.IndexOf(go)+1);
        s.Remove((go));
        //updatedirective();
        Destroy(go);
        //UpdateDirectiveStash();
    }
    // Start is called before the first frame update
    private void Start()
    {
        Anim_Walk = GameObject.FindObjectOfType(typeof(AnimPlay)) as AnimPlay;
        level =0;
        _squares = new GameObject[49];
        for (var i = 1; i <= 49; i++)
        {
            _squares[i - 1] = GAME_GRID.Find("P" + i).gameObject;
        }

        selector = 0;
        changeselected(0);
        //level_locked();
        // resetGame();

        //Add_directive(3);Add_directive(3);Add_directive(3);Add_directive(1);Add_directive(3);Add_directive(3); //test
        //StartCoroutine(test());
    }

    /*private void UpdateDirectiveStash()
    {
        var t = Directive_Init;
        var x = Directive_Init;
        do
        {
            t = t._next;
            
            if (t.Equals(null)) break;
        } while (t._next.action != 0);
    }*/
    public Sprite yuvarlak_gecti, yuvarlak_kaldi;
    public Image stay_level, stay_level_change;
    public List<Button> level_button;
    public void next()
    {
        level++;
        PlayerPrefs.SetInt("level", (max > level ? max : level));
        max = PlayerPrefs.GetInt("level");
        if (level > 0)
            level_button[level].GetComponent<Image>().sprite = level % 5 == 0 ? yuvarlak_gecti : stay_level_change.sprite;
        level_button[level + 1].interactable = true;
        level_button[level + 1].GetComponent<Image>().sprite = (level + 1) % 5 == 0 ? yuvarlak_kaldi : stay_level.sprite;

        
        resetGame();
    }
    int max;
    public void buttonLevel_select(int i)
    {
        level = i;
        resetGame();
    }

    public void level_locked()
    {
        PlayerPrefs.SetInt("level", (max > level? max : level));
        max = PlayerPrefs.GetInt("level");
        for (int i = 0; i < level_button.Count; i++)
        {
            if (i <= max)
            {
                level_button[i].interactable = true;
            }
            else
            {
                level_button[i].interactable = false;
            }
        }
    }


    public void directiveAddTest(int action)
    {
        if (selectedButtons.Count<8)
        Add_directive(action);
        else if (selector==0 && selectedButtons.Count<12)
        Add_directive(action);
    }
    public void komutAnim()
    {
        komuts_anim[0].Play("sol_anim");
        komuts_anim[1].Play("sag_anim");
        komuts_anim[2].Play("ileri_anim");
        komuts_anim[3].Play("zipla_anim");
        komuts_anim[4].Play("enerji_anim");
        komuts_anim[5].Play("teleport_anim");
        komuts_anim[6].Play("p1_anim");
        komuts_anim[7].Play("p2_anim");
        komuts_anim[8].Play("play_anim");
        /*komuts_anim[9].Play("AnaPanel_ileri_anim");
        komuts_anim[10].Play("F1_anim");
        komuts_anim[11].Play("F2_anim");*/

        
       
    }

    public Transform camParent;
    public IEnumerator info()
    {
        yield return new WaitForSeconds(3f);
        anim_engel.SetActive(false);
    }
    private IEnumerator yil1()
    {
        yield return new WaitForSeconds(1f);
        yildiz1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yildiz_golge[0].SetActive(true);
    }
    private IEnumerator yil2()
    {
        yield return new WaitForSeconds(1f);
        yildiz2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yildiz_golge[1].SetActive(true);
        yildiz_golge[2].SetActive(true);
    }
    private IEnumerator yil3()
    {
        yield return new WaitForSeconds(1f);
        yildiz3.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yildiz_golge[3].SetActive(true);
        yildiz_golge[4].SetActive(true);
        yildiz_golge[5].SetActive(true);
    }
    public void kapat_yildiz()
    {
        yildiz1.SetActive(false);
        yildiz2.SetActive(false);
        yildiz3.SetActive(false);
        yildiz_golge[0].SetActive(false);
        yildiz_golge[1].SetActive(false);
        yildiz_golge[2].SetActive(false);
        yildiz_golge[3].SetActive(false);
        yildiz_golge[4].SetActive(false);
        yildiz_golge[5].SetActive(false);
    }
    
    List<GameObject> buffers = new List<GameObject>();
    public void resetGame()
    {
        //Popup_counter = PopUp.counter;
        //Popup_counter = 0;
        //Popup_counter = PopUp.counter;
        //Debug.Log("pop counter" + Popup_counter);
        Anim_Walk.Stop();
        selectedButtons.Clear();
        mainButtons.Clear();
        p1Buttons.Clear();
        p2Buttons.Clear();
        first_init();
        pawn.transform.SetParent(_squares[0].transform);
        pawn.transform.localPosition =2.5f* Vector3.back;
        pawn.transform.localEulerAngles = Vector3.zero;
        foreach (var VARIABLE in buffers)
        {
            Destroy(VARIABLE);
        }
        buffers.Clear();
        foreach (var VARIABLE in _squares)
        {
            /*VARIABLE.transform.localPosition = new Vector3(VARIABLE.transform.localPosition.x,VARIABLE.transform.localPosition.y,0);
            VARIABLE.GetComponent<POINT_ACTION>().hasAction = false;
            var t =Random.Range(0, 100);
            if (t < 10) VARIABLE.GetComponent<POINT_ACTION>().hasAction = true;
            //if (t > 70)
            {
                var v = Random.Range(-1, 2);
                VARIABLE.transform.localPosition += 25 * v * Vector3.forward;
            }*/
            VARIABLE.transform.localPosition = new Vector3(VARIABLE.transform.localPosition.x,VARIABLE.transform.localPosition.y,0);
            VARIABLE.GetComponent<MeshRenderer>().enabled = false;
            VARIABLE.GetComponent<POINT_ACTION>().hasAction = false;
            VARIABLE.GetComponent<POINT_ACTION>().teleporter = false;
            VARIABLE.GetComponent<POINT_ACTION>().teleportTo = 0;
            
        }

        #region Levels
        Color[] teleportColors = new Color[5];
        teleportColors[0] = Color.green;
        teleportColors[1] = Color.blue;
        teleportColors[2] = Color.magenta;
        teleportColors[3] = Color.yellow;
        teleportColors[4] = Color.cyan;
        
        if (level == 0)
        {
            F1_Panels_close();
            F2_Panels_close();

            Popup_image.SetActive(true);
            StartCoroutine(info());
            Popup.SetActive(true);
            //PopUp.Popup_Lvl1();

            for (var t = 0; t < 3; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }

            _squares[0].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[1].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[0].GetComponent<POINT_ACTION>().teleportTo = 1;
            _squares[1].GetComponent<POINT_ACTION>().teleportTo = 0;
            
            _squares[2].GetComponent<POINT_ACTION>().hasAction = true;
            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 1)
        {
            F1_Panels_close();
            F2_Panels_close();

            Popup_image.SetActive(true);
            StartCoroutine(info());
            Popup.SetActive(true);
            //PopUp.Popup_Lvl1();

            for (var t = 0; t < 3; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }

            for (var t = 2; t < 2 + 7 + 7; t += 7)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }

            for (var t = 16; t > 13; t--)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }

            _squares[14].GetComponent<POINT_ACTION>().hasAction = true;
            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[14].Play("kup15_anim");

            komutAnim();

            StartCoroutine(info());


        }
        else if (level == 2)
        {
            F1_Panels_close();
            F2_Panels_close();

            Popup_image.SetActive(true);
            StartCoroutine(info());
            Popup.SetActive(true);
            //PopUp.Popup_Lvl3();

            _squares[0].GetComponent<MeshRenderer>().enabled = true; 
            _squares[7].GetComponent<MeshRenderer>().enabled = true; 
            _squares[8].GetComponent<MeshRenderer>().enabled = true; 
            _squares[1].GetComponent<MeshRenderer>().enabled = true;
            _squares[1].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[7].transform.localPosition += -50*Vector3.forward;
            _squares[8].transform.localPosition += -100*Vector3.forward;
            _squares[1].transform.localPosition += -150*Vector3.forward;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_3_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[8].Play("kup9_2_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 3)//5
        {
            F1_Panels_close();
            F2_Panels_close();

            //Popup_image.SetActive(true);
            //StartCoroutine(info());
            //Popup.SetActive(true);
            //PopUp.Popup_Lvl3();

            for (var t = 0; t < 3; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 1 * -50 * Vector3.forward;
                if (t == 0)
                    _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
            }

            for (var t = 14; t < 17; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 2 * -50 * Vector3.forward;
                if (t == 14)
                {
                    _squares[t].transform.localPosition += 1 * -50 * Vector3.forward;
                    _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
                }
            }

            for (var t = 7; t < 10; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 1 * -50 * Vector3.forward;
                if (t == 9)
                {
                    _squares[t].transform.localPosition += 1 * -50 * Vector3.forward;
                    _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
                }

            }
            kup_anim[0].Play("kup1_1_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[2].Play("kup3_1_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[9].Play("kup10_2_anim");
            kup_anim[14].Play("kup15_3_anim");
            kup_anim[15].Play("kup16_2_anim");
            kup_anim[16].Play("kup17_2_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 4)
        {
            F1_Panels_close();
            F2_Panels_close();

            for (var t = 0; t < 5; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true; 
            }for (var t = 7; t < 7+5; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true; 
            }for (var t = 14; t < 14+5; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true; 
            }

            for (var t = 1; t < 16; t += 7)
            {
                _squares[t].transform.localPosition += -50*Vector3.forward;
            }
            for (var t = 3; t < 18; t += 7)
            {
                _squares[t].transform.localPosition += -50*Vector3.forward;
            }
            for (var t = 4; t < 20; t += 7)
            {
                _squares[t].transform.localPosition += -100*Vector3.forward;
            }
            _squares[18].GetComponent<POINT_ACTION>().hasAction = true;


            kup_anim[0].Play("kup1_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[15].Play("kup16_1_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[3].Play("kup4_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[17].Play("kup18_1_anim");
            kup_anim[4].Play("kup5_2_anim");
            kup_anim[11].Play("kup12_2_anim");
            kup_anim[18].Play("kup19_2_anim");

            komutAnim();
            
            StartCoroutine(info());


        }
        else if (level == 5)
        {
            F1_Panels_close();
            F2_Panels_close();
            for (var t = 0; t < 15; t += 7)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                if(t>0)
                    _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
                    
            }for (var t = 1; t < 16; t += 7)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true; 
                if(t>1)
                    _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
            }
            _squares[7].transform.localPosition += 2*-50*Vector3.forward;
            _squares[14].transform.localPosition += 1*-50*Vector3.forward;
            _squares[8].transform.localPosition += 1*-50*Vector3.forward;

            kup_anim[0].Play("kup1_anim");
            kup_anim[7].Play("kup8_2_anim");
            kup_anim[14].Play("kup15_1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[15].Play("kup16_anim");

            komutAnim();
            
            StartCoroutine(info());



        }
        else if (level == 6)//7
        {
            F1_Panels_close();
            F2_Panels_close();
            #region long version
            //for (var t = 0; t < 5; t++)
            //{
            //    _squares[t].GetComponent<MeshRenderer>().enabled = true;

            //}
            //for (var t = 7; t < 12; t++)
            //{
            //    _squares[t].GetComponent<MeshRenderer>().enabled = true;

            //}
            //for (var t = 14; t < 19; t++)
            //{
            //    _squares[t].GetComponent<MeshRenderer>().enabled = true;

            //}
            //for (var t = 21; t < 26; t++)
            //{
            //    _squares[t].GetComponent<MeshRenderer>().enabled = true;

            //}
            //for (var t = 28; t < 33; t++)
            //{
            //    _squares[t].GetComponent<MeshRenderer>().enabled = true;

            //}
            #endregion
            int a = 0, arr_size = 5;
            for (int i = 0; i < 5; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;
                    _squares[j].transform.localPosition += 1 * -50 * Vector3.forward;
                    for (int k = 8; k < 11; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 15; k < 18; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 22; k < 25; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    #region shouldbe shorter for loop version
                    //int z = 0, arr_size2=11;
                    //for (int k = 0; k < 2; k++)
                    //{
                    //    for (int y = z; y < arr_size2; y++)
                    //    {
                    //        _squares[y].transform.localPosition += 1 * -2f * Vector3.forward;

                    //    }
                    //}
                    //z = z + 7;
                    //arr_size2 = z + 3;
                    //_squares[15].transform.localPosition += 1 * -2 * Vector3.forward;
                    //_squares[17].transform.localPosition += 1 * -2 * Vector3.forward;
                    #endregion
                    _squares[16].transform.localPosition += 1 * -2 * Vector3.forward;
                    _squares[16].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[10].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[14].GetComponent<POINT_ACTION>().hasAction = true;
                }
                a = a + 7;
                arr_size = a + 5;
            }
            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[11].Play("kup12_anim");
            kup_anim[18].Play("kup19_anim");
            kup_anim[25].Play("kup26_anim");
            kup_anim[32].Play("kup33_anim");
            kup_anim[31].Play("kup32_anim");
            kup_anim[30].Play("kup31_anim");
            kup_anim[29].Play("kup30_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[15].Play("kup16_1_anim");
            kup_anim[22].Play("kup23_1_anim");
            kup_anim[23].Play("kup24_1_anim");
            kup_anim[24].Play("kup25_1_anim");
            kup_anim[17].Play("kup18_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[9].Play("kup10_1_anim");
            kup_anim[16].Play("kup17_2_anim");

            komutAnim();
            StartCoroutine(info());
        }
        else if (level == 7)
        {
            F1_Panels_open();
            F2_Panels_close();

            Popup_image.SetActive(true);
            StartCoroutine(info());
            Popup.SetActive(true);
            

            for (var t = 0; t < 4; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;

            }
            for (var t = 3; t < 25; t += 7)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            for (var t = 21; t < 24; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            _squares[3].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[24].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[21].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[21].Play("kup22_anim");

            komutAnim();
            
            StartCoroutine(info());

        }
        else if (level == 8)
        {
            F1_Panels_open();
            F2_Panels_close();

            
            
            for (var t = 0; t < 4; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;

            }
            for (var t = 7; t < 11; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 1 * -50 * Vector3.forward;
            }
            for (var t = 14; t < 18; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 2 * -50 * Vector3.forward;
            }
            _squares[3].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[7].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[17].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[9].Play("kup10_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[14].Play("kup15_2_anim");
            kup_anim[15].Play("kup16_2_anim");
            kup_anim[16].Play("kup17_2_anim");
            kup_anim[17].Play("kup18_2_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 9)//16
        {
            F1_Panels_open();
            F2_Panels_close();
            for (var t = 0; t < 4; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            for (var t = 7; t < 11; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            for (var t = 14; t < 18; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            for (var t = 21; t < 25; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            _squares[0].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[3].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[21].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[24].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[24].Play("kup25_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 10)
        {
            F1_Panels_open();
            F2_Panels_close();
            for (var t = 0; t < 5; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (var t = 7; t < 12; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 1 * -50 * Vector3.forward;
            }
            for (var t = 14; t < 19; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 2 * -50 * Vector3.forward;
            }
            for (var t = 21; t < 26; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].transform.localPosition += 3 * -50 * Vector3.forward;
                _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
            }
            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[9].Play("kup10_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[11].Play("kup12_1_anim");
            kup_anim[14].Play("kup15_2_anim");
            kup_anim[15].Play("kup16_2_anim");
            kup_anim[16].Play("kup17_2_anim");
            kup_anim[17].Play("kup18_2_anim");
            kup_anim[18].Play("kup19_2_anim");
            kup_anim[21].Play("kup22_3_anim");
            kup_anim[22].Play("kup23_3_anim");
            kup_anim[23].Play("kup24_3_anim");
            kup_anim[24].Play("kup25_3_anim");
            kup_anim[25].Play("kup26_3_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 11)
        {
            F1_Panels_open();
            F2_Panels_close();
            _squares[0].GetComponent<MeshRenderer>().enabled = true;
            _squares[1].GetComponent<MeshRenderer>().enabled = true;
            _squares[8].GetComponent<MeshRenderer>().enabled = true;
            _squares[9].GetComponent<MeshRenderer>().enabled = true;
            _squares[16].GetComponent<MeshRenderer>().enabled = true;
            _squares[17].GetComponent<MeshRenderer>().enabled = true;
            _squares[24].GetComponent<MeshRenderer>().enabled = true;
            _squares[25].GetComponent<MeshRenderer>().enabled = true;
            _squares[32].GetComponent<MeshRenderer>().enabled = true;
            _squares[32].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[25].Play("kup26_anim");
            kup_anim[32].Play("kup33_anim");

            komutAnim();
            
            StartCoroutine(info());

        }
        else if (level == 12)//17
        {
            F1_Panels_open();
            F2_Panels_close();
            for (var t = 0; t < 3; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (var t = 7; t < 10; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (var t = 14; t < 17; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
                _squares[t].GetComponent<POINT_ACTION>().hasAction = true;
            }

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[16].Play("kup17_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 13) //15
        {
            F1_Panels_open();
            F2_Panels_open();
            for (var t = 0; t < 4; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            for (var t = 10; t < 25; t += 7)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;

            }
            for (var t = 25; t < 28; t++)
            {
                _squares[t].GetComponent<MeshRenderer>().enabled = true;
            }
            _squares[1].transform.localPosition += 1 * -50 * Vector3.forward;
            _squares[26].transform.localPosition += 1 * -50 * Vector3.forward;
            _squares[25].transform.localPosition += 2 * -50 * Vector3.forward;
            _squares[2].transform.localPosition += 2 * -50 * Vector3.forward;
            _squares[3].transform.localPosition += 3 * -50 * Vector3.forward;
            _squares[10].transform.localPosition += 3 * -50 * Vector3.forward;
            _squares[17].transform.localPosition += 3 * -50 * Vector3.forward;
            _squares[24].transform.localPosition += 3 * -50 * Vector3.forward;
            _squares[27].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[27].Play("kup28_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[26].Play("kup27_1_anim");
            kup_anim[2].Play("kup3_2_anim");
            kup_anim[25].Play("kup26_2_anim");
            kup_anim[3].Play("kup4_3_anim");
            kup_anim[10].Play("kup11_3_anim");
            kup_anim[17].Play("kup18_3_anim");
            kup_anim[24].Play("kup25_3_anim");

            komutAnim();

            StartCoroutine(info());
        }
        else if (level == 14)//12
        {
            F1_Panels_open();
            F2_Panels_open();
            for (var h = 0; h < 5; h++)
            {
                _squares[h].GetComponent<MeshRenderer>().enabled = true;
                //_squares[t + 1].GetComponent<POINT_ACTION>().hasAction = true;
                _squares[h].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (var u = 14; u < 19; u++)
            {
                _squares[u].GetComponent<MeshRenderer>().enabled = true;
                //_squares[t - 1].GetComponent<POINT_ACTION>().hasAction = true;
                _squares[u].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (var l = 28; l < 33; l++)
            {
                _squares[l].GetComponent<MeshRenderer>().enabled = true;
                //_squares[t + 1].GetComponent<POINT_ACTION>().hasAction = true;
                _squares[l].GetComponent<POINT_ACTION>().hasAction = true;
            }
            _squares[11].GetComponent<MeshRenderer>().enabled = true;
            _squares[21].GetComponent<MeshRenderer>().enabled = true;


            _squares[0].GetComponent<POINT_ACTION>().hasAction = false;
            _squares[21].GetComponent<POINT_ACTION>().hasAction = false;
            _squares[11].GetComponent<POINT_ACTION>().hasAction = false;
            _squares[28].GetComponent<POINT_ACTION>().hasAction = false;
            _squares[18].GetComponent<POINT_ACTION>().hasAction = false;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[11].Play("kup12_anim");
            kup_anim[18].Play("kup19_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[29].Play("kup30_anim");
            kup_anim[30].Play("kup31_anim");
            kup_anim[31].Play("kup32_anim");
            kup_anim[32].Play("kup33_anim");
            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 15)//18
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 5;
            for (int i = 0; i < 4; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;

                    for (int k = 8; k < 12; k++)
                    {
                        _squares[k].GetComponent<MeshRenderer>().enabled = false;
                    }
                    for (int k = 15; k < 19; k++)
                    {
                        _squares[k].GetComponent<MeshRenderer>().enabled = false;
                    }
                    _squares[7].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[14].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[22].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[24].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[25].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[19].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[12].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[4].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[1].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[2].GetComponent<POINT_ACTION>().hasAction = true;
                }
                a = a + 7;
                arr_size = a + 6;
            }

            _squares[5].GetComponent<MeshRenderer>().enabled = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[5].Play("kup6_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[13].Play("kup14_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[20].Play("kup21_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[25].Play("kup26_anim");
            kup_anim[26].Play("kup27_anim");
            kup_anim[27].Play("kup28_anim");

            komutAnim();

            StartCoroutine(info());

        }
        else if (level == 16) //tek 2
        {
            F1_Panels_open();
            F2_Panels_open();
            _squares[0].GetComponent<MeshRenderer>().enabled = true;
            _squares[1].GetComponent<MeshRenderer>().enabled = true;
            _squares[2].GetComponent<MeshRenderer>().enabled = true;
            _squares[9].GetComponent<MeshRenderer>().enabled = true;
            _squares[16].GetComponent<MeshRenderer>().enabled = true;
            _squares[17].GetComponent<MeshRenderer>().enabled = true;
            _squares[18].GetComponent<MeshRenderer>().enabled = true;
            _squares[25].GetComponent<MeshRenderer>().enabled = true;
            _squares[32].GetComponent<MeshRenderer>().enabled = true;
            _squares[33].GetComponent<MeshRenderer>().enabled = true;
            _squares[34].GetComponent<MeshRenderer>().enabled = true;
            _squares[41].GetComponent<MeshRenderer>().enabled = true;
            _squares[48].GetComponent<MeshRenderer>().enabled = true;
            _squares[47].GetComponent<MeshRenderer>().enabled = true;

            _squares[2].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[16].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[18].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[32].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[34].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[48].GetComponent<POINT_ACTION>().hasAction = true;

            _squares[0].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[1].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[2].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[9].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[16].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[17].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[18].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[25].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[32].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[33].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[34].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[41].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[48].transform.localPosition += 1 * -50f * Vector3.forward;

            _squares[47].GetComponent<POINT_ACTION>().hasAction = true;
            kup_anim[0].Play("kup1_1_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[2].Play("kup3_1_anim");
            kup_anim[9].Play("kup10_1_anim");
            kup_anim[16].Play("kup17_1_anim");
            kup_anim[17].Play("kup18_1_anim");
            kup_anim[18].Play("kup19_1_anim");
            kup_anim[25].Play("kup26_1_anim");
            kup_anim[32].Play("kup33_1_anim");
            kup_anim[33].Play("kup34_1_anim");
            kup_anim[34].Play("kup35_1_anim");
            kup_anim[41].Play("kup42_1_anim");
            kup_anim[48].Play("kup49_1_anim");
            kup_anim[47].Play("kup48_anim");
            komutAnim();
            StartCoroutine(info());
        }
        else if (level == 17)
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 5;
            for (int i = 0; i < 4; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;
                    //_squares[j].transform.localPosition += 1 * -50 * Vector3.forward;
                    for (int k = 7; k < 12; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 14; k < 19; k++)
                    {
                        _squares[k].transform.localPosition += 2 * -2f * Vector3.forward;
                    }
                    for (int k = 21; k < 26; k++)
                    {
                        _squares[k].transform.localPosition += 3 * -2f * Vector3.forward;
                        _squares[k].GetComponent<POINT_ACTION>().hasAction = true;


                    }
                    for (int k = 0; k < 5; k++)
                    {
                        _squares[k].GetComponent<POINT_ACTION>().hasAction = true;
                    }
                    #region shouldbe shorter for loop version
                    //int z = 0, arr_size2=11;
                    //for (int k = 0; k < 2; k++)
                    //{
                    //    for (int y = z; y < arr_size2; y++)
                    //    {
                    //        _squares[y].transform.localPosition += 1 * -2f * Vector3.forward;

                    //    }
                    //}
                    //z = z + 7;
                    //arr_size2 = z + 3;
                    //_squares[15].transform.localPosition += 1 * -2 * Vector3.forward;
                    //_squares[17].transform.localPosition += 1 * -2 * Vector3.forward;
                    #endregion
                    _squares[0].GetComponent<POINT_ACTION>().hasAction = false;
                    _squares[25].GetComponent<POINT_ACTION>().hasAction = false;

                }
                a = a + 7;
                arr_size = a + 5;

            }
            _squares[11].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[18].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[9].Play("kup10_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[11].Play("kup12_1_anim");
            kup_anim[14].Play("kup15_2_anim");
            kup_anim[15].Play("kup16_2_anim");
            kup_anim[16].Play("kup17_2_anim");
            kup_anim[17].Play("kup18_2_anim");
            kup_anim[18].Play("kup19_2_anim");
            kup_anim[21].Play("kup22_3_anim");
            kup_anim[22].Play("kup23_3_anim");
            kup_anim[23].Play("kup24_3_anim");
            kup_anim[24].Play("kup25_3_anim");
            kup_anim[25].Play("kup26_3_anim");

            //komutAnim();

            //StartCoroutine(info());
        }
        else if (level == 18) //çift1
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 5;
            for (int i = 0; i < 5; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;
                    //_squares[j].transform.localPosition += 1 * -50f * Vector3.forward;


                }
                a = a + 7;
                arr_size = a + 5;

            }
            _squares[2].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[4].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[14].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[16].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[18].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[28].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[30].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[32].GetComponent<POINT_ACTION>().hasAction = true;

            _squares[8].transform.localPosition += 1 * +50f * Vector3.forward;
            _squares[10].transform.localPosition += 1 * +50f * Vector3.forward;
            _squares[22].transform.localPosition += 1 * +50f * Vector3.forward;
            _squares[24].transform.localPosition += 1 * +50f * Vector3.forward;

            kup_anim[0].Play("kup1_1_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[2].Play("kup3_1_anim");
            kup_anim[3].Play("kup4_1_anim");
            kup_anim[4].Play("kup5_1_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[9].Play("kup10_1_anim");
            kup_anim[11].Play("kup12_1_anim");
            kup_anim[14].Play("kup15_1_anim");
            kup_anim[15].Play("kup16_1_anim");
            kup_anim[16].Play("kup17_1_anim");
            kup_anim[17].Play("kup18_1_anim");
            kup_anim[18].Play("kup19_1_anim");
            kup_anim[21].Play("kup22_1_anim");
            kup_anim[23].Play("kup24_1_anim");
            kup_anim[25].Play("kup26_1_anim");
            kup_anim[28].Play("kup29_1_anim");
            kup_anim[29].Play("kup30_1_anim");
            kup_anim[30].Play("kup31_1_anim");
            kup_anim[31].Play("kup32_1_anim");
            kup_anim[32].Play("kup33_1_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[24].Play("kup25_anim");

            komutAnim();

            StartCoroutine(info());
        }//18bolum
        else if (level == 19)//13
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 5;
            for (int i = 0; i < 5; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;

                }
                a = a + 7;
                arr_size = a + 6;
            }
            for (int k = 0; k < 31; k += 15)
            {

                _squares[k].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (int k = 1; k < 32; k += 15)
            {
                _squares[k].transform.localPosition += 1 * -50f * Vector3.forward;
                _squares[k].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (int k = 2; k < 33; k += 15)
            {
                _squares[k].transform.localPosition += 2 * -50f * Vector3.forward;
                _squares[k].GetComponent<POINT_ACTION>().hasAction = true;
            }
            for (int k = 3; k < 34; k += 15)
            {
                _squares[k].transform.localPosition += 3 * -50f * Vector3.forward;
                _squares[k].GetComponent<POINT_ACTION>().hasAction = true;
            }
            _squares[5].GetComponent<MeshRenderer>().enabled = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[5].Play("kup6_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[11].Play("kup12_anim");
            kup_anim[12].Play("kup13_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[25].Play("kup26_anim");
            kup_anim[26].Play("kup27_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[29].Play("kup30_anim");
            kup_anim[30].Play("kup31_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[16].Play("kup17_1_anim");
            kup_anim[31].Play("kup32_1_anim");
            kup_anim[2].Play("kup3_2_anim");
            kup_anim[17].Play("kup18_2_anim");
            kup_anim[32].Play("kup33_2_anim");
            kup_anim[3].Play("kup4_3_anim");
            kup_anim[18].Play("kup19_3_anim");
            kup_anim[33].Play("kup34_3_anim");

            komutAnim();

            StartCoroutine(info());
        }//bolum 19
        else if (level == 20) //ışın1
        {
            F1_Panels_open();
            F2_Panels_open();

            Popup_image.SetActive(true);
            StartCoroutine(info());
            Popup.SetActive(true);

            int a = 0, arr_size = 6;
            for (int i = 0; i < 6; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 6;

            }

            _squares[1].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[1].GetComponent<POINT_ACTION>().teleportTo = 17;
            _squares[1].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[17].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[17].GetComponent<POINT_ACTION>().teleportTo = 1;
            _squares[17].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[19].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[19].GetComponent<POINT_ACTION>().teleportTo = 33;
            _squares[19].GetComponent<POINT_ACTION>().setColor(teleportColors[1]);
            
            _squares[33].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[33].GetComponent<POINT_ACTION>().teleportTo = 19;
            _squares[33].GetComponent<POINT_ACTION>().setColor(teleportColors[1]);
            
            
            
            _squares[19].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[18].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[17].transform.localPosition += 3 * -50f * Vector3.forward;

            _squares[33].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[32].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[32].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[5].Play("kup6_anim");
            kup_anim[6].Play("kup7_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[11].Play("kup12_anim");
            kup_anim[12].Play("kup13_anim");
            kup_anim[13].Play("kup14_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[25].Play("kup26_anim");
            kup_anim[26].Play("kup27_anim");
            kup_anim[27].Play("kup28_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[29].Play("kup30_anim");
            kup_anim[30].Play("kup31_anim");
            kup_anim[31].Play("kup32_anim");
            kup_anim[35].Play("kup36_anim");
            kup_anim[36].Play("kup37_anim");
            kup_anim[37].Play("kup38_anim");
            kup_anim[38].Play("kup39_anim");
            kup_anim[39].Play("kup40_anim");
            kup_anim[40].Play("kup41_anim");
            kup_anim[32].Play("kup33_2_anim");
            kup_anim[17].Play("kup18_3_anim");
            kup_anim[18].Play("kup19_3_anim");
            kup_anim[19].Play("kup20_3_anim");
            kup_anim[33].Play("kup34_3_anim");
            komutAnim();

            StartCoroutine(info());
        }
        else if (level == 21) //ışın2
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 4;
            for (int i = 0; i < 5; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 4;
            }
            _squares[3].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[3].GetComponent<POINT_ACTION>().teleportTo = 14;
            _squares[3].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[14].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[14].GetComponent<POINT_ACTION>().teleportTo = 3;
            _squares[14].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[17].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[17].GetComponent<POINT_ACTION>().teleportTo = 28;
            _squares[17].GetComponent<POINT_ACTION>().setColor(teleportColors[1]);
            
            _squares[28].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[28].GetComponent<POINT_ACTION>().teleportTo = 17;
            _squares[28].GetComponent<POINT_ACTION>().setColor(teleportColors[1]);
            
            _squares[0].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[1].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[2].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[3].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[14].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[15].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[16].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[17].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[28].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[29].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[30].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[31].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[1].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[2].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[15].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[16].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[29].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[30].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[31].GetComponent<POINT_ACTION>().hasAction = true;
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[0].Play("kup1_2_anim");
            kup_anim[1].Play("kup2_2_anim");
            kup_anim[2].Play("kup3_2_anim");
            kup_anim[3].Play("kup4_2_anim");
            kup_anim[14].Play("kup15_2_anim");
            kup_anim[15].Play("kup16_2_anim");
            kup_anim[16].Play("kup17_2_anim");
            kup_anim[17].Play("kup18_2_anim");
            kup_anim[28].Play("kup29_2_anim");
            kup_anim[29].Play("kup30_2_anim");
            kup_anim[30].Play("kup31_2_anim");
            kup_anim[31].Play("kup32_2_anim");
            komutAnim();

            StartCoroutine(info());
        }
        else if (level == 22) //ışın3
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 7;
            for (int i = 0; i < 7; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 7;

            }
            _squares[5].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[12].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[13].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[6].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[20].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[27].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[34].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[41].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[47].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[40].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[48].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[46].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[45].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[44].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[43].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[36].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[35].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[42].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[5].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[13].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[12].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[41].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[40].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[47].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[43].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[36].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[35].GetComponent<POINT_ACTION>().hasAction = true;
            
            _squares[0].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[0].GetComponent<POINT_ACTION>().teleportTo = 6;
            _squares[0].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[6].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[6].GetComponent<POINT_ACTION>().teleportTo = 0;
            _squares[6].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            
            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[11].Play("kup12_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[18].Play("kup19_anim");
            kup_anim[19].Play("kup20_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[25].Play("kup26_anim");
            kup_anim[26].Play("kup27_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[29].Play("kup30_anim");
            kup_anim[30].Play("kup31_anim");
            kup_anim[31].Play("kup32_anim");
            kup_anim[32].Play("kup33_anim");
            kup_anim[33].Play("kup34_anim");
            kup_anim[37].Play("kup38_anim");
            kup_anim[38].Play("kup39_anim");
            kup_anim[39].Play("kup40_anim");
            kup_anim[5].Play("kup6_2_anim");
            kup_anim[12].Play("kup13_2_anim");
            kup_anim[13].Play("kup14_2_anim");
            kup_anim[20].Play("kup21_2_anim");
            kup_anim[27].Play("kup28_2_anim");
            kup_anim[34].Play("kup35_2_anim");
            kup_anim[41].Play("kup42_2_anim");
            kup_anim[40].Play("kup41_2_anim");
            kup_anim[47].Play("kup48_2_anim");
            kup_anim[46].Play("kup47_2_anim");
            kup_anim[45].Play("kup46_2_anim");
            kup_anim[44].Play("kup45_2_anim");
            kup_anim[43].Play("kup44_2_anim");
            kup_anim[36].Play("kup37_2_anim");
            kup_anim[35].Play("kup36_2_anim");
            kup_anim[6].Play("kup7_3_anim");
            kup_anim[48].Play("kup49_3_anim");
            kup_anim[42].Play("kup43_3_anim");
            komutAnim();

            StartCoroutine(info());
        }
        else if (level == 23) //ışın4
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 7;
            for (int i = 0; i < 7; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 7;

            }
            _squares[0].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[1].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[7].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[8].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[14].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[15].transform.localPosition += 2 * -50f * Vector3.forward;

            _squares[33].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[34].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[40].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[41].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[47].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[48].transform.localPosition += 2 * -50f * Vector3.forward;

            _squares[5].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[6].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[4].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[11].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[18].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[25].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[24].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[23].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[30].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[37].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[44].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[43].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[42].transform.localPosition += 1 * -50f * Vector3.forward;

            _squares[7].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[14].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[15].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[8].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[41].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[34].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[40].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[33].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[44].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[23].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[25].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[4].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[6].GetComponent<POINT_ACTION>().hasAction = true;
            
            _squares[1].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[1].GetComponent<POINT_ACTION>().teleportTo = 48;
            _squares[1].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[48].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[48].GetComponent<POINT_ACTION>().teleportTo = 1;
            _squares[48].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[47].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[47].GetComponent<POINT_ACTION>().teleportTo = 42;
            _squares[47].GetComponent<POINT_ACTION>().setColor(teleportColors[1]);
            
            _squares[42].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[42].GetComponent<POINT_ACTION>().teleportTo = 47;
            _squares[42].GetComponent<POINT_ACTION>().setColor(teleportColors[1]);

            kup_anim[2].Play("kup3_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[29].Play("kup30_anim");
            kup_anim[35].Play("kup36_anim");
            kup_anim[36].Play("kup37_anim");
            kup_anim[12].Play("kup13_anim");
            kup_anim[13].Play("kup14_anim");
            kup_anim[19].Play("kup20_anim");
            kup_anim[20].Play("kup21_anim");
            kup_anim[26].Play("kup27_anim");
            kup_anim[27].Play("kup28_anim");
            kup_anim[31].Play("kup32_anim");
            kup_anim[32].Play("kup33_anim");
            kup_anim[38].Play("kup39_anim");
            kup_anim[39].Play("kup40_anim");
            kup_anim[45].Play("kup46_anim");
            kup_anim[46].Play("kup47_anim");
            kup_anim[42].Play("kup43_1_anim");
            kup_anim[43].Play("kup44_1_anim");
            kup_anim[44].Play("kup45_1_anim");
            kup_anim[37].Play("kup38_1_anim");
            kup_anim[30].Play("kup31_1_anim");
            kup_anim[23].Play("kup24_1_anim");
            kup_anim[24].Play("kup25_1_anim");
            kup_anim[25].Play("kup26_1_anim");
            kup_anim[18].Play("kup19_1_anim");
            kup_anim[11].Play("kup12_1_anim");
            kup_anim[4].Play("kup5_1_anim");
            kup_anim[5].Play("kup6_1_anim");
            kup_anim[6].Play("kup7_1_anim");
            kup_anim[0].Play("kup1_2_anim");
            kup_anim[1].Play("kup2_2_anim");
            kup_anim[7].Play("kup8_2_anim");
            kup_anim[8].Play("kup9_2_anim");
            kup_anim[14].Play("kup15_2_anim");
            kup_anim[15].Play("kup16_2_anim");
            kup_anim[33].Play("kup34_2_anim");
            kup_anim[39].Play("kup35_2_anim");
            kup_anim[40].Play("kup41_2_anim");
            kup_anim[41].Play("kup42_2_anim");
            kup_anim[47].Play("kup48_2_anim");
            kup_anim[48].Play("kup49_2_anim");
            komutAnim();

            StartCoroutine(info());
        }
        else if (level == 24) //25 bolum
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 3;
            for (int i = 0; i < 5; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 3;

            }
            _squares[1].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[15].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[29].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[2].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[16].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[30].transform.localPosition += 2 * -50f * Vector3.forward;

            _squares[8].GetComponent<MeshRenderer>().enabled = false;
            _squares[9].GetComponent<MeshRenderer>().enabled = false;
            _squares[22].GetComponent<MeshRenderer>().enabled = false;
            _squares[23].GetComponent<MeshRenderer>().enabled = false;

            _squares[2].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[16].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[30].GetComponent<POINT_ACTION>().hasAction = true;
            kup_anim[0].Play("kup1_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[15].Play("kup16_1_anim");
            kup_anim[29].Play("kup30_1_anim");
            kup_anim[2].Play("kup3_2_anim");
            kup_anim[16].Play("kup17_2_anim");
            kup_anim[30].Play("kup31_2_anim");
            komutAnim();

            StartCoroutine(info());
        }//bolum25
        else if (level == 25) //çift3 bolum 26
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 7;
            for (int i = 0; i < 4; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 7;

            }

            _squares[3].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[10].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[11].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[19].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[18].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[17].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[16].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[5].GetComponent<MeshRenderer>().enabled = false;
            _squares[6].GetComponent<MeshRenderer>().enabled = false;
            _squares[26].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[27].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[20].transform.localPosition += 1 * -50f * Vector3.forward;
            for (int k = 21; k < 26; k++)
            {
                _squares[k].GetComponent<MeshRenderer>().enabled = false;
            }
            _squares[0].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[13].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[16].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[7].GetComponent<MeshRenderer>().enabled = false;
            _squares[8].GetComponent<MeshRenderer>().enabled = false;
            _squares[9].GetComponent<MeshRenderer>().enabled = false;
            _squares[15].GetComponent<MeshRenderer>().enabled = false;
            _squares[14].GetComponent<MeshRenderer>().enabled = false;

            kup_anim[0].Play("kup1_anim");
            kup_anim[1].Play("kup2_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[12].Play("kup13_anim");
            kup_anim[13].Play("kup14_anim");
            kup_anim[3].Play("kup4_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[11].Play("kup12_1_anim");
            kup_anim[20].Play("kup21_1_anim");
            kup_anim[27].Play("kup28_2_anim");
            kup_anim[26].Play("kup27_2_anim");
            kup_anim[19].Play("kup20_2_anim");
            kup_anim[18].Play("kup19_3_anim");
            kup_anim[17].Play("kup18_3_anim");
            kup_anim[16].Play("kup17_3_anim");
            komutAnim();

            StartCoroutine(info());
        }//zor bolum
        else if (level == 26)//14
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 5;
            for (int i = 0; i < 5; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;

                    for (int k = 0; k < 2; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 7; k < 9; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 3; k < 5; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 10; k < 12; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 21; k < 23; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 24; k < 26; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 28; k < 30; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    for (int k = 31; k < 33; k++)
                    {
                        _squares[k].transform.localPosition += 1 * -2f * Vector3.forward;
                    }
                    _squares[0].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[3].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[4].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[7].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[25].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[28].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[29].GetComponent<POINT_ACTION>().hasAction = true;
                    _squares[32].GetComponent<POINT_ACTION>().hasAction = true;

                }
                a = a + 7;
                arr_size = a + 5;
            }
            kup_anim[0].Play("kup1_1_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[3].Play("kup4_1_anim");
            kup_anim[4].Play("kup5_1_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[11].Play("kup12_1_anim");
            kup_anim[21].Play("kup22_1_anim");
            kup_anim[22].Play("kup23_1_anim");
            kup_anim[24].Play("kup25_1_anim");
            kup_anim[25].Play("kup26_1_anim");
            kup_anim[28].Play("kup29_1_anim");
            kup_anim[29].Play("kup30_1_anim");
            kup_anim[31].Play("kup32_1_anim");
            kup_anim[32].Play("kup33_1_anim");
            kup_anim[2].Play("kup3_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[18].Play("kup19_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[30].Play("kup31_anim");

            komutAnim();
            
            StartCoroutine(info());

        }//zor bolum
        else if (level == 27) //çift2
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 5;
            for (int i = 0; i < 4; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;
                    //_squares[j].transform.localPosition += 1 * -50f * Vector3.forward;
                    _squares[1].GetComponent<MeshRenderer>().enabled = false;
                    _squares[2].GetComponent<MeshRenderer>().enabled = false;
                    _squares[4].GetComponent<MeshRenderer>().enabled = false;
                    _squares[9].GetComponent<MeshRenderer>().enabled = false;
                    _squares[16].GetComponent<MeshRenderer>().enabled = false;

                }
                a = a + 7;
                arr_size = a + 5;

            }
            for (int k = 21; k < 26; k++)
            {
                _squares[k].transform.localPosition += 2 * -50f * Vector3.forward;
                _squares[k].GetComponent<POINT_ACTION>().hasAction = true;

            }

            _squares[7].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[8].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[14].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[15].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[10].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[11].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[17].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[18].GetComponent<POINT_ACTION>().hasAction = true;

            _squares[7].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[8].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[10].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[11].transform.localPosition += 1 * -50f * Vector3.forward;

            _squares[14].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[15].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[17].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[18].transform.localPosition += 1 * -50f * Vector3.forward;

            kup_anim[0].Play("kup1_anim");
            kup_anim[3].Play("kup4_anim");
            kup_anim[7].Play("kup8_1_anim");
            kup_anim[8].Play("kup9_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[11].Play("kup12_1_anim");
            kup_anim[14].Play("kup15_1_anim");
            kup_anim[15].Play("kup16_1_anim");
            kup_anim[17].Play("kup18_1_anim");
            kup_anim[18].Play("kup19_1_anim");
            kup_anim[21].Play("kup22_2_anim");
            kup_anim[22].Play("kup23_2_anim");
            kup_anim[23].Play("kup24_2_anim");
            kup_anim[24].Play("kup25_2_anim");
            kup_anim[25].Play("kup26_2_anim");
            kup_anim[26].Play("kup27_2_anim");
            komutAnim();
            
            StartCoroutine(info());
        }//25bolum
        else if (level == 28) //ışın5
        {
            F1_Panels_open();
            F2_Panels_open();
            int a = 0, arr_size = 7;
            for (int i = 0; i < 7; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 7;

            }
            _squares[1].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[43].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[2].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[44].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[3].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[45].transform.localPosition += 3 * -50f * Vector3.forward;

            _squares[41].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[32].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[34].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[25].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[27].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[18].transform.localPosition += 3 * -50f * Vector3.forward;
            
            _squares[3].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[3].GetComponent<POINT_ACTION>().teleportTo = 45;
            _squares[3].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[45].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[45].GetComponent<POINT_ACTION>().teleportTo = 3;
            _squares[45].GetComponent<POINT_ACTION>().setColor(teleportColors[0]);
            
            _squares[43].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[43].GetComponent<POINT_ACTION>().teleportTo = 27;
            _squares[43].GetComponent<POINT_ACTION>().setColor(teleportColors[1]);
            
            _squares[27].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[27].GetComponent<POINT_ACTION>().teleportTo = 43;
            _squares[27].GetComponent<POINT_ACTION>().setColor(teleportColors[1]); 
            
            _squares[41].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[41].GetComponent<POINT_ACTION>().teleportTo = 25;
            _squares[41].GetComponent<POINT_ACTION>().setColor(teleportColors[2]);
            
            _squares[25].GetComponent<POINT_ACTION>().teleporter = true;
            _squares[25].GetComponent<POINT_ACTION>().teleportTo = 41;
            _squares[25].GetComponent<POINT_ACTION>().setColor(teleportColors[2]);

            _squares[18].GetComponent<POINT_ACTION>().hasAction = true;
            kup_anim[0].Play("kup1_anim");
            kup_anim[4].Play("kup5_anim");
            kup_anim[5].Play("kup6_anim");
            kup_anim[6].Play("kup7_anim");
            kup_anim[7].Play("kup8_anim");
            kup_anim[8].Play("kup9_anim");
            kup_anim[9].Play("kup10_anim");
            kup_anim[10].Play("kup11_anim");
            kup_anim[11].Play("kup12_anim");
            kup_anim[12].Play("kup13_anim");
            kup_anim[13].Play("kup14_anim");
            kup_anim[14].Play("kup15_anim");
            kup_anim[15].Play("kup16_anim");
            kup_anim[16].Play("kup17_anim");
            kup_anim[17].Play("kup18_anim");
            kup_anim[19].Play("kup20_anim");
            kup_anim[20].Play("kup21_anim");
            kup_anim[21].Play("kup22_anim");
            kup_anim[22].Play("kup23_anim");
            kup_anim[23].Play("kup24_anim");
            kup_anim[24].Play("kup25_anim");
            kup_anim[26].Play("kup27_anim");
            kup_anim[28].Play("kup29_anim");
            kup_anim[29].Play("kup30_anim");
            kup_anim[30].Play("kup31_anim");
            kup_anim[31].Play("kup32_anim");
            kup_anim[33].Play("kup34_anim");
            kup_anim[35].Play("kup36_anim");
            kup_anim[36].Play("kup37_anim");
            kup_anim[37].Play("kup38_anim");
            kup_anim[38].Play("kup39_anim");
            kup_anim[39].Play("kup40_anim");
            kup_anim[40].Play("kup41_anim");
            kup_anim[42].Play("kup43_anim");
            kup_anim[46].Play("kup47_anim");
            kup_anim[47].Play("kup48_anim");
            kup_anim[48].Play("kup49_anim");
            kup_anim[1].Play("kup2_1_anim");
            kup_anim[43].Play("kup44_1_anim");
            kup_anim[32].Play("kup33_1_anim");
            kup_anim[41].Play("kup42_1_anim");
            kup_anim[2].Play("kup3_2_anim");
            kup_anim[44].Play("kup45_2_anim");
            kup_anim[25].Play("kup26_2_anim");
            kup_anim[34].Play("kup35_2_anim");
            kup_anim[3].Play("kup4_3_anim");
            kup_anim[45].Play("kup46_3_anim");
            kup_anim[18].Play("kup19_3_anim");
            kup_anim[27].Play("kup28_3_anim");
            komutAnim();
            
            StartCoroutine(info());
        }
        else if (level == 29) //çift 4
        {
            F1_Panels_open();
            F2_Panels_open();
            //cameraObject.GetComponent<touchScript>().enabled = true;
            int a = 0, arr_size = 4;
            for (int i = 0; i < 7; i++)
            {
                for (int j = a; j < arr_size; j++)
                {
                    _squares[j].GetComponent<MeshRenderer>().enabled = true;


                }
                a = a + 7;
                arr_size = a + 4;



            }
            _squares[14].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[28].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[44].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[38].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[10].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[2].transform.localPosition += 1 * -50f * Vector3.forward;
            _squares[22].transform.localPosition += 1 * -50f * Vector3.forward;

            _squares[1].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[7].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[35].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[43].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[17].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[23].transform.localPosition += 2 * -50f * Vector3.forward;
            _squares[31].transform.localPosition += 2 * -50f * Vector3.forward;

            _squares[0].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[42].transform.localPosition += 3 * -50f * Vector3.forward;
            _squares[24].transform.localPosition += 3 * -50f * Vector3.forward;

            _squares[8].GetComponent<MeshRenderer>().enabled = false;
            _squares[9].GetComponent<MeshRenderer>().enabled = false;
            _squares[16].GetComponent<MeshRenderer>().enabled = false;
            _squares[15].GetComponent<MeshRenderer>().enabled = false;
            _squares[29].GetComponent<MeshRenderer>().enabled = false;
            _squares[30].GetComponent<MeshRenderer>().enabled = false;
            _squares[36].GetComponent<MeshRenderer>().enabled = false;
            _squares[37].GetComponent<MeshRenderer>().enabled = false;

            _squares[14].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[28].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[44].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[38].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[10].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[2].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[22].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[1].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[7].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[35].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[43].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[17].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[23].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[31].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[0].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[42].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[24].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[3].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[21].GetComponent<POINT_ACTION>().hasAction = true;
            _squares[45].GetComponent<POINT_ACTION>().hasAction = true;

            kup_anim[2].Play("kup3_1_anim");
            kup_anim[10].Play("kup11_1_anim");
            kup_anim[14].Play("kup15_1_anim");
            kup_anim[22].Play("kup23_1_anim");
            kup_anim[28].Play("kup29_1_anim");
            kup_anim[38].Play("kup39_1_anim");
            kup_anim[44].Play("kup45_1_anim");
            kup_anim[1].Play("kup2_2_anim");
            kup_anim[7].Play("kup8_2_anim");
            kup_anim[17].Play("kup18_2_anim");
            kup_anim[23].Play("kup24_2_anim");
            kup_anim[31].Play("kup32_2_anim");
            kup_anim[35].Play("kup36_2_anim");
            kup_anim[43].Play("kup44_2_anim");
            kup_anim[0].Play("kup1_3_anim");
            kup_anim[42].Play("kup43_3_anim");
            kup_anim[24].Play("kup25_3_anim");
            komutAnim();

            StartCoroutine(info());
        }



        #endregion


        var tempPoint = new Vector3();
        var tempCounter = 0f;
        foreach (var VARIABLE in _squares)
        {
            if (VARIABLE.GetComponent<MeshRenderer>().enabled)
            {
//                Debug.Log("ins");
                tempPoint += VARIABLE.transform.position;
                tempCounter++;
            }
        }
        tempPoint = (1/tempCounter)*tempPoint;
        camParent.position = tempPoint;
        /*Debug.Log("temp" +tempPoint);
        Debug.Log("cam"+camParent.position);
        Debug.Log("must"+_squares[1].transform.position);*/
        foreach (var VARIABLE in selectedButtons)
        {
            Destroy(VARIABLE);
        }
        foreach (var VARIABLE in _squares)
        {
            if((VARIABLE).GetComponent<POINT_ACTION>().teleporter) continue;
            VARIABLE.GetComponent<POINT_ACTION>().def();
        }
        getActionCount();
        checkActions();
        initalize_directive();
        
        foreach (var VARIABLE in actionPlaces)
        {
            _squares[VARIABLE].GetComponent<POINT_ACTION>().turnOff();
        }

        foreach (var VARIABLE in _squares)
        {
            var x = VARIABLE.transform.localPosition.z;
            var counter = 1;
            while (x < 0)
            {
                var go = Instantiate(tilePrefab, VARIABLE.transform);
                buffers.Add(go);
                go.transform.localPosition = 2.2f * counter*Vector3.forward;
                go.transform.localScale = Vector3.one;
                go.transform.localEulerAngles = Vector3.zero;
                counter++;
                x += 50;
            }
        }
        anim_engel.SetActive(true);
        Function_panel.SetActive(false);
        FinishScreen.SetActive(false);
        Function_panel.SetActive(true);
        RunTest();
    }

    public void RunTest()
    {
        pos_pointer = 0;
        dir_pointer = 1;

        pawn.transform.SetParent(_squares[0].transform);
        pawn.transform.localPosition = 2.5f * Vector3.back;
        pawn.transform.localEulerAngles = Vector3.zero;
        if (level == 2)
        {
            dir_pointer = 7;
            pawn.transform.localEulerAngles = 90 * Vector3.forward;
        }
        //Debug.Log(Directive_Init._next.action);
        //if(!tesst.Equals(null))
        if (testUp)
        {
            testUp = false;
            StopCoroutine(tesst);

        }
        tesst = StartCoroutine(test());
    }

    private Coroutine tesst;
    private List<bool> actionBools = new List<bool>();
    private List<int> actionPlaces = new List<int>();
    public void getActionCount()
    {
        actionBools.Clear();
        actionPlaces.Clear();
        for (var t = 0; t < _squares.Length; t++)
        {
            if (_squares[t].GetComponent<POINT_ACTION>().hasAction)
            {
                actionBools.Add(_squares[t].GetComponent<POINT_ACTION>().actionState);
                actionPlaces.Add(t);
            }
        }
    }

    [SerializeField] private GameObject FinishScreen;
    private int required_count_0 = 3, required_count_1 = 9, required_count_2 = 6, required_count_3 = 11, required_count_4 = 8,
        required_count_5 = 12, required_count_6 = 12, required_count_7 = 9, required_count_8 = 13, required_count_9 = 10,
        required_count_10 = 17, required_count_11 = 9, required_count_12 = 14, required_count_13 = 11, required_count_14 = 19,
        required_count_15 = 20, required_count_16 = 18, required_count_17 = 18, required_count_18 = 24, required_count_19 = 20,
        required_count_20 = 9, required_count_21 = 14, required_count_22 = 24, required_count_23 = 22, required_count_24 = 17,
        required_count_25 = 21, required_count_26 = 11, required_count_27 = 28, required_count_28 = 19, required_count_29 = 26;
    public GameObject yildiz1, yildiz2, yildiz3;
    public GameObject[] pnl_star;
    public Image level_color;
    public Image[] level_buttons_color;
    public bool checkActions()
    {
        
        foreach (var VARIABLE in actionBools)
        {
            if (!VARIABLE) return false;
        }
        StartCoroutine(Wait_action());
        FinishScreen.SetActive(true);
        //ses yaptırma
        bitis_sound_src.PlayOneShot(bitis_sound_clip);
        if (level == 0)
        {
            if (actioncount == required_count_0)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                pnl_star[2].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_0 && actioncount < 5)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[1].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_0 && actioncount < 6)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[0].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else
            {

            }
        }
        if (level == 1)
        {
            if (actioncount == required_count_1)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                pnl_star[5].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_1 && actioncount < 11)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[4].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_1 && actioncount < 12)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[3].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 2)
        {
            if (actioncount == required_count_2)
            {
                yildiz3.SetActive(false);
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[8].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_2 && actioncount < 8)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                
                pnl_star[7].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_2 && actioncount < 9)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[6].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 3)
        {
            if (actioncount == required_count_3)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[11].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_3 && actioncount < 13)
            {

                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[10].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_3 && actioncount < 14)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[9].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 4)
        {
            if (actioncount == required_count_4)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[14].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_4 && actioncount < 10)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[13].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_4 && actioncount < 11)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[12].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 5)
        {
            if (actioncount == required_count_5)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[17].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_5 && actioncount < 14)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[16].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_5 && actioncount < 15)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[15].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 6)
        {
            if (actioncount == required_count_6)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[20].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_6 && actioncount < 14)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[19].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_6 && actioncount < 15)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[18].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 7)
        {
            if (actioncount == required_count_7)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[23].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_7 && actioncount < 11)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[22].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_7 && actioncount < 12)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[21].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 8)
        {
            if (actioncount == required_count_8)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[26].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_8 && actioncount < 15)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[25].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_8 && actioncount < 16)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[24].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 9)
        {
            if ( required_count_9 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[29].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_9 && actioncount < 12)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[28].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_9 && actioncount < 13)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[27].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 10)
        {
            if (required_count_10 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[32].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_10 && actioncount < 19)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[31].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_10 && actioncount < 20)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[30].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 11)
        {
            if (required_count_11 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[35].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_11 && actioncount < 11)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[34].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_11 && actioncount < 12)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[33].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 12)
        {
            if (required_count_12 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[38].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_12 && actioncount < 16)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[37].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_12 && actioncount < 17)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[36].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 13)
        {
            if (required_count_13 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[41].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_13 && actioncount < 13)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[40].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_13 && actioncount < 14)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[39].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 14)
        {
            if (required_count_14 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[44].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_14 && actioncount < 21)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[43].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_14 && actioncount < 22)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[42].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 15)
        {
            if (required_count_15 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[47].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_15 && actioncount < 22)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[46].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_15 && actioncount < 23)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[45].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 16)
        {
            if (required_count_16 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[50].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_16 && actioncount < 20)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[49].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_16 && actioncount < 21)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[48].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 17)
        {
            if (required_count_17 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[53].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_17 && actioncount < 20)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[52].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_17 && actioncount < 21)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[51].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 18)
        {
            if (required_count_18 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[56].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_18 && actioncount < 26)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[55].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_18 && actioncount < 27)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[54].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 19)
        {
            if (required_count_19 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[59].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_19 && actioncount < 22)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[58].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_19 && actioncount < 23)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[57].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 20)
        {
            if (required_count_20 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[62].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_20 && actioncount < 11)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[61].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_20 && actioncount < 12)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[60].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 21)
        {
            if (required_count_21 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[65].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_21 && actioncount < 16)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[64].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_21 && actioncount < 17)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[63].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 22)
        {
            if (required_count_22 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[68].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_22 && actioncount < 26)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[67].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_22 && actioncount < 27)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[66].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 23)
        {
            if (required_count_23 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[71].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_23 && actioncount < 24)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[70].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_23 && actioncount < 25)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[69].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 24)
        {
            if (required_count_24 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[74].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_24 && actioncount < 19)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[73].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_24 && actioncount < 20)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[72].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 25)
        {
            if (required_count_25 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[77].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_25 && actioncount < 23)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[76].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_25 && actioncount < 24)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[75].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 26)
        {
            if (required_count_26 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[80].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_26 && actioncount < 13)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[79].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_26 && actioncount < 14)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[78].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 27)
        {
            if (required_count_27 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[83].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_27 && actioncount < 30)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[82].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_27 && actioncount < 31)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[81].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 28)
        {
            if (required_count_28 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[86].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_28 && actioncount < 21)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[85].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_28 && actioncount < 22)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[84].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        if (level == 29)
        {
            if (required_count_29 == actioncount)
            {
                kapat_yildiz();
                StartCoroutine(yil3());
                yildiz[3].Play("yildiz3_1_anim");
                yildiz[4].Play("yildiz3_2_anim");
                yildiz[5].Play("yildiz3_3_anim");
                
                pnl_star[89].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_29 && actioncount < 28)
            {
                kapat_yildiz();
                StartCoroutine(yil2());
                yildiz[1].Play("yildiz_2_1_anim");
                yildiz[2].Play("yildiz_2_2_anim");
                pnl_star[88].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
            else if (actioncount > required_count_29 && actioncount < 29)
            {
                kapat_yildiz();
                StartCoroutine(yil1());
                yildiz[0].Play("yildiz1_anim");
                pnl_star[87].SetActive(true);
                level_buttons_color[0].GetComponent<Image>().sprite = level_color.sprite;
            }
        }
        return true;
    }
    
    private IEnumerator Wait_action()
    {
        yield return new WaitForSeconds(1f);
    }

    public void directiveRemove(int place)
    {
        //Debug.Log("place " + place);
        delete_directive(place);
        /*var t = ACTION_BAR.childCount;
        for(var h = 0; h<t; h++)
        {
            Destroy(ACTION_BAR.GetChild(h).GetComponent<Button>());
            Button b = ACTION_BAR.GetChild(h).gameObject.AddComponent<Button>();
            b.onClick.AddListener(GeneratedOnClick(ACTION_BAR.GetChild(h).gameObject));
        }*/
    }

    private void Add_directive(int action)
    {
        selected_tail._next = new DIRECTIVE {action = action};
        selected_tail = selected_tail._next;
        selectedCounter++;
        Creator(action);
    }
    private void Add_directive(int action,int place)
    {
        if (place == 0)
        {
            var t = new DIRECTIVE() {action = action};
            var x = selected_directive._next;
            selected_directive._next = t;
            t._next = x;
            selectedCounter++;
            Creator(action, place);
            return;
        }
        var h = new DIRECTIVE();
        h = selected_directive;
        for (var t = 0; t < place-1; t++)
            h = h._next;
        var j = new DIRECTIVE();
        if (h._next == (null))
        {
            Add_directive(action);
            return;
        }
        h = h._next;
        j = h._next;
        h._next = new DIRECTIVE {action = action};
        h._next._next = j;
        selectedCounter++;
        Creator(action, place);
    }

    private void delete_directive(int place)
    {
        var h = new DIRECTIVE();
        h = selected_directive;
       // Debug.Log(selected_directive);
        if (selectedCounter <=1)
        {
            initalize_directive();
        }
        else if (place == 1)
        {
            selected_directive._next = selected_directive._next._next;
        }
        else
        {
            for (var t = 1; t < place; t++)
                h = h._next;
            if ((bool) h?._next.Equals(selected_tail))
            {
                h._next = null;
                selected_tail = h;
            }
            else if (h.Equals(selected_tail))
            {
                var t = selected_directive;
                while (!t._next.Equals(selected_tail))
                {
                    t = t._next;
                }
                t._next = null;
                selected_tail = t;
            }
            else
            {
                h._next = h._next._next;
            }
        }
        if (selectedCounter < 1) return;
        selectedCounter--;
        updatedirective();
    }

    private void initalize_directive()
    {
        actionBools.Clear();
        actionPlaces.Clear();
        for (var h = 0; h < SELECTED_ACTION_BAR.childCount; h++)
        {
            Destroy(SELECTED_ACTION_BAR.GetChild(h).gameObject);
        }
        getActionCount();
        selectedCounter = 0;
        pos_pointer = 0;
        dir_pointer = 1;
        if (level == 2)
        {
            dir_pointer = 7;
            pawn.transform.localEulerAngles =90*Vector3.forward;
        }
        selected_directive = new DIRECTIVE {action = 0};
        selected_directive._next = selected_directive;
        selected_tail = selected_directive;
        selected_directive.isHeader = true;
        updatedirective();
    }

    private void first_init()
    {
        for (var h = 0; h < MAIN_ACTION_BAR.childCount; h++)
        {
            Destroy(MAIN_ACTION_BAR.GetChild(h).gameObject);
        }for (var h = 0; h < p1_ACTION_BAR.childCount; h++)
        {
            Destroy(p1_ACTION_BAR.GetChild(h).gameObject);
        }for (var h = 0; h < p2_ACTION_BAR.childCount; h++)
        {
            Destroy(p2_ACTION_BAR.GetChild(h).gameObject);
        }
        MAIN_directive = new DIRECTIVE {action = 0};
        MAIN_directive._next = MAIN_directive;
        MAIN_tail = MAIN_directive;
        MAIN_tail.isHeader = true;
        P1_directive = new DIRECTIVE {action = 0};
        P1_directive._next = P1_directive;
        P1_tail = P1_directive;
        P1_tail.isHeader = true;
        P2_directive = new DIRECTIVE {action = 0};
        P2_directive._next = P2_directive;
        P2_tail = P2_directive;
        P2_tail.isHeader = true;
    }

    private void teleport_pawn()
    {
        teleport_sound_src.PlayOneShot(teleport_sound_clip);
        Anim_Walk.Stop();
        if (!_squares[pos_pointer].GetComponent<POINT_ACTION>().teleporter) return;
        Anim_Walk.teleport();
        StartCoroutine(teleport());
        
    }

    private IEnumerator teleport()
    {
        yield return new WaitForSeconds(.7f);
        pawn.transform.SetParent(_squares[_squares[pos_pointer].GetComponent<POINT_ACTION>().teleportTo].transform);
        pawn.transform.localPosition = 2.5f*Vector3.back;
        pos_pointer = _squares[pos_pointer].GetComponent<POINT_ACTION>().teleportTo;
        yield return new WaitForSeconds(.8f);
        Anim_Walk.Stop();
    }

    public void fasterRun()
    {
        Time.timeScale = 3;
        RunTest();
    }
    int count = 0;
    private void move()
    {
        //Anim_Walk.Stop();
        //Anim_Walk.RunStart();
        if (!canMove()) return;
        pos_pointer += dir_pointer;
        StartCoroutine(walking_anim());
    }

    private void jump()
    {
        //Anim_Walk.Stop();
        //Anim_Walk.JumpStart();
        if (!canJump()) return;
        pos_pointer += dir_pointer;
        
        //if (Andy.animation["walk"].time * Andy.animation["walk"].clip.frameRate >= 30)
        //{
        //    Anim_Walk.RunStop();
        //}
        StartCoroutine(jump_anim());
    }
    private IEnumerator walking_anim()
    {
        Anim_Walk.Stop();
        Anim_Walk.RunStart();
        //yürüme ses
        // yurume_sound_src.PlayOneShot(yurume_sound_clip); 
        pawn.transform.SetParent(_squares[pos_pointer].transform);
        var t = pawn.transform.localPosition;
        var c = 0f;
       // Debug.Log(t);
        while (pawn.transform.localPosition.magnitude > 2.501f)
        {
            pawn.transform.localPosition = Vector3.Lerp(t, 2.5f*Vector3.back, c += 0.05f);
            //Debug.Log("magnitude value"+ t.magnitude);
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("here");
        Anim_Walk.Stop();
        pawn.transform.localPosition = 2.5f*Vector3.back;
    }
    private IEnumerator jump_anim()
    {
        ziplama_sound_src.PlayOneShot(ziplama_sound_clip);
        pawn.transform.SetParent(_squares[pos_pointer].transform);
        if (upordown)
        {
            Anim_Walk.fall();
        }
        else
        {
            Anim_Walk.JumpStart();
        }
        var t = pawn.transform.localPosition;
        var c = 0f;
        while (t.magnitude > 0.01f)
        {
            float height = 3*(0.5f * (1f - 4 * (c - .5f) * (c - .5f)));
            pawn.transform.localPosition = Vector3.Lerp(t, 2.5f*Vector3.back, c += 0.05f) + height* Vector3.back;
            yield return new WaitForEndOfFrame();
            if (c > 1f) break;
        }
        Anim_Walk.Stop();
        pawn.transform.localPosition = 2.5f*Vector3.back;

    }

    private bool canMove()
    {
        var t = pos_pointer;
        t += dir_pointer;
        if ((t > 49 || t < 0)) return false;
        if (pos_pointer % 7 == 6 && dir_pointer == 1) return false;
        if (pos_pointer % 7 == 0 && dir_pointer == -1) return false;
        if (Mathf.Abs(_squares[pos_pointer].transform.localPosition.z - _squares[t].transform.localPosition.z) >
            0.1f) return false;
        if (!_squares[t].GetComponent<MeshRenderer>().enabled) return false;
        return t < 49 && t > -1;
    }

    private bool upordown;
    private bool canJump()
    {
        var t = pos_pointer;
        t += dir_pointer;
        if ((t > 49 || t < 0)) return false;
        if (pos_pointer % 7 == 6 && dir_pointer == 1) return false;
        if (pos_pointer % 7 == 0 && dir_pointer == -1) return false;
        if (Mathf.Abs(_squares[pos_pointer].transform.localPosition.z - _squares[t].transform.localPosition.z) <
            0.1f) return false;
        if (Mathf.Abs(_squares[pos_pointer].transform.localPosition.z - _squares[t].transform.localPosition.z) >
            50.1f) return false;
        if (!_squares[t].GetComponent<MeshRenderer>().enabled) return false;
        upordown = _squares[pos_pointer].transform.localPosition.z - _squares[t].transform.localPosition.z <0;
        return t < 49 && t > -1;
    }
    private void rotate(bool left_right)
    {
        Anim_Walk.Stop();
        switch (dir_pointer)
        {
            case 1:
                dir_pointer = left_right ? 7 : -7;
                break;
            case -1:
                dir_pointer = left_right ? -7 : 7;
                break;
            case 7:
                dir_pointer = left_right ? -1 : 1;
                break;
            case -7:
                dir_pointer = left_right ? 1 : -1;
                break;
        }
        if(turning) StopCoroutine(turning_anim);
        turning_anim = StartCoroutine(turningAnim(left_right));
    }

    private bool turning;
    private IEnumerator turningAnim(bool left_right)
    {
        turning = true;
        var c = 0f;
        var x = pawn.transform.localEulerAngles;
        var target = 0;
        if(left_right)
            target = dir_pointer == 7 ? 90 : dir_pointer == -7 ? 270 : dir_pointer == 1 ? 360 : 180;
        else 
            target = dir_pointer == 7 ? 90 : dir_pointer == -7 ? -90 : dir_pointer == 1 ? 0 : 180;
        while (Mathf.Abs(target-x.z) > 0.5f)
        {
            pawn.transform.localEulerAngles = Vector3.Lerp(x, target * Vector3.forward, c += 0.08f);
            yield return new WaitForEndOfFrame();
        }
        turning = false;
    }

    private Coroutine turn;
    private bool testUp;
    private int change_count;

    private int actioncount;
    
    
    private IEnumerator test()
    {
        testUp = true;
        
        actioncount = p1_ACTION_BAR.childCount + p2_ACTION_BAR.childCount + MAIN_ACTION_BAR.childCount;
        for(var g=0; g<actionBools.Count;g++)
        {
            actionBools[g] = false;
        }
        
        var temp_action_bar = MAIN_ACTION_BAR;//functionlari yanip söndürmek
        if(turning) StopCoroutine(turning_anim);
        turning = false;
        foreach (var VARIABLE in actionPlaces)
        {
            _squares[VARIABLE].GetComponent<POINT_ACTION>().turnOff();
        }
        var t = new DIRECTIVE();
        t = MAIN_directive;
        var counter = 0;
        while (t._next != null && t._next.action>0 && counter < 1000)
        {
            //yield return new WaitForSeconds(0.4f);//diğer komuta geçmek için bir miktar beklemesi sağlanıyor
            //Anim_Walk.Stop();                                      //            Debug.Log(t._next.action);
            counter++;
           // Debug.Log(t._next.action);
            t = t._next ?? t;
            switch (t.action)
            {
                case 0:
                    break;
                case 1:
                    rotate(true);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[0].sprite;
                    yield return new WaitForSeconds(0.2f);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[8].sprite;

                    
                    break;
                case 2:
                    rotate(false);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[1].sprite;
                    yield return new WaitForSeconds(0.2f);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[9].sprite;

                    break;
                case 3:
                    move();
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[2].sprite;
                    yield return new WaitForSeconds(0.4f);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[10].sprite;


                    break;
                case 4:
                    jump();
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[3].sprite;
                    yield return new WaitForSeconds(0.4f);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[11].sprite;
                    break;
                case 5:
                    lightUp();
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[4].sprite;
                    yield return new WaitForSeconds(0.2f);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[12].sprite;
                    yield return new WaitForSeconds(0.3f);
                    break;
                case 6:
                    teleport_pawn();
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[5].sprite;
                    yield return new WaitForSeconds(2f);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = change_color[13].sprite;
                    break;
                case 11:case 22:
                    //Anim_Walk.Stop();
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = t.action == 11 ? change_color[6].sprite : change_color[7].sprite;
                    yield return new WaitForSeconds(0.2f);
                    MAIN_ACTION_BAR.GetChild(change_count).GetComponent<Image>().sprite = t.action == 11 ? change_color[14].sprite : change_color[15].sprite;
                    var temp = new DIRECTIVE();
                    bool firstrun = true;

                    
                here:
                    var ct = 0;
                    //Debug.Log(temp.action);
                    if (firstrun) { 
                        temp = t.action==11? P1_directive : P2_directive;
                        temp_action_bar = t.action == 11 ? p1_ACTION_BAR : p2_ACTION_BAR;
                    }
                    while (temp._next != null && temp._next.action>0 && counter < 1000)
                    {
                        counter++;
                        temp = temp._next ?? temp;
                        switch (temp.action)
                        {
                            case 0:
                                break;
                            case 1:       
                                rotate(true);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[0].sprite;
                                yield return new WaitForSeconds(0.2f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[8].sprite;

                                
                                break;
                            case 2:
                                rotate(false);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[1].sprite;
                                yield return new WaitForSeconds(0.2f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[9].sprite;

                                break;

                            case 3:
                                move();
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[2].sprite;
                                yield return new WaitForSeconds(0.4f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[10].sprite;



                                break;
                            case 4:
                                jump();

                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[3].sprite;
                                yield return new WaitForSeconds(0.2f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[11].sprite;


                                break;
                            case 5:
                                lightUp();


                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[4].sprite;
                                yield return new WaitForSeconds(0.2f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[12].sprite;
                                break;
                            case 6:
                                teleport_pawn();
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[5].sprite;
                                yield return new WaitForSeconds(2f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[13].sprite;

                                break;
                            case 11:
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[6].sprite;
                                yield return new WaitForSeconds(0.2f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[14].sprite;
                                temp = P1_directive;
                                temp_action_bar = p1_ACTION_BAR;
                                firstrun = false;
                                goto here;
                                
                            case 22:
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[7].sprite;
                                yield return new WaitForSeconds(0.2f);
                                temp_action_bar.GetChild(ct).GetComponent<Image>().sprite = change_color[15].sprite;
                                temp = P2_directive;
                                temp_action_bar = p2_ACTION_BAR;
                                firstrun = false;
                                goto here;
                                break;

                        }
                        ct++;
                        if (temp.Equals(null) || checkActions()) break;
                        yield return new WaitForSeconds(0.2f);
                        
                    }
                    break;
            }
            change_count++;
            if (t.Equals(null) || checkActions()) break;
            yield return new WaitForSeconds(0.4f);
        }
        change_count = 0;
        
        Time.timeScale = 1f;
        testUp = false;
    }

    private void lightUp()
    {
        yakma_sound_src.PlayOneShot(yakma_sound_clip);
        Anim_Walk.Stop();
        if (!_squares[pos_pointer].GetComponent<POINT_ACTION>().hasAction) return;
        actionBools[actionPlaces.IndexOf(pos_pointer)] = !actionBools[actionPlaces.IndexOf(pos_pointer)];
        pawn.transform.parent.GetComponent<POINT_ACTION>().switchLight();
    }

    [FormerlySerializedAs("parentTemp")] public GameObject SELECTED_PARENT_TEMP;
    public GameObject MAIN_PARENT_TEMP;
    public GameObject P1_PARENT_TEMP;
    public GameObject P2_PARENT_TEMP;
    private GameObject temp;
    public void dragInitButton(int a)
    {
        GameObject go = Instantiate(buttonPrefab);
        var button = go.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() => GeneratedOnClick(go));
        //go.GetComponentInChildren<Text>().text = a == 1 ? "Sol" : a == 2 ? "Sağ" : a == 3 ? "Yürü" : a == 4 ? "Zıpla" : a==5? "yak":"teleport";
        go.GetComponentInChildren<Text>().text = a == 1 ? "" : a == 2 ? "" : a == 3 ? "" : a == 4 ? "" : a == 5 ? "" : a == 11 ? "" : a == 22 ? "" : "";
        go.GetComponentInChildren<Image>().sprite = a == 1 ? komutSprites[0].sprite : a == 2 ? komutSprites[1].sprite : a == 3 ? komutSprites[2].sprite : a == 4 ? komutSprites[3].sprite : a == 5 ? komutSprites[4].sprite : a == 11 ? komutSprites[5].sprite : a == 22 ? komutSprites[6].sprite : komutSprites[7].sprite;

        temp = go;
        temp.transform.SetParent(SELECTED_PARENT_TEMP.transform);
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
    }
    public void dragButton()
    {
        temp.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, temp.transform.position.z);
    }

    public void endDragButton(int a)
    {
        if (selectedButtons.Count > 11 || (selector!=0 && (selectedButtons.Count>7))) goto escape;
        var tf = false;
        var first = new Vector2(-101.4f, 80f);
        var last = new Vector2(98.7f, -80f);
        var position = temp.transform.localPosition;
        var testX = (position.x >= first.x-50 && position.x <= last.x+50);
        var testY = (position.y <= first.y+50 && position.y >= last.y-50);
       // Debug.Log("posX " + position.x +" posY " + position.y);
        if (testX && testY)
        {
            //Debug.Log(position);
            if ((position.y >(first.y -15)) && (position.x < first.x-10))
            {
                Add_directive(a, 0);
                tf = true;
                goto escape;
            }if ((position.y>(first.y -15-40)) && (position.x < first.x-10))
            {
                Add_directive(a, Mathf.Min(selectedButtons.Count,6));
                tf = true;
                goto escape;
            }if ((position.y >(first.y -15-80)) && (position.x < first.x-10))
            {
                Add_directive(a, Mathf.Min(selectedButtons.Count,12));
                tf = true;
                goto escape;
            }
            for (var t = 0; t < SELECTED_ACTION_BAR.childCount; t++)
            {
               // Debug.Log("here");
                var h = SELECTED_ACTION_BAR.GetChild(t).transform.localPosition;
                if (Mathf.Abs((position.y - h.y)) < 15f)
                {
                    if (Mathf.Abs(position.x - h.x) < 15f)
                    {
                        directiveRemove(t+1);
                        selectedButtons.Remove(SELECTED_ACTION_BAR.GetChild(t).gameObject);
                        Destroy(SELECTED_ACTION_BAR.GetChild(t).gameObject);
                        Add_directive(a, t+1);
                        tf = true;
                        break;
                    }
                    if (Mathf.Abs(position.x - h.x) < 30f)
                    {
                        Add_directive(a, t+1);
                        tf = true;
                        break;
                    }
                }
            }
            if(!tf)Add_directive(a);
            
        }
        escape:;
        Destroy(temp);

    }

    public GameObject f1_pnl_open, f2_pnl_open, f1_btn_open, f2_btn_open;

    public void F1_Panels_open()
    {
        f1_btn_open.SetActive(true);
        f1_pnl_open.SetActive(true);

    }

    public void F2_Panels_open()
    {
        f2_pnl_open.SetActive(true);
        f2_btn_open.SetActive(true);
    }

    public void F1_Panels_close()
    {
        f1_btn_open.SetActive(false);
        f1_pnl_open.SetActive(false);
    }
    public void F2_Panels_close()
    {
        f2_pnl_open.SetActive(false);
        f2_btn_open.SetActive(false);
    }


}
 
public class DIRECTIVE
{
    public int action; // 0 null, 1 rotate left, 2 rotate right, 3 move, 4 jump, 5 light the bulb, 6 teleport, 11 P1, 22 P2
    public DIRECTIVE _next;
    public bool isHeader = false;
    //public DIRECTIVE _pre;
}

