using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameJam
{
    public interface IEntityPeekPeek
    {
        Vector2 Pos { get; }
        float Radius { get; }
        bool IsWillRemove { get; }
    }

    public interface IEntity : IEntityPeekPeek
    {
        void Init();
        void Show();
        void Hide();
        void Release();
    }

    public interface IUpdatable
    {
        void UpdateEntity();

        bool IsWillRemove { get; }
    }
}