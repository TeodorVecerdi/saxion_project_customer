using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysqlTest : MonoBehaviour {
    private void Start() {
        var connection = new DBConnection();
        Debug.Log(connection.Highscores(5));
       
    }
}