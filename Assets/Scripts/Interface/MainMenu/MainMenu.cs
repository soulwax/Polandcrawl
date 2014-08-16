using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GUISkin skin;		

    private Rect boxRect;		
    private string show;		
    private string dungeonSize;	
    private string dungeonType;

    public Texture2D entity1;
    public Texture2D entity2;


    void Start()
    {
        //if the player is in the scene destroy him
        //this can occur if you are going from the dungeon and back to the menu, because of the DontDestroyOnLoad function
        if (GameObject.Find("Player"))
        {
            Destroy(GameObject.Find("Player"));
        }
        //if the dungeon variables gameobject is in the scene destroy it
        if (GameObject.Find("DungeonVariables"))
        {
            Destroy(GameObject.Find("DungeonVariables"));
        }
        //if the game manager is in the scene destroy it
        if (GameObject.Find("Manager"))
        {
            Destroy(GameObject.Find("Manager"));
        }


        boxRect = new Rect(0, 0, 200, 270);
        show = "Main Menu";

        dungeonSize = "Medium";
        dungeonType = "A";
    }

    void OnGUI()
    {
        if (skin != null)
        {
            GUI.skin = skin;
            skin.button.normal.textColor = Color.black;
            skin.label.alignment = TextAnchor.UpperCenter;
        }

        GUI.BeginGroup(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 250, 200, 500));
        GUI.Box(boxRect, "");

        if (show == "Main Menu")
        {
            //play button
            if (GUI.Button(new Rect(40, 75, 120, 30), "Play"))
            {
                boxRect = new Rect(0, 0, 200, 390);
                show = "Play";
            }
            //help button
            if (GUI.Button(new Rect(40, 100, 120, 30), "Help"))
            {
                boxRect = new Rect(0, 0, 200, 350);
                show = "Help";
            }
            //website button
            if (GUI.Button(new Rect(40, 145, 120, 30), "krautchan.net/int"))
            {
                //open website
                Application.OpenURL("www.krautchan.net/int");
            }
            //quit Button
            if (GUI.Button(new Rect(40, 190, 120, 30), "Quit"))
            {
                Application.Quit();
            }
        }

        else if (show == "Play")
        {
            int yoffs = 20;
            if (GUI.Button(new Rect(40, 50+yoffs, 120, 30), "Enter Dungeon"))
            {
                //creating new gameobject which will store all the dungeon valuables
                GameObject go = new GameObject("DungeonVariables");
                go.AddComponent<DungeonVariables>();

                //if temporary dungeon size is equal to small, dungeon size is 6
                if (dungeonSize == "Small")
                {
                    go.GetComponent<DungeonVariables>().size = 1;
                }
                //if temporary dungeon size is equal to medium, dungeon size is 8
                else if (dungeonSize == "Medium")
                {
                    go.GetComponent<DungeonVariables>().size = 2;
                }
                //if temporary dungeon size is equal to large, dungeon size is 10
                else if (dungeonSize == "Large")
                {
                    go.GetComponent<DungeonVariables>().size = 3;
                }

                //adjusting the dungeon size to the right value
                if (dungeonType == "A")
                {
                    go.GetComponent<DungeonVariables>().type = DungeonVariables.Type.Corridors;
                }
                else if (dungeonType == "B")
                {
                    go.GetComponent<DungeonVariables>().type = DungeonVariables.Type.Corridors;
                }

                DontDestroyOnLoad(go);
                Application.LoadLevel("gvTest");
            }

            //adjust the dungeon size
            GUI.Label(new Rect(0, 95 + yoffs, 200, 30), "Dungeon Size: " + dungeonSize);
            if (GUI.Button(new Rect(65, 120 + yoffs, 70, 20), "Small"))
            {
                dungeonSize = "Small";
            }
            if (GUI.Button(new Rect(65, 145 + yoffs, 70, 20), "Medium"))
            {
                dungeonSize = "Medium";
            }
            if (GUI.Button(new Rect(65, 170 + yoffs, 70, 20), "Large"))
            {
                dungeonSize = "Large";
            }

            //adjust the dungeon type
            GUI.Label(new Rect(0, 205 + yoffs, 200, 30), "Dungeon Type: " + dungeonType);
            if (GUI.Button(new Rect(65, 230 + yoffs, 70, 20), "A"))
            {
                dungeonType = "A";
            }
            if (GUI.Button(new Rect(65, 255 + yoffs, 70, 20), "B"))
            {
                dungeonType = "B";
            }
            if (GUI.Button(new Rect(65, 280 + yoffs, 70, 20), "?"))
            {
                boxRect = new Rect(0, 0, 200, 400);
                show = "Help Type";
            }

            //back button
            if (GUI.Button(new Rect(40, 330 - yoffs/2, 120, 30), "Back"))
            {
                boxRect = new Rect(0, 0, 200, 250);
                show = "Main Menu";
            }
        }
        //if we are showing the help type menu
        else if (show == "Help Type")
        {
            int xoffs = 10;
            int yoffs = 50;
            GUI.Label(new Rect(10 + xoffs, 20 + yoffs, 180 - xoffs * 2, 70), "Type A");
            GUI.Label(new Rect(10 + xoffs, 35 + yoffs, 180 - xoffs * 2, 100), "Long corridors and several dead ends, that gives you the most interesting dungeon in different shapes.");
            GUI.Label(new Rect(10 + xoffs, 110 + yoffs, 180 - xoffs * 2, 100), "Disadvantage: You can easily spend a lot of time back tracking.");

            GUI.Label(new Rect(10 + xoffs, 175 + yoffs, 180 - xoffs * 2, 70), "Type B");
            GUI.Label(new Rect(10 + xoffs, 190 + yoffs, 180 - xoffs * 2, 100), "Grid dungeon, where everything is connected.");
            GUI.Label(new Rect(10 + xoffs, 225 + yoffs, 180 - xoffs * 2, 100), "Disadvantage: Can be a little confusing to navigate through.");

            if (GUI.Button(new Rect(40, 280 + yoffs, 120, 30), "Back"))
            {
                boxRect = new Rect(0, 0, 200, 390);
                show = "Play";
            }
        }
        //if we are showing the help menu
        else if (show == "Help")
        {
            int yoffs = 40;
            GUI.Label(new Rect(20, 30 + yoffs, 160, 100), "The game is turn-based");
            GUI.Label(new Rect(20, 55 + yoffs, 160, 100), "Use arrow keys to move");
            GUI.Label(new Rect(15, 80 + yoffs, 170, 100), "You can pick up items by walking in to them or by clicking on them (can't be more than 3 floors from you)");
            GUI.Label(new Rect(20, 150 + yoffs, 160, 100), "Use the stairs to go to a new dungeon");
            GUI.Label(new Rect(20, 190 + yoffs, 160, 100), "Use Escape to pauce");

            if (GUI.Button(new Rect(40, 230 + yoffs, 120, 30), "Back"))
            {
                boxRect = new Rect(0, 0, 200, 250);
                show = "Main Menu";
            }
        }

        GUI.EndGroup();

        int w = 200;
        int ww = entity1.width;
        int hh = entity1.height;
        GUI.DrawTexture(new Rect(Screen.width / 2 + w + ww*2, Screen.height / 4 - hh / 2, -130, 130), entity1);
        GUI.DrawTexture(new Rect(Screen.width / 2 - w / 2, Screen.height / 4 - hh / 2, -130, 130), entity2);
    }

}
