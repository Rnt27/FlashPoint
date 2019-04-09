using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BayatGames.SaveGameFree.Serializers;
using BayatGames.SaveGameFree.Types;

namespace BayatGames.SaveGameFree.Examples
{
    

    public class SaveMyGame : MonoBehaviour
    {

        

        

        ISaveGameSerializer serializer = new SaveGameBinarySerializer();


        //public BoardData b;
        public Transform target;
        public bool loadOnStart = true;
        public GameObject board;
        public BoardManager BoardManager;

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
        
        
        public void Save()
        {
            
            
            
            SaveBoard(board);
            //SaveGame.Save<Vector3Save>(identifier, target.position, serializer);
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
        }

        public void SaveBoard(GameObject Board)
        {
            Debug.Log("in save");
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
                            if (mychild.GetComponent<Wall>().state != status)
                            {
                                mychild.GetComponent<Wall>().SetState(status);
                            }
                    
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                        else if (mychild.gameObject.GetComponent<Door>() != null)
                        {
                            DoorState status = SaveGame.Load<DoorState>(mychild.name, new DoorState(), serializer);
                            if (mychild.GetComponent<Door>().state != status)
                            {
                                mychild.GetComponent<Door>().SetState(status);
                            }
                            SaveGame.Save<DoorState>(mychild.name, child.GetComponent<Door>().state, serializer);
                            //SaveGame.Save<int>(child.name + "x", child.GetComponent<Space>().x, serializer);
                            //SaveGame.Save<int>(child.name+"y", child.GetComponent<Space>().y, serializer);

                        }
                    }
                }
            }

        }

    }

}

