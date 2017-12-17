using UnityEngine;
using System.Collections;

public interface IState<TOwner> where TOwner : class
{
	void Enter(TOwner entity);
	void Update(TOwner entity);
	void Exit(TOwner entity);
}

public abstract class State<TOwner, TState> : Singleton<TState>, IState<TOwner> where TState : class, new() where TOwner : class, new()
{
	public abstract void Enter(TOwner entity);
	public abstract void Update(TOwner entity);
	public abstract void Exit(TOwner entity);
}
