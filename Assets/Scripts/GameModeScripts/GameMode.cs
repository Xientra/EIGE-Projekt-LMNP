using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameMode  {
    void ProcessInput(KeyCode keyCode);
    void SetupScene();
    void CloseScene();
}
