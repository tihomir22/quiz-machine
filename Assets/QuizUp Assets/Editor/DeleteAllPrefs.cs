 using UnityEditor;
 using UnityEngine;
 
 public class DeleteAllPrefs : EditorWindow {
 
     [MenuItem("Edit/Reset Playerprefs")]
 
     public static void DeletePlayerPrefs()
     {
         PlayerPrefs.DeleteAll();
     }
 
 }