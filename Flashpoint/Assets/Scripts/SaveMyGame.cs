using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public bool loadOnStart = true;
        public GameObject board;
        FirefighterController[] controlFirefighters;
        //public BoardManager BoardManager;

        //public BoardData myData;
        public string identifier = "mySave";

        void Start()
        {
            if (loadOnStart)
            {
                Load();
            }
        }

        void OnApplicationQuit()
        {
            Save();
        }

        void Update()
        {
            controlFirefighters = FindObjectsOfType<FirefighterController>();
        }


        public void Save()
        {
            
            
            
            SaveBoard(board);
            SaveFF(controlFirefighters);
            //
            //SaveGame.Save<QuaternionSave>(identifier, target.rotation, SerializerDropdown.Singleton.ActiveSerializer);
            //SaveGame.Save<Vector3Save>(identifier, target.localScale, SerializerDropdown.Singleton.ActiveSerializer);
        }

        public void Load()
        {
            /*target.transform.position = SaveGame.Load<Vector3Save>(
                identifier,
                Vector3.zero,
                serializer);*/
            LoadBoard(board);
            LoadFF(controlFirefighters);
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
                    SaveGame.Save<SpaceStatus>(child.name, child.GetComponent<Space>().status, serializer);
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
                            SaveGame.Save<WallState>(mychild.name, mychild.GetComponent<Wall>().state, serializer);
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                        else if (mychild.gameObject.GetComponent<Door>() != null)
                        {
                            //Debug.Log("space saved");
                            SaveGame.Save<DoorState>(mychild.name, mychild.GetComponent<Door>().state, serializer);
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
                    SpaceStatus status = SaveGame.Load<SpaceStatus>(child.name, new SpaceStatus(), serializer);
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
                            WallState status = SaveGame.Load<WallState>(mychild.name, new WallState(), serializer);
                            if (mychild.gameObject.GetComponent<Wall>().state != status)
                            {
                                mychild.gameObject.GetComponent<Wall>().SetState(status);
                            }
                    
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                        else if (mychild.gameObject.GetComponent<Door>() != null)
                        {
                            DoorState status = SaveGame.Load<DoorState>(mychild.name, new DoorState(), serializer);
                            if (mychild.gameObject.GetComponent<Door>().state != status)
                            {
                                mychild.gameObject.GetComponent<Door>().SetState(status);
                            }
                            SaveGame.Save<DoorState>(mychild.name, mychild.GetComponent<Door>().state, serializer);
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                    }
                }
            }

        }


        public void SaveFF(FirefighterController[] allFF)
        {
            foreach (FirefighterController f in allFF)
            {
                Transform target = f.gameObject.transform;
                SavePosition(target, f.name);
                SaveGame.Save<int>(f.gameObject.name + "AP", f.gameObject.GetComponent<FirefighterManager>().getAP(), serializer);
            }
            
        }

        private void SavePosition(Transform target, string name)
        {
            SaveGame.Save<Vector3Save>(name, target.position, serializer);
        }

        public void LoadFF(FirefighterController[] allFF)
        {
            foreach (FirefighterController f in allFF)
            {
                
                GameObject target = f.gameObject;
                LoadPosition(target, f.name);
                int a = SaveGame.Load<int>(target.name + "AP", new int(), serializer);
                f.gameObject.GetComponent<FirefighterManager>().setAP(a);
            }

        }

        private void LoadPosition(GameObject target, string name)
        {
            target.transform.position = SaveGame.Load<Vector3Save>(
                name,
                Vector3.zero,
                serializer); 
        }
    }

}

