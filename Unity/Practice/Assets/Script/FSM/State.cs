using UnityEngine;
using System.Collections;

public interface IState<TOwner> where TOwner : class
{
	void MainEnter(TOwner entity);
	void MainUpdate(TOwner entity);
	void MainExit(TOwner entity);
}

public abstract class State<TOwner, TState> : Singleton<TState>, IState<TOwner> where TState : class, new() where TOwner : class, new()
{
	public abstract void MainEnter(TOwner entity);
	public abstract void MainUpdate(TOwner entity);
	public abstract void MainExit(TOwner entity);
}
