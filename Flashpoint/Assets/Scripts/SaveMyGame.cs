using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree.Types;
using System;

namespace BayatGames.SaveGameFree.Examples
{
    

    public class SaveMyGame : MonoBehaviour
    {

        

        

        ISaveGameSerializer serializer = new SaveGameBinarySerializer();


        //public BoardData b;
        //public Transform target;
        public GameObject canvas;
        public int n;
        public static SaveMyGame saveObject;
        public GameObject gameManager;
        public bool loadOnStart;
        public GameObject board;
        public FirefighterManager[] controlFirefighters;
        public bool loaded = false;
        public Button save;
        public Button load;
        public Button load2;
        //public BoardManager BoardManager;

        //public BoardData myData;
        public string identifier;
        string identifier1 = "mySave1";
        string identifier2 = "mySave2";

        public void setIdentifier(int i)
        {
            if (i == 1)
            {
                identifier = identifier1;
            }
            if (i == 2)
            {
                identifier = identifier2;
            }
        }

        private void Awake()
        {
            if (saveObject == null)
            {
                DontDestroyOnLoad(this.gameObject);
                saveObject = this;

            } else if (saveObject != this)
            {
                Destroy(gameObject);

            }
            
        }

        void Start()
        {
            
        }

        void OnApplicationQuit()
        {
            Save();
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().name == "Main Screen" && !CanSave())
            {
                board = GameObject.Find("Board");
                gameManager = GameObject.Find("GameManager");
                controlFirefighters = FindObjectsOfType<FirefighterManager>();
                canvas = GameObject.Find("Canvas");
                save = canvas.transform.GetChild(12).GetComponent<Button>();
                save.onClick.AddListener(() => setIdentifier(n));
                load = canvas.transform.GetChild(13).GetComponent<Button>();
                load.onClick.AddListener(() => Load(identifier1));
                load2 = canvas.transform.GetChild(14).GetComponent<Button>();
                load2.onClick.AddListener(() => Load(identifier2));
            }

            
            if (loadOnStart && SceneManager.GetActiveScene().name == "Main Screen" && !loaded)
            {
                Debug.Log("On start load");
                Load(identifier);
                loaded = true;
                loadOnStart = false;
            }

        }
        

        public void SetLoadOnStartToTrue(int i)
        {
            setIdentifier(i);
            loadOnStart = true;
        }

        bool CanSave()
        {
            return board != null && gameManager != null && controlFirefighters != null;
        }

        public void Save()
        {
            setIdentifier(n);
            if (CanSave())
            {
                for (int i = 0; i <2; i++)
                {
                    setIdentifier(i + 1);
                    SaveBoard(board);
                    SaveFF(controlFirefighters);
                    SaveGameManager(gameManager);

                }
                
            }
            
            
            //
            //SaveGame.Save<QuaternionSave>(identifier, target.rotation, SerializerDropdown.Singleton.ActiveSerializer);
            //SaveGame.Save<Vector3Save>(identifier, target.localScale, SerializerDropdown.Singleton.ActiveSerializer);
        }

        public void Load(string ident)
        {
            identifier = ident;
            Debug.Log(identifier);
            //
            /*target.transform.position = SaveGame.Load<Vector3Save>(
                identifier,
                Vector3.zero,
                serializer);*/
            Debug.Log("load");
            LoadGameManager(gameManager);
            LoadBoard(board);
            LoadFF(controlFirefighters);
        }

        public void SaveGameManager (GameObject gameManager)
        {
            SaveGame.Save<bool>(identifier+gameManager.name + "HasLevelStarted", gameManager.GetComponent<Game>().HasLevelStarted, serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "IsGamePlaying", gameManager.GetComponent<Game>().IsGamePlaying, serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "IsGameOver", gameManager.GetComponent<Game>().IsGameOver, serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "asLevelFinished", gameManager.GetComponent<Game>().HasLevelFinished, serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "IsEndTurnPlaying", gameManager.GetComponent<Game>().IsEndTurnPlaying, serializer);

            SaveGame.Save<bool>(identifier + gameManager.name + "MoveButton", gameManager.GetComponent<Game>().GetMoveButtonState(), serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "PunchButton", gameManager.GetComponent<Game>().GetPunchButtonState(), serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "TouchButton", gameManager.GetComponent<Game>().GetTouchButtonState(), serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "ExtinguishButton", gameManager.GetComponent<Game>().GetExtinguishButtonState(), serializer);
            SaveGame.Save<bool>(identifier + gameManager.name + "EndTurnButton", gameManager.GetComponent<Game>().GetEndTurnButtonState(), serializer);

        }

        public void LoadGameManager(GameObject gameManager)
        {
            gameManager.GetComponent<Game>().HasLevelStarted = SaveGame.Load<bool>(identifier + gameManager.name + "HasLevelStarted", new bool(), serializer);
            gameManager.GetComponent<Game>().IsGamePlaying = SaveGame.Load<bool>(identifier + gameManager.name + "IsGamePlaying", new bool(), serializer);
            SaveGame.Load<bool>(identifier + gameManager.name + "IsGameOver", gameManager.GetComponent<Game>().IsGameOver, serializer);
            gameManager.GetComponent<Game>().HasLevelFinished = SaveGame.Load<bool>(identifier + gameManager.name + "asLevelFinished",  new bool(), serializer);
            gameManager.GetComponent<Game>().IsEndTurnPlaying = SaveGame.Load<bool>(identifier + gameManager.name + "IsEndTurnPlaying",  new bool(), serializer);

            gameManager.GetComponent<Game>().SetMoveButtonState( SaveGame.Load<bool>(identifier + gameManager.name + "MoveButton",  new bool(), serializer));
            gameManager.GetComponent<Game>().SetMoveButtonState(SaveGame.Load<bool>(identifier + gameManager.name + "PunchButton",  new bool(), serializer));
            gameManager.GetComponent<Game>().SetMoveButtonState(SaveGame.Load<bool>(identifier + gameManager.name + "TouchButton",  new bool(), serializer));
            gameManager.GetComponent<Game>().SetMoveButtonState(SaveGame.Load<bool>(identifier + gameManager.name + "ExtinguishButton",  new bool(), serializer));
            gameManager.GetComponent<Game>().SetMoveButtonState(SaveGame.Load<bool>(identifier + gameManager.name + "EndTurnButton", new bool(), serializer));

        }
        public void SaveBoard(GameObject Board)
        {
            Debug.Log("data Path " + Application.dataPath);
            Debug.Log("persistent data Path " + Application.persistentDataPath);
            foreach (Transform child in Board.GetComponentsInChildren<Transform>())
            {
               // Debug.Log(child);
                if (child.gameObject.GetComponent<Space>() !=  null){
                    //Debug.Log("space saved");
                    SaveGame.Save<SpaceStatus>(identifier + child.name, child.GetComponent<Space>().status, serializer);
                    //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                    //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                }
                else if (child.name == "Walls")
                {
                    Debug.Log("in walls");
                    foreach (Transform mychild in child.transform)
                    {

                        if (mychild.gameObject.GetComponent<Wall>() != null)
                        {
                            //Debug.Log("space saved");
                            SaveGame.Save<WallState>(identifier + mychild.name, mychild.GetComponent<Wall>().state, serializer);
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                        else if (mychild.gameObject.GetComponent<Door>() != null)
                        {
                            //Debug.Log("space saved");
                            SaveGame.Save<DoorState>(identifier + mychild.name, mychild.GetComponent<Door>().state, serializer);
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                    }

                
                }
            }

        }

        public void LoadBoard(GameObject Board)
        {
            
            
            Debug.Log("in load");
            foreach (Transform child in Board.transform)
            {
                //Debug.Log(child);
                if (child.gameObject.GetComponent<Space>() != null)
                {
                    //Debug.Log("space loaded");
                    SpaceStatus status = SaveGame.Load<SpaceStatus>(identifier + child.name, new SpaceStatus(), serializer);
                    if (child.GetComponent<Space>().status != status)
                    {
                        child.GetComponent<Space>().SetStatus(status);
                    }
                        
                    //child.GetComponent<Space>().x = SaveGame.Load<int>(child.name + "x", new int(), serializer);
                    //child.GetComponent<Space>().y = SaveGame.Load<int>(child.name + "y", new int(), serializer);

                }
                if (child.name == "Walls")
                {
                    Debug.Log("in walls");
                    foreach (Transform mychild in child.transform)
                    {

                        if (mychild.gameObject.GetComponent<Wall>() != null)
                        {
                            WallState status = SaveGame.Load<WallState>(identifier + mychild.name, new WallState(), serializer);
                            if (mychild.gameObject.GetComponent<Wall>().state != status)
                            {
                                mychild.gameObject.GetComponent<Wall>().SetState(status);
                            }
                    
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                        else if (mychild.gameObject.GetComponent<Door>() != null)
                        {
                            DoorState status = SaveGame.Load<DoorState>(identifier + mychild.name, new DoorState(), serializer);
                            if (mychild.gameObject.GetComponent<Door>().state != status)
                            {
                                mychild.gameObject.GetComponent<Door>().SetState(status);
                            }
                            SaveGame.Save<DoorState>(identifier + mychild.name, mychild.GetComponent<Door>().state, serializer);
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                    }
                }
            }

        }


        public void SaveFF(FirefighterManager[] allFF)
        {
            foreach (FirefighterManager f in allFF)
            {
                Transform target = f.gameObject.transform;
                SavePosition(target, f.name);
                SaveGame.Save<Space>(identifier + gameManager.name + "space", f.CurrentSpace, serializer);
                SaveGame.Save<bool>(identifier + gameManager.name + "spawned", f.isSpawned, serializer);
                SaveGame.Save<int>(identifier + gameManager.name + "AP", f.myAP, serializer);
                SaveGame.Save<int>(identifier + gameManager.name + "savedAp", f.mysavedAp, serializer);
                SaveGame.Save<bool>(identifier + gameManager.name + "IsGameOver", f.GetIsMyTurn, serializer);
                SaveGame.Save<bool>(identifier + gameManager.name + "IsCarryingVictim", f.GetIsCarryingVictim, serializer);


            }
            /*foreach (FirefighterController f in allFF)
            {
                Transform target = f.gameObject.transform;
                SavePosition(target, f.name);
                SaveGame.Save<int>(identifier + f.gameObject.name + "AP", f.gameObject.GetComponent<FirefighterManager>().getAP(), serializer);
                //SaveGame.Save<Color>(f.gameObject.name + "color", f.gameObject.GetComponent<SetupLocalPlayer>().playerColor, serializer);
            }*/

        }

        private void SavePosition(Transform target, string name)
        {
            SaveGame.Save<Vector3Save>(identifier + name, target.position, serializer);
        }

        public void LoadFF(FirefighterManager[] allFF)
        {
            foreach (FirefighterManager f in allFF)
            {
                GameObject target = f.gameObject;
                LoadPosition(target, f.name);
                f.CurrentSpace = SaveGame.Load<Space>(identifier + gameManager.name + "space", new Space(), serializer);
                f.isSpawned = SaveGame.Load<bool>(identifier + gameManager.name + "spawned",new bool(), serializer);
                f.myAP = SaveGame.Load<int>(identifier + gameManager.name + "AP", new int(), serializer);
                f.mysavedAp = SaveGame.Load<int>(identifier + gameManager.name + "savedAp", new int() , serializer);
                f.GetIsMyTurn = SaveGame.Load<bool>(identifier + gameManager.name + "IsGameOver" ,new bool(), serializer);
                f.GetIsCarryingVictim = SaveGame.Load<bool>(identifier + gameManager.name + "IsCarryingVictim", new bool(), serializer);

            }
            /*foreach (FirefighterController f in allFF)
            {
                
                GameObject target = f.gameObject;
                LoadPosition(target, f.name);
                int a = SaveGame.Load<int>(identifier + target.name + "AP", new int(), serializer);
                f.gameObject.GetComponent<FirefighterManager>().setAP(a);
                //f.gameObject.GetComponent<SetupLocalPlayer>().playerColor = SaveGame.Load<Color>(f.gameObject.name + "color", new Color(), serializer);
            }
            */
        }

        private void LoadPosition(GameObject target, string name)
        {
            target.transform.position = SaveGame.Load<Vector3Save>(
                identifier + name,
                Vector3.zero,
                serializer); 
        }
    }

}

