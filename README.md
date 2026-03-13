# Oprava-Disku-RP
Jednoduchý nástroj v C#, který jsem vytvořil, aby mi pomohl vyčistit disk od balastu, který se tam časem nahromadí. Tento projekt vznikl jako ročníková práce na SPŠ Třebíč.
Proč tenhle nástroj?
Všichni to známe – složky Temp, staré logy, stažené soubory, které už nikdo nepotřebuje. Windows sice nějaké nástroje má, ale mně nikdy nevyhovovaly tím, jak jsou nepřehledné. Chtěl jsem něco, co si nastavím podle sebe, spustím a uvidím přesně, co se maže a kam to mizí.
Co to umí?
Rekurzivní sken: Proleze celou složku do hloubky, na jakou mu řekneš.
Filtry, co dávají smysl:
Hledání podle přípony (masky).
Omezení velikosti (aby to nemazalo důležité malé soubory).
Výběr data v kalendáři (sleduje, kdy byl soubor naposledy upraven).
Bezpečí především: Pokud program narazí na systémovou složku, kam nemá přístup, prostě ji přeskočí a hodí o tom záznam do logu – nepadá, neshazuje systém.
Logování: Všechno, co aplikace udělá, najdeš v přehledném log_cisteni.txt přímo na své ploše.
Moderní vzhled: Žádné šedé okno z devadesátek, ale vlastní tmavý režim.
Jak s tím pracovat?
Stáhni si .exe (nebo zkompiluj z kódu).
Vyber složku, kterou chceš "uklidit".
Nastav si filtry (co hledat, jak velké, jak staré).
Pozor: Pokud chceš opravdu mazat, musíš zaškrtnout checkbox "Smazat nalezené". Jinak aplikace jen vypíše seznam souborů, které by smazat mohla – je to bezpečnější pro první testování.
Klikni na "SPUSTIT" a sleduj, jak se uvolňuje místo.
Technologie
Programováno v C# (WinForms) ve Visual Studiu. Žádné složité knihovny, všechno jede na standardním .NET frameworku, takže to nezabírá skoro žádné místo a běží to okamžitě.
Pár poznámek na závěr
Aplikace je navržená tak, aby byla "blbuvzdorná". Pokud máš strach, že smažeš něco důležitého, nejdřív to zkus bez zaškrtnutého checkboxu pro mazání. Uvidíš seznam souborů v tabulce, zkontroluješ si je a teprve potom můžeš povolit ostré čištění.
