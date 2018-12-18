using UnityEngine;
using System.Collections.Generic;

public class EUnit
{
	public int					hashcode = 0;
	public GameEventCallBack	callback = null;

	public void Dispose(){ Reset(); }

	public void Set(int hc, GameEventCallBack cb)
	{
		hashcode = hc;
		callback = cb;
	}

	public void Reset()
	{
		hashcode = 0;
		callback = null;
	}
}

public class Events : MonoBehaviour
{
	public static Events msInst	= null;
    
    private List<EUnit>	mEUnitPool 		= null;
	private List<List<EUnit>> mListPool	= null;
	private Dictionary<string, List<EUnit>> mGameEvent2Listeners = null;

	public List<EUnit>	EUnitPool{ get { return mEUnitPool; } }
	public List<List<EUnit>> ListPool	{ get { return mListPool; } }
	public Dictionary<string, List<EUnit>> GameEvent2Listeners { get { return mGameEvent2Listeners; } }

	void Awake()
	{
		msInst = this;

		mEUnitPool = new List<EUnit>();
		mListPool  = new List<List<EUnit>>();
		for(int i = 0; i < 50; i++)
		{
			mListPool.Add(new List<EUnit>());
		}

		mGameEvent2Listeners = new Dictionary<string, List<EUnit>>();
	}

	private EUnit BorrowEUnit()
	{
		EUnit tEUnit = null;
		if(mEUnitPool.Count > 0)
		{
			tEUnit = mEUnitPool[0];
			mEUnitPool.RemoveAt(0);
		}
		else
		{
			tEUnit = new EUnit();
		}
		return tEUnit;
	}

	private void ReturnEUnit(EUnit unit)
	{
		if(unit != null)
		{
			unit.Reset();
			mEUnitPool.Add(unit);
		}
	}

	private List<EUnit> BorrowList()
	{
		List<EUnit> tList = null;
		if(mListPool.Count > 0)
		{
			tList = mListPool[0];
			mListPool.RemoveAt(0);
		}
		else
		{
			tList = new List<EUnit>();
		}
		return tList;
	}

	private void ReturnList(List<EUnit> list)
	{
		if(list != null)
		{
			for(int i = 0; i < list.Count; i++)
			{
				ReturnEUnit(list[i]);
			}
			list.Clear();

			mListPool.Add(list);
		}
	}

	public void AddEventListener(string evt, GameEventCallBack callback)
	{
		AddEventListener(evt, callback, null);
	}

	public void AddEventListener(string evt, GameEventCallBack callback, object owner)
	{
		List<EUnit> tList = null;
		if(!mGameEvent2Listeners.TryGetValue(evt, out tList))
		{
			tList = BorrowList();
			mGameEvent2Listeners.Add(evt, tList);
		}
		
		bool bHas = false;
		for(int i = 0; i < tList.Count; i++)
		{
			if(tList[i].callback == callback)
			{
				bHas = true;
				break;
			}
		}
		if(!bHas)
		{
			EUnit tEUnit = BorrowEUnit();
			tEUnit.Set((owner == null ? 0 : owner.GetHashCode()), callback);
			tList.Add(tEUnit);
		}
	}

	public void RemoveEventListener(string evt, GameEventCallBack callback)
	{
		List<EUnit> tList = null;
		if(mGameEvent2Listeners.TryGetValue(evt, out tList))
		{
			for(int i = 0; i < tList.Count; i++)
			{
				if(tList[i].callback == callback)
				{
					ReturnEUnit(tList[i]);
					tList.RemoveAt(i);
					break;
				}
			}
			if(tList.Count <= 0)
			{
				ReturnList(tList);
				mGameEvent2Listeners.Remove(evt);
			}
		}
    }

	public void RemoveEventListener(object owner)
	{
		if(owner != null)
		{
			int tHasCode = owner.GetHashCode();
			List<EUnit> tList = null;
			bool tFinish = false;
			while(!tFinish)
			{
				tFinish = true;
				foreach(KeyValuePair<string, List<EUnit>> pair in mGameEvent2Listeners)
				{
					tList = pair.Value;
					if(tList != null)
					{
						for(int i = 0; i < tList.Count; i++)
						{
							if(tList[i].hashcode == tHasCode)
							{
								ReturnEUnit(tList[i]);
								tList.RemoveAt(i);
								--i;
							}
						}

						if(tList.Count <= 0)
						{
							ReturnList(tList);
							mGameEvent2Listeners.Remove(pair.Key);
							tFinish = false;
							break;
						}
					}
				}
			}
		}
	}

	public void DispatchEvent(string evt, object param)
	{
		List<EUnit> tList = null;
		if(mGameEvent2Listeners.TryGetValue(evt, out tList))
		{
			for(int i = tList.Count - 1; i >= 0; i--)
			{
				if(tList[i] != null && tList[i].callback != null)
				{
					tList[i].callback.Invoke(param);
				}
			}
		}
    }
}