using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    List<Local> allLocals = new List<Local>();
    List<Suspect> allSuspects = new List<Suspect>();
    List<Weapon> allWeapons = new List<Weapon>();
    List<Photo> allPhotos = new List<Photo>();
    List<GameObject> inBoard = new List<GameObject>();

    bool isLocalReal;
    bool isWeaponReal;
    bool isSuspectReal;

    Local crimeLocal;
    Suspect crimeSuspect;
    Weapon crimeWeapon;

    public Canvas Canvas;
    public GameObject PhotoPrefab;

    public TextMeshProUGUI IsNotReal;
    public TextMeshProUGUI Moves;
    public TextMeshProUGUI BestMoves;
    public TextMeshProUGUI wasBestResult;
    public TextMeshProUGUI howManyMoves;

    public GameObject PlayGameOverUI;
    public Button PlayButton;
    public Button HowToPlayButton;
    public Button PlayFromHow;
    public Color ItsRealColor;
    public GameObject GoToMenuUI;
    public Button RestartButton;


    public Image LocalPhotoUI;
    public Image SuspectPhotoUI;
    public Image WeaponPhotoUI;

    int moves = 0;

    AudioSource audioMain;
    public AudioClip GameWinSound;
    public AudioClip Descarte;
    public AudioClip ItsReal;
    public AudioClip ItsNotReal;

    public GameObject BackToEndIcon;
    public GameObject HowToPlayUI;

    // Start is called before the first frame update
    void Start()
    {
        audioMain = gameObject.GetComponent<AudioSource>();

        Button btnHowToPlay = HowToPlayButton.GetComponent<Button>();
        btnHowToPlay.onClick.AddListener(HowToPlay);

        Button btnPlay = PlayButton.GetComponent<Button>();
        btnPlay.onClick.AddListener(PlayGame);

        IsNotReal.gameObject.LeanAlpha(0, 0);

        int movesSaved = PlayerPrefs.GetInt("bestMoves");

        if (movesSaved != 0) BestMoves.text = "your best solution was at " + movesSaved + " moves";
    }

    void HowToPlay()
    {
        Button btnPlay = PlayFromHow.GetComponent<Button>();
        btnPlay.onClick.AddListener(PlayGame);

        HowToPlayUI.SetActive(true);
    }

    void PlayGame()
    {
        HowToPlayUI.SetActive(false);
        PlayGameOverUI.gameObject.SetActive(false);
        BackToEndIcon.SetActive(true);
        SetupNewInvestigation();
        ListPhotos();
    }

    void GameWin()
    {
        BackToEndIcon.SetActive(false);
        audioMain.PlayOneShot(GameWinSound);

        TextMeshProUGUI localTitle = LocalPhotoUI.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI suspectTitle = SuspectPhotoUI.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI weaponTitle = WeaponPhotoUI.GetComponentInChildren<TextMeshProUGUI>();
        Image localImage = LocalPhotoUI.GetComponent<Image>();
        Image suspectImage = SuspectPhotoUI.GetComponent<Image>();
        Image weaponImage = WeaponPhotoUI.GetComponent<Image>();

        localImage.sprite = crimeLocal.GetSprite();
        suspectImage.sprite = crimeSuspect.GetSprite();
        weaponImage.sprite = crimeWeapon.GetSprite();

        localTitle.text = crimeLocal.Name.ToString();
        suspectTitle.text = crimeSuspect.Name.ToString();
        weaponTitle.text = crimeWeapon.Name.ToString();

        var allPhotosOnCanvas = GameObject.FindGameObjectsWithTag("Photo");

        foreach (GameObject child in allPhotosOnCanvas)
        {
            if (child == PhotoPrefab) continue;

            Destroy(child);
        }


        Button btn = RestartButton.GetComponent<Button>();
        btn.onClick.AddListener(RestartGame);
        GoToMenuUI.gameObject.SetActive(true);
        GoToMenuUI.transform.SetAsLastSibling();

        howManyMoves.text = "you did " + this.moves + " moves";
        int movesSaved = PlayerPrefs.GetInt("bestMoves");

        if (movesSaved == 0 || this.moves < movesSaved)
        {
            wasBestResult.gameObject.SetActive(true);
            PlayerPrefs.SetInt("bestMoves", this.moves);
            PlayerPrefs.Save();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PhotoMovedToBoard (Photo photo, GameObject photoGameObject)
    {
        if (inBoard.Exists(e => e.gameObject == photoGameObject)) return;

        moves++;
        Moves.text = "moves: " + moves;

        inBoard.Add(photoGameObject);
        bool isReal = IsReal(photo);

        if (isReal)
        {
            audioMain.PlayOneShot(ItsReal);
            IsNotReal.text = "It is real.";
            LeanTween.cancel(photoGameObject);

            if (photo.Style == Photo.Styles.LOCAL) isLocalReal = true;
            if (photo.Style == Photo.Styles.WEAPON) isWeaponReal = true;
            if (photo.Style == Photo.Styles.SUSPECT) isSuspectReal = true;
        } else
        {
            audioMain.PlayOneShot(ItsNotReal);
            IsNotReal.text = "It is not real.";
        }

        RemovePhotoFromList(photo);

        if (isLocalReal && isWeaponReal && isSuspectReal)
        {
            GameWin();
        }
    }

    bool IsReal(Photo photo)
    {
        if (photo.Name == crimeLocal.Name.ToString()
            || photo.Name == crimeSuspect.Name.ToString()
            || photo.Name == crimeWeapon.Name.ToString()
        ) {
            return true;
        }

        return false;
    }

    void RemovePhotoFromList(Photo photo)
    {
        allPhotos.Remove(photo);
        ListPhotos();
    }

    public void PhotoMovedToEnd(Photo photo, GameObject photoGameObject)
    {
        audioMain.PlayOneShot(Descarte);
        inBoard.Remove(photoGameObject);
        allPhotos.Remove(photo);
        allPhotos.Add(photo);
        ListPhotos();
    }

    void ListPhotos()
    {
        var allPhotosOnCanvas = GameObject.FindGameObjectsWithTag("Photo");
        var list3 = allPhotosOnCanvas.Except(inBoard).ToList();

        foreach (GameObject child in list3)
        {
            if (child == PhotoPrefab) continue;

            Destroy(child);
        }

        for (int i = 0; allPhotos.Count > i; i++)
        {
            if (i > 8) break;

            GameObject instantiatedPhoto = Instantiate(PhotoPrefab, Canvas.transform);
            instantiatedPhoto.SetActive(true);
            RectTransform UIPhoto = instantiatedPhoto.GetComponent<RectTransform>();

            UIPhoto.anchoredPosition = new Vector2((i * 125f) - 500f, -320f);
            instantiatedPhoto.GetComponent<PhotoController>().Photo = allPhotos[i];

            Image image = instantiatedPhoto.GetComponent<Image>();
            image.sprite = allPhotos[i].Image;

            UIPhoto.Find("Title").GetComponent<TextMeshProUGUI>().text = allPhotos[i].Name;
            UIPhoto.Find("Description").GetComponent<TextMeshProUGUI>().text = allPhotos[i].Description;

            if (i == 8) instantiatedPhoto.LeanScale(new Vector2(1.5f, 1.5f), 1f).setEasePunch();
        }
    }

    Photo GetPhoto()
    {
        Photo photoToReturn = allPhotos[0];
        allPhotos.RemoveAt(0);

        return photoToReturn;
    }

    void SetupNewInvestigation()
    {
        /**
        * Locals
        */
        allLocals.Add(new Local(Local.Names.BEDROOM, Weapon.Types.SUFFOCATING));
        allLocals.Add(new Local(Local.Names.GARDEN, Weapon.Types.NATURE));
        allLocals.Add(new Local(Local.Names.KITCHEN, Weapon.Types.SLASHING));
        allLocals.Add(new Local(Local.Names.REST_ROOM, Weapon.Types.CONCUSSION));

        // Suspects
        allSuspects.Add(new Suspect(
            Suspect.Names.CALDEIRA,
            new Local.Names[] { Local.Names.BEDROOM, Local.Names.KITCHEN } // vermelho - azul
        ));
        allSuspects.Add(new Suspect(
            Suspect.Names.CASTRO,
            new Local.Names[] { Local.Names.KITCHEN, Local.Names.REST_ROOM } // azul - amarelo
        ));
        allSuspects.Add(new Suspect(
            Suspect.Names.GREGOR,
            new Local.Names[] { Local.Names.REST_ROOM, Local.Names.GARDEN} // amarelo - roxo
        ));
        allSuspects.Add(new Suspect(
            Suspect.Names.SALLES,
            new Local.Names[] { Local.Names.GARDEN, Local.Names.BEDROOM } // roxo - vermelho
        ));

        allSuspects.Add(new Suspect(
            Suspect.Names.BAUER,
            new Local.Names[] { Local.Names.GARDEN, Local.Names.KITCHEN } // roxo - azul
        ));
        allSuspects.Add(new Suspect(
            Suspect.Names.IGOR,
            new Local.Names[] { Local.Names.BEDROOM, Local.Names.REST_ROOM } // vermelho - amarelo
        ));

        allSuspects.Add(new Suspect(
            Suspect.Names.JULIANO,
            new Local.Names[] { Local.Names.KITCHEN, Local.Names.GARDEN } // azul - roxo
        ));
        allSuspects.Add(new Suspect(
            Suspect.Names.THIAGO,
            new Local.Names[] { Local.Names.REST_ROOM, Local.Names.BEDROOM } // amarelo - vermelho
        ));

        /**
         * Weapons
         * 
         */
        // REST_ROOM 
        allWeapons.Add(new Weapon(
            Weapon.Names.WRENCH,
            Weapon.Types.CONCUSSION
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.BOOK,
            Weapon.Types.CONCUSSION
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.FRAME,
            Weapon.Types.CONCUSSION
        ));
        allWeapons.Add(new Weapon(
           Weapon.Names.HAMMER,
           Weapon.Types.CONCUSSION
       ));

        // BEDROOM 
        allWeapons.Add(new Weapon(
            Weapon.Names.BLANKET,
            Weapon.Types.SUFFOCATING
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.TOWEL,
            Weapon.Types.SUFFOCATING
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.CAR,
            Weapon.Types.SUFFOCATING
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.PILLOW,
            Weapon.Types.SUFFOCATING
        ));

        // KITCHEN
        allWeapons.Add(new Weapon(
            Weapon.Names.SCISSORS,
            Weapon.Types.SLASHING
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.BROKEN_GLASS,
            Weapon.Types.SLASHING
        ));
        allWeapons.Add(new Weapon(
           Weapon.Names.KNIFE,
           Weapon.Types.SLASHING
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.BEDSIDE_LAMP,
            Weapon.Types.SLASHING
        ));

        // GARDEN
        allWeapons.Add(new Weapon(
            Weapon.Names.FERTILIZER,
            Weapon.Types.NATURE
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.FLOWER,
            Weapon.Types.NATURE
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.TREE_BRANCH,
            Weapon.Types.NATURE
        ));
        allWeapons.Add(new Weapon(
            Weapon.Names.AXE,
            Weapon.Types.NATURE
        ));


        // Setup photos
        foreach (Local local in allLocals)
        {
            allPhotos.Add(new Photo(
                local.Name.ToString(),
                "Weapons of type [" + local.WeaponTypeUsedHere.ToString() + "] can be used here.",
                Photo.Styles.LOCAL,
                local.GetSprite()
            ));
        }

        foreach (Suspect suspect in allSuspects)
        {
            allPhotos.Add(new Photo(
                suspect.Name.ToString(),
                "I've seen this one near of the [" + suspect.seenIn[0].ToString() + "] and [" + suspect.seenIn[1].ToString() + "].",
                Photo.Styles.SUSPECT,
                suspect.GetSprite()
            ));
        }

        foreach (Weapon weapon in allWeapons)
        {
            allPhotos.Add(new Photo(
                weapon.Name.ToString(),
                "Is a [" + weapon.Type.ToString() + "] weapon.",
                Photo.Styles.WEAPON,
                weapon.GetSprite()
            ));
        }

        GetCrimeInfo();
    }

    void GetCrimeInfo()
    {
        System.Random rng = new System.Random();
        allPhotos = allPhotos.OrderBy(a => rng.Next()).ToList();

        // Crime Local
        crimeLocal = allLocals[Random.Range(0, allLocals.Count)];

        // Crime Suspect
        List<Suspect> suspectFiltred = allSuspects.FindAll(
            suspect => Array.Exists(
                suspect.seenIn,
                local => local == crimeLocal.Name
            )
        );

        crimeSuspect = suspectFiltred[Random.Range(0, suspectFiltred.Count)];

        // Crime Weapon
        List<Weapon> weaponFiltred = allWeapons.FindAll(
            weapon =>
                weapon.Type == crimeLocal.WeaponTypeUsedHere
        );

        crimeWeapon = weaponFiltred[Random.Range(0, weaponFiltred.Count)];
    }
}

