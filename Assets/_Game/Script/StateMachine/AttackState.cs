using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.FindBridge();
    }

    public void OnExecute(Enemy enemy)
    {
        if (enemy.GetBrickCount() == 0)
        {
            enemy.ChangeState(new PatrolState());
        } 
    }

    public void OnExit(Enemy enemy)
    {

    }
}
