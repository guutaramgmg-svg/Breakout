using UnityEngine;

[CreateAssetMenu(menuName = "Field")]
public class Field : ScriptableObject
{
    public int FieldHp = 1;

    public FieldObjectData.Move FieldMove;
    
    public FieldObjectData.FieldObj FieldObj;


}
