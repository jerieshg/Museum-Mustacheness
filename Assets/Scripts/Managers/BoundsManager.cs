using UnityEngine;
using System.Collections;

public class BoundsManager : MonoBehaviour
{

	public static BoundsManager boundsManager;

	private Bounds activeBound;

	void Awake()
	{
		boundsManager = this;
	}

	void setActiveBound(Bounds bounds)
	{
		activeBound = bounds;
	}

}

