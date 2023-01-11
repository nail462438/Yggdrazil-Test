using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BaseType
{
    TypeA,
    TypeB,
    TypeC
}

public class TypeManager : MonoBehaviour
{
    public BaseType baseType;
}
