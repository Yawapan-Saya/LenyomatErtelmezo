using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class Magic : MonoBehaviour
{
    int y;
    int x;

    string path = "";
    string alaprajz01 = "assets/Alaprajzok/alaprajz01.txt", alaprajz02 = "assets/Alaprajzok/alaprajz02.txt", alaprajz03 = "assets/Alaprajzok/alaprajz03.txt", alaprajz04 = "assets/Alaprajzok/alaprajz04.txt",
        alaprajz05 = "assets/Alaprajzok/alaprajz05.txt", alaprajz06 = "assets/Alaprajzok/alaprajz06.txt", alaprajz07 = "assets/Alaprajzok/alaprajz07.txt", alaprajz08 = "assets/Alaprajzok/alaprajz08.txt",
        alaprajz09 = "assets/Alaprajzok/alaprajz09.txt";

    public Toggle tg01, tg02, tg03, tg04, tg05, tg06, tg07, tg08, tg09;

    public HelyreRak fal;
    public HelyreRak asztal;
    public HelyreRak szek;
    public HelyreRak kanape;
    public HelyreRak padlo;

    public GameObject huff;

    GameObject kla;

    string[,,] szoba;

    Tarolo vlist = new Tarolo();

    class Tarolo
    {
        public List<HelyreRak> asztalok = new List<HelyreRak>();
        public List<HelyreRak> szekek = new List<HelyreRak>();
        public List<HelyreRak> kanapek = new List<HelyreRak>();
        public List<HelyreRak> falak = new List<HelyreRak>();
        public List<HelyreRak> padlok = new List<HelyreRak>();
    }

    public void DoIt()
    {
        if (tg01.isOn)
        {
            path = alaprajz01;
        }
        else if (tg02.isOn)
        {
            path = alaprajz02;
        }
        else if (tg03.isOn)
        {
            path = alaprajz03;
        }
        else if (tg04.isOn)
        {
            path = alaprajz04;
        }
        else if (tg05.isOn)
        {
            path = alaprajz05;
        }
        else if (tg06.isOn)
        {
            path = alaprajz06;
        }
        else if (tg07.isOn)
        {
            path = alaprajz07;
        }
        else if (tg08.isOn)
        {
            path = alaprajz08;
        }
        else if (tg09.isOn)
        {
            path = alaprajz09;
        }


        //Innentől kezdve ne nyúlj hozzá!█████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
        StreamReader olv = new StreamReader(path);
        string[] sor = olv.ReadLine().Split(' ');
         y = Convert.ToInt32(sor[0]);
         x = Convert.ToInt32(sor[1]);
        szoba = new string[y, x, 2];
        // Tömb feltöltése csak az 1. réteg
        int k = 0;
        while (!olv.EndOfStream)
        {
            string sor2 = olv.ReadLine();
            for (int i = 0; i < x; i++)
            {
                szoba[k, i, 0] = sor2[i].ToString();
                if (sor2[i] == '1')
                {
                    szoba[k, i, 1] = "U";
                }
                else
                {
                    szoba[k, i, 1] = ".";
                }
            }
            k++;
        }

        bool vanfal = false;

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                //első faldarab megtalálása, majd falak megkeresése
                if (szoba[i, j, 0] == "1" && !vanfal)
                {
                    szoba[i, j, 1] = "F";
                    Falkereső(i, j, szoba);
                    vanfal = true;
                }
                //Székek keresése
                if (szoba[i, j, 1] == "U" && !Szomszéd(i, j, szoba, "U") && !Szomszéd(i, j, szoba, "F") && !Szomszéd(i, j, szoba, "A") && !Szomszéd(i, j, szoba, "K"))
                {
                    szoba[i, j, 1] = "S";
                }
                //Asztal keresése
                if (szoba[i, j, 1] == "U" && i < szoba.GetLength(0) - 1 && j < szoba.GetLength(1) - 1)
                {
                    if (szoba[i + 1, j, 1] == "U" && szoba[i, j + 1, 1] == "U" && szoba[i + 1, j + 1, 1] == "U" || Szomszéd(i, j, szoba, "A"))
                    {
                        szoba[i, j, 1] = "A";
                    }
                }
                //Kanapé keresése
                if ((szoba[i, j, 1] == "U" && !Szomszéd(i, j, szoba, "F")) || (szoba[i, j, 1] == "U" && Szomszéd(i, j, szoba, "K")))
                {
                    szoba[i, j, 1] = "K";
                }
            }
        }
        //És egészen idáig ne piszkáld!███████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

        Debug.Log(y);
        Debug.Log(x);
        //Kiírás

        //-------------------------------------------------------------------------------------------
        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //plane.transform.Rotate(-90, 0, 0);
        //plane.transform.localScale = new Vector3(x, y, z);       
        //-----------------------------------------------------------------------------------------------------

        //for (int i = 0; i < y; i++)
        //{
        //    for (int j = 0; j < x; j++)
        //    {

        //        if (szoba[i,j,1] == "F")
        //        {
        //            StartCoroutine(Visszatartás(10));
        //            Instantiate(fal, new Vector2(j,i), Quaternion.identity);
        //        }
        //        else if (szoba[i, j, 1] == "A")
        //        {
        //            StartCoroutine(Visszatartás(10));
        //            Instantiate(asztal, new Vector2(j,i), Quaternion.identity);

        //        }
        //        else if (szoba[i, j, 1] == "S")
        //        {
        //            StartCoroutine(Visszatartás(10));
        //            Instantiate(szek, new Vector2(j,i), Quaternion.identity);

        //        }
        //        else if (szoba[i, j, 1] == "K")
        //        {
        //            StartCoroutine(Visszatartás(10));
        //            Instantiate(kanape, new Vector2(j, i), Quaternion.identity);

        //        }
        //        StartCoroutine(Visszatartás(10));
        //        Instantiate(padlo, new Vector2(j, i), Quaternion.identity);

        //    }
        //    Debug.Log('\n');
        //}
        //Debug.Log("ok");






        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (szoba[i, j, 1] == "F")
                {
                    HelyreRak f =  Instantiate(fal, new Vector3(j, 2,i), Quaternion.identity);
                    vlist.falak.Add(f);
                }
            }
        }

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (szoba[i, j, 1] == "A")
                {
                    HelyreRak a = Instantiate(asztal, new Vector3(j, 2, i), Quaternion.identity);
                    vlist.asztalok.Add(a);
                }
            }
        }

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                 if (szoba[i, j, 1] == "S")
                 {
                    HelyreRak sz = Instantiate(szek, new Vector3(j, 2, i), Quaternion.identity);
                    vlist.szekek.Add(sz);

                 }
            }
        }

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (szoba[i, j, 1] == "K")
                {
                    HelyreRak ka = Instantiate(kanape, new Vector3(j, 2, i), Quaternion.identity);
                    vlist.kanapek.Add(ka);
                }
            }
        }

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                HelyreRak p = Instantiate(padlo, new Vector3(j, 1.9f, i), Quaternion.identity);
                vlist.padlok.Add(p);
            }
        }

        

    }

    static bool Szomszéd(int y, int x, string[,,] t, string keresettSzomszéd)
    {
        if (y > 0)
        {
            if (t[y - 1, x, 1] == keresettSzomszéd)
            {
                return true;
            }
        }
        // 9 < 11-1
        if (y < t.GetLength(0) - 1)
        {
            if (t[y + 1, x, 1] == keresettSzomszéd)
            {
                return true;
            }
        }

        if (x > 0)
        {
            if (t[y, x - 1, 1] == keresettSzomszéd)
            {
                return true;
            }

        }
        if (x < t.GetLength(1) - 1)
        {
            if (t[y, x + 1, 1] == keresettSzomszéd)
            {
                return true;
            }
        }

        return false;
    }

    static void Falkereső(int y, int x, string[,,] t)
    {
        if (t[y, x, 1] == "F")
        {
            if (y < t.GetLength(0) - 1 && t[y + 1, x, 1] == "U")
            {
                t[y + 1, x, 1] = "F";
                Falkereső(y + 1, x, t);
            }
            if (x < t.GetLength(1) - 1 && t[y, x + 1, 1] == "U")
            {
                t[y, x + 1, 1] = "F";
                Falkereső(y, x + 1, t);
            }
            if (y > 0 && t[y - 1, x, 1] == "U")
            {
                t[y - 1, x, 1] = "F";
                Falkereső(y - 1, x, t);
            }
            if (x > 0 && t[y, x - 1, 1] == "U")
            {
                t[y, x - 1, 1] = "F";
                Falkereső(y, x - 1, t);
            }
        }
        return;

        
    }

    //HelyreRak Script Megoldja ehelyett egyszerübben

    //IEnumerator Visszatartás(float wait, GameObject o)
    //{
    //    Debug.Log("egy");
    //    for (int i = 0; i < wait/10; i++)
    //    {
    //        yield return new WaitForSecondsRealtime(0.1f);
    //        o.transform.position = o.transform.position - Vector3.up * 1.0f;
    //        Debug.Log("x");
    //    }
    //    Debug.Log("kettő");
    //}

    public void DestroyAll()
    {
        foreach (var item in vlist.asztalok)
        {
            Destroy(item.gameObject);
        }
        vlist.asztalok.Clear();
        foreach (var item in vlist.falak)
        {
            Destroy(item.gameObject);
        }
        vlist.falak.Clear();
        foreach (var item in vlist.kanapek)
        {
            Destroy(item.gameObject);
        }
        vlist.kanapek.Clear();
        foreach (var item in vlist.padlok)
        {
            Destroy(item.gameObject);
        }
        vlist.padlok.Clear();
        foreach (var item in vlist.szekek)
        {
            Destroy(item.gameObject);
        }
        vlist.szekek.Clear();
    }

    public void MasikScript()
    {
        foreach (var item in vlist.falak)
        {
            if (!item.vegzettE())
            {
                item.mozgat();
                break;
            }
        }
    }

    
    public void Update()
    {
        //Innen kell javítani
        if (true)
        {
            foreach (var item in vlist.falak)
            {
                if (!item.vegzettE())
                {
                    item.mozgat();
                    break;
                }
            }
        }

        foreach (var item in vlist.szekek)
        {
            if (!item.vegzettE())
            {
                item.mozgat();
                break;
            }
        }
        

        foreach (var item in vlist.kanapek)
        {
            if (!item.vegzettE())
            {
                item.mozgat();
                break;
            }
        }
        foreach (var item in vlist.asztalok)
        {
            if (!item.vegzettE())
            {
                item.mozgat();
                break;
            }
        }
        foreach (var item in vlist.padlok)
        {
            if (!item.vegzettE())
            {
                item.mozgat();
                break;
            }
        }
    }
}

