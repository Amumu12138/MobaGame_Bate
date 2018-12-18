using System;
using System.Collections.Generic;

public struct ElementAttribute
{
    string elementKey;
    string elementValue;

    public ElementAttribute(string _elementKey, object _elementValue)
    {
        elementKey = _elementKey;
        elementValue = _elementValue.ToString();
    }

    public string ElementKey { get { return elementKey; } }
    public string ElementValue { get { return elementValue; } }

	public string ElementToString { get { return elementValue; } }
	public string[] ElementToStrings{ get { return elementValue.Split ('/'); } }

	public int ElementToInt { get { return Convert.ToInt32 (elementValue); } }
	public int[] ElementToInts
	{
		get
		{
			string[] tempElementString = ElementToStrings;
			int[] tempElementInt = new int[tempElementString.Length];

			for (int i = 0; i < tempElementString.Length; i++) 
			{
				tempElementInt[i] = Convert.ToInt32 (tempElementString[i]);
			}

			return tempElementInt;
		}
	}

	public float ElementToFloat{ get { return Convert.ToSingle (elementValue); } }
	public float[] ElementToFloats
	{
		get
		{
			string[] tempElementString = ElementToStrings;
			float[] tempElementFloat = new float[tempElementString.Length];
			
			for (int i = 0; i < tempElementString.Length; i++) 
			{
				tempElementFloat[i] = Convert.ToSingle (tempElementString[i]);
			}
			
			return tempElementFloat;
		}
	}
}

public struct NodeAttribute
{
    string nodeKey;
    List<ElementAttribute> elementValue;

    public NodeAttribute(string _nodeKey, List<ElementAttribute> _elementValue)
    {
        nodeKey = _nodeKey;
        elementValue = _elementValue;
    }

    public string NodeKey { get { return nodeKey; } }
    public List<ElementAttribute> NodeValue { get { return elementValue; } }

    public void RemoveChild(string _elementKey)
    {
        for (int i = 0; i < elementValue.Count; i++)
        {
            if (elementValue[i].ElementKey == _elementKey)
            {
                elementValue.RemoveAt(i);
                break;
            }
        }
    }

    public string ElementValue(string elementKey)
    {
        if (null == elementValue)
        {
            throw new UnityEngine.UnityException("nodeValue 值为空");
        }

        for (int i = 0; i < elementValue.Count; i++)
        {
            if (elementValue[i].ElementKey == elementKey)
            {
                return elementValue[i].ElementValue;
            }
        }

        return "";
    }
}