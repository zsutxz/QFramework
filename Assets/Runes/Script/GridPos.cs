using UnityEngine;
public class GridPos {
    public int PosRow { get; set; }
    public int PosCol { get; set; }
    public Stone Stone { get; set; }

    public bool IsEmpty () {
        return Stone == null;
    }
    public void Clear () {
        Stone = null;
    }


}