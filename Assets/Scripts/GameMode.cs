using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameMode  {
    abstract public void ProcessInput(KeyCode keyCode);
    abstract public void SetupScene();
    abstract public void CloseScene();
}
