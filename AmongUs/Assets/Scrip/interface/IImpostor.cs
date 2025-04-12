using UnityEngine;

public interface IImpostor
{
    void Kill();
    void Sabotage();
    bool CanUseVent();

}

public interface ICrewMate
{
    void Report();
}
