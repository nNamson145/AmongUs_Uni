using UnityEngine;

public interface IRole
{
    void TickRole();
}

public interface IImpostor : IRole
{
    void Kill();
    void Sabotage();
    bool CanUseVent();
}

public interface ICrewMate : IRole
{
    void Report();
}
