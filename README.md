# Lenyomat Értelmező

A könyvtár képes visszaállítani a lenyomatból a szoba szerkezetét

[A feladat](https://isze.hu/dusza-arpad-orszagos-programozoi-emlekverseny)

## Telepítés

1. Telepítsd a LenyomatErtelmező nevű [nuget](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio) csomagot
2. Másold be ezt a forrásfájlod elejére: ```using LenyomatErtelmező;```

## Használat

1. Hozz létre egy bool[,] mátrixot, ami az épület lenyomatát tartalmazza
2. Hozz létre egy Alaprajz-ot a lenyomatból

## Dokumentáció

### Alaprajz#Alaprajz

**paraméter lenyomat:** Az épület bool[,] típusú lenyomata

**Visszatérési érték:** Egy felcímkézett épületet

### Alaprajz#Print

**Visszatérési érték:** Egy string, amely a felcímkézi az objectekete a feladat leírásának megfelelően

### Alaprajz#SzobákSzáma

**Visszatérési érték:** A szobák száma

### Alaprajz#Szélesség

Az épület szélessége (beleértve a körülette lévő kertet)

### Alaprajz#Magasság

Az épület magassága (beleértve a körülette lévő kertet)


## Unity - Megoldás vizualizálása

|Script| mappában találhatóak a használt scriptek

|Alaprajz| mappában keresendőek  a felhasznált alaprajzok / "szobák"

|Futtatás| mappában pedig a 3D-s vizualizációs alkalmazásnak a(z) .exe fájlja található
  Ahoz, hogy a(z) .exe működjön le kell tölteni az egész "Futtatás" mappát hozzá!

## Fejlesztők

- Joó András
- Südi Tamás
- Tóth Zsombor
