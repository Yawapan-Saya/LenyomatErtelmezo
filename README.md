# Lenyomat Értelmező

A könyvtár képes visszaállítani a lenyomatból a szoba szerkezetét

[A feladat](https://isze.hu/dusza-arpad-orszagos-programozoi-emlekverseny)

## Telepítés

1. Telepítsd a LenyomatErtelmező nevű [https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio](nuget) csomagot
2. Másold be ezt a forrásfájlod elejére: ```using LenyomatErtelmező;```

## Használat

1. Hozz létre egy bool[,] mátrixot, ami az épület lenyomatát tartalmazza
2. Hozz létre egy Alaprajz-ot a lenyomatból

## Dokumentáció

#### Alaprajz#Alaprajz

paraméter lenyomat: Az épület bool[,] típusú lenyomata
Visszatérési érték: Egy felcímkézett épületet

#### Alaprajz#Print

Visszatérési érték: Egy string, amely a felcímkézi az objectekete a feladat leírásának megfelelően

#### Alaprajz#SzobákSzáma

Visszatérési érték: A szobák száma

#### Alaprajz#Szélesség

Az épület szélessége (beleírtve a körülette lévő kertet)

#### Alaprajz#Magasság

Az épület magassága (beleírtve a körülette lévő kertet)

## Fejlesztők

- Joó András
- Südi Tamás
- Tóth Zsombor
