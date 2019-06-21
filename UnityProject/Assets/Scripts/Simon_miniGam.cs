using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simon_miniGam : MonoBehaviour
{

    public Color actualcolor;
    public List<Color> gameList = new List<Color>();
    public List<Color> Colors = new List<Color>();
    public List<Color> UserInput = new List<Color>();
    public int nbSequence = 5;
    public bool WantGame = false;
    private Random random = new Random();
    public bool isFilled = false;
    public bool canPlay = false;

    private bool turnofPlayer =false;

    public ColorInput red;
    public Transform _red;
    public ColorInput blue;
    public Transform _blue;
    public ColorInput green;
    public Transform _green;
    public ColorInput Yellow;
    public Transform _Yellow;

    public int UserStage = 0;
 

    private int stage = 1;
    private Color ActualColor;

    private bool colorDisplayed = false;


    // Start is called before the first frame update
    void Start()
    {
        Colors.Add(new Color32(255, 0, 0, 255)); //red
        Colors.Add(new Color32(255, 255, 0, 255)); //yellow
        Colors.Add(new Color32(0, 255, 0, 255)); //green
        Colors.Add(new Color32(0, 0, 255, 255)); //blue


    }
    
    void Update()
    {


        //si le joueur veut jouer, premierement remplir la list

        
        if (WantGame == true && isFilled == false)
        {
            
            for (int i = 0; i < nbSequence; i++)
            {
                int r = Random.Range(1, Colors.Count);
                gameList.Add(Colors[r]);
            }
            isFilled = true;
            
            //rempli la list de couleur aléatoire
            
        }

        //quand la list est remplie, affiche la sequence

        if (WantGame && isFilled && colorDisplayed == false)
        {

            GetComponent<SpriteRenderer>().color = Color.white;
            _red.GetComponent<SpriteRenderer>().color = Color.white;
            _Yellow.GetComponent<SpriteRenderer>().color = Color.white;
            _blue.GetComponent<SpriteRenderer>().color = Color.white;
            _green.GetComponent<SpriteRenderer>().color = Color.white;
            // affiche les case en blanche, le jeux peux commencer


            StartCoroutine(SetColor(stage));

            // la sequence est maintenant exécutée, le joueur doit jouer


            colorDisplayed = true;
        }
        
        //quand la sequence est affiché, recoit la couleur jouée et la compare a l'étape actuelle


        //si l'etapes et bonne verifie la suivante jusqua ce que la sequence soit bonne


        //sinon recommence le jeux



        //si la sequence est bonne, passe a la prochaine etapes


        //si la derniere etapes est bonne, release une clefs



    }


    IEnumerator SetColor(int sequenceStage)
    {

        _red.GetComponent<SpriteRenderer>().color = Color.white;
        _Yellow.GetComponent<SpriteRenderer>().color = Color.white;
        _blue.GetComponent<SpriteRenderer>().color = Color.white;
        _green.GetComponent<SpriteRenderer>().color = Color.white;

        yield return new WaitForSeconds(2);
        _red.GetComponent<ColorInput>().canTrigger = false;
        _Yellow.GetComponent<ColorInput>().canTrigger = false;
        _green.GetComponent<ColorInput>().canTrigger = false;
        _blue.GetComponent<ColorInput>().canTrigger = false;

        yield return new WaitForSeconds(2);

            for (int i = 0; i < sequenceStage; i++)
            {

                //affiche la suite de couleurs
                actualcolor = gameList[i];
                yield return new WaitForSeconds(1);

            if (actualcolor == Color.red)
            {
                _red.GetComponent<SpriteRenderer>().color = actualcolor;
                yield return new WaitForSeconds(1);
                _red.GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (actualcolor == Color.yellow)
            {
                _Yellow.GetComponent<SpriteRenderer>().color = actualcolor;
                yield return new WaitForSeconds(1);
                _Yellow.GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (actualcolor == Color.blue)
            {
                _blue.GetComponent<SpriteRenderer>().color = actualcolor;
                yield return new WaitForSeconds(1);
                _blue.GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (actualcolor == Color.green)
            {
                _green.GetComponent<SpriteRenderer>().color = actualcolor;
                yield return new WaitForSeconds(1);
                _green.GetComponent<SpriteRenderer>().color = Color.white;
            }


        }
        yield return new WaitForSeconds(0.5f);
        _red.GetComponent<SpriteRenderer>().color = Color.cyan;
        _Yellow.GetComponent<SpriteRenderer>().color = Color.cyan;
        _green.GetComponent<SpriteRenderer>().color = Color.cyan;
        _blue.GetComponent<SpriteRenderer>().color = Color.cyan;

        yield return new WaitForSeconds(0.5f);
        _red.GetComponent<SpriteRenderer>().color = Color.white;
        _Yellow.GetComponent<SpriteRenderer>().color = Color.white;
        _green.GetComponent<SpriteRenderer>().color = Color.white;
        _blue.GetComponent<SpriteRenderer>().color = Color.white;

        yield return new WaitForSeconds(0.5f);
        _red.GetComponent<SpriteRenderer>().color = Color.cyan;
        _Yellow.GetComponent<SpriteRenderer>().color = Color.cyan;
        _green.GetComponent<SpriteRenderer>().color = Color.cyan;
        _blue.GetComponent<SpriteRenderer>().color = Color.cyan;

        yield return new WaitForSeconds(0.5f);
        _red.GetComponent<SpriteRenderer>().color = Color.cyan;
        _Yellow.GetComponent<SpriteRenderer>().color = Color.cyan;
        _green.GetComponent<SpriteRenderer>().color = Color.cyan;
        _blue.GetComponent<SpriteRenderer>().color = Color.cyan;

        //display initial color 
        _red.GetComponent<SpriteRenderer>().color = _red.GetComponent<ColorInput>().initialColor;
        _Yellow.GetComponent<SpriteRenderer>().color = _Yellow.GetComponent<ColorInput>().initialColor;
        _green.GetComponent<SpriteRenderer>().color = _green.GetComponent<ColorInput>().initialColor;
        _blue.GetComponent<SpriteRenderer>().color = _blue.GetComponent<ColorInput>().initialColor;


        _red.GetComponent<ColorInput>().canTrigger = true;
        _Yellow.GetComponent<ColorInput>().canTrigger = true;
        _green.GetComponent<ColorInput>().canTrigger = true;
        _blue.GetComponent<ColorInput>().canTrigger = true;


   


    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        WantGame = true;
    }

    public void PlayerStep(Color color)
    {
        if (UserStage < stage)
        {
            try
            {
                UserInput.Add(color);
            }
            catch (System.Exception e)
            {

                Debug.Log(e);
            }
            
            if (UserInput[UserStage] == gameList[UserStage])
            {
                UserStage++;
                if (UserStage >= gameList.Count)
                {
                    Debug.Log("Gagné");
                }
            }

        }
        if (UserInput.Count == stage)
        {
            UserInput = null;
        }
        stage++;
        UserStage = 0;
        
        colorDisplayed = false;





    }
}



