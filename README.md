# Corinth Hackathon 2022 - TTS VR Unity Project


Stáhnout a naimportovat (Assets > Import Package > Custom Package) do
projektu .unitypackage Azure Speech SDK. https://aka.ms/csspeech/unitypackage


Tento repozitář aktuálně obsahuje základ projektu, který by Vám měl ušetřit práci s instalací SDK balíčků pro vývoj na VR brýlích a s jejich nastavením.
Během Hackatonu bude tento projekt rozšiřován o základní assety, další nástroje a případné opravy chyb.

Pokud se chcete na Hackatonu věnovat virtuální realitě na Pico all-in-one VR brýlích (platforma Android), věnujte pozornost tomuto projektu již nyní.  
Pokud se chcete zaměřit jen na Holobox (PC s Windows), můžete tenhle projekt zatím ignorovat.

Použití tohoto projektu není povinné, pokud máte vlastní projekt pravděpodobně nebudete chtít přejít na náš, přesto bychom doporučovali tento stáhnout a zkusit přeložit.

## Postup zprovoznění projektu pro Pico VR:
1. Ujistěte se, že máte nainstalované Unity3D verzi 2020.3.29f1 (poslední LTS), včetně podpory pro build na Android, Android SDK a OpenJDK.

2. Projekt nejlépe naklonujte přes Váš oblíbený Git-klient, nebo alespoň stáhněte jako .zip.

3. Otevřete projekt v Unity3D (proběhne import).

4. V okně Build Settings (File -> Build Settings...), přepněte cílovou platformu na Android (znovu proběhne import).  
   
   ![platform](https://user-images.githubusercontent.com/100353389/155550717-2de63b18-d469-45a8-a2f8-7f2cb2af1f13.png)

5. Připravte si svojí ikonu aplikace a vyberte ji v Player Setting (Edit -> Project Settings... -> Player -> Icon), ať si na hackathonu poznáte svou aplikaci od ostatních.  
   
   ![icon](https://user-images.githubusercontent.com/100353389/155550774-d73c6681-14cf-4ec4-9dc7-884e1ae7f97c.png)

6. Ve Build Settings se následně pokuste o Build projektu (potřeba vytvořit cílovou složku a zadat jméno .apk souboru). Měl by vzniknout balíček s aplikací pro Pico VR brýle (na Vašem mobilu Vám toto .apk určitě nepůjde).  
   
   ![build](https://user-images.githubusercontent.com/100353389/155550869-25508e93-5380-4660-b88f-04f7267da82a.png)

7. Pokud Vám build neproběhl (nevzniklo .apk), pokuste se problém vyřešit (často je problém s kolizí OpenJDK s jiným instalovaným Java SDK – nastavení PATH v systému), v nejhorším případě zkuste i reinstalaci Unity.

8. Pokud se Vám přesto nepodaří udělat build, dejte vědět, pokusíme se Vám na úvod Hackatonu pomoci.

9. Na začátku Hackatonu aplikaci nainstalujeme a spustíme na VR brýlích.

## VR interakce
Projekt obsahuje ukázku základních interakcí ve VR (uchopování předmětů, interakce s UI). Pokud by Vám tyto základní interakce nestačily, doporučujeme prozkoumat [tento repozitář](https://github.com/Unity-Technologies/XR-Interaction-Toolkit-Examples), který demonstruje další možnosti XR Interaction Toolkitu. Zájemcům pomůžeme s jeho zprovozněním na Pico VR brýlích nebo poskytneme hotový Unity projekt.

