using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float timer;

    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        enemy.MoveToBrick();
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (enemy.GetBrickCount() <= 10)
        {
            if (timer > 10f)
            {
                enemy.MoveToBrick();
                timer = 0;
            }
            else
            {
                if (enemy.GetDistance(enemy.GetDestination()) > 0.1f)
                {
                    return;
                }
                else
                {
                    enemy.MoveToBrick();
                }
            }
        }
        else
        {
            enemy.ChangeState(new AttackState());
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
