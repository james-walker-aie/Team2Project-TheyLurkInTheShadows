using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> enemies = new List<Transform>();
    public List<Transform> AttackGroup1 = new List<Transform>();
    public List<Transform> AttackGroup2 = new List<Transform>();
    Transform E1 = null;
    Transform E2 = null;
    Transform E3 = null;
    Transform E4 = null;
    GameObject player;
    float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(E1 != null)
        {
            if (E1.GetComponent<EnemyController>().state == EnemyController.State.Dead)
            {
                E1 = null;
            }
        }
        

        if (E2 != null)
        {
            if (E2.GetComponent<EnemyController>().state == EnemyController.State.Dead)
            {
                E2 = null;
            }
        }
        

        if (E3 != null)
        {
            if (E3.GetComponent<EnemyController>().state == EnemyController.State.Dead)
            {
                E3 = null;
            }
        }
        

        if (E4 != null)
        {
            if (E4.GetComponent<EnemyController>().state == EnemyController.State.Dead)
            {
                E4 = null;
            }
        }
       

        if (enemies.Count != 0)
        {
            timer -= Time.deltaTime;

            GetClosestEnemy(enemies);

            if (!AttackGroup1.Contains(E1))
            {
                if (AttackGroup2.Contains(E1))
                {
                    AttackGroup2.Remove(E1);
                }
                AttackGroup1.Add(E1);
                E1.GetComponent<EnemyController>().AttackGroup = 1;
            }
                
            if (!AttackGroup1.Contains(E2))
            {
                if (AttackGroup2.Contains(E2))
                {
                    AttackGroup2.Remove(E2);
                }

                AttackGroup1.Add(E2);
                E2.GetComponent<EnemyController>().AttackGroup = 1;
            }
                
            if (!AttackGroup1.Contains(E3))
            {
                if (AttackGroup2.Contains(E3))
                {
                    AttackGroup2.Remove(E3);
                }

                AttackGroup1.Add(E3);
                E3.GetComponent<EnemyController>().AttackGroup = 1;
            }

            if (!AttackGroup1.Contains(E4))
            {
                if (AttackGroup2.Contains(E4))
                {
                    AttackGroup2.Remove(E4);
                }

                AttackGroup1.Add(E4);
                E4.GetComponent<EnemyController>().AttackGroup = 1;
            }

            for (int i = 0; i < AttackGroup1.Count; i++)
            {
                if(AttackGroup1[i] != E1 && AttackGroup1[i] != E2 && AttackGroup1[i] != E3 && AttackGroup1[i] != E4)
                {
                    AttackGroup1.Remove(AttackGroup1[i]);
                }
            }

            for(int l = 0; l < enemies.Count; l++)
            {
                if (!AttackGroup1.Contains(enemies[l]))
                {
                    if (!AttackGroup2.Contains(enemies[l]))
                        AttackGroup2.Add(enemies[l]);
                    enemies[l].GetComponent<EnemyController>().AttackGroup = 2;
                }
            }
                
            if(AttackGroup1.Count > 0)
            {
                if (timer <= 0)
                {
                    int Rand = Random.Range(0, AttackGroup1.Count);


                    if (AttackGroup1[Rand] != null)
                    {
                        if(AttackGroup1[Rand].GetComponent<EnemyController>().Class == EnemyController.Type.Basic)
                        {
                            if (AttackGroup1[Rand].GetComponent<EnemyController>().distance <= 4)
                            {
                                if (AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Block && AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Hit)
                                    AttackGroup1[Rand].GetComponent<EnemyController>().canAttack = true;

                                timer = 3;
                            }
                        }
                        else
                        if(AttackGroup1[Rand].GetComponent<EnemyController>().Class == EnemyController.Type.Heavy)
                        {
                            if (AttackGroup1[Rand].GetComponent<EnemyController>().distance <= 4)
                            {
                                if (AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Block && AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Hit)
                                    AttackGroup1[Rand].GetComponent<EnemyController>().canAttack = true;

                                timer = 3;
                            }
                            else
                            if(AttackGroup1[Rand].GetComponent<EnemyController>().distance <= 6)
                            {
                                if (AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Block && AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Hit)
                                    AttackGroup1[Rand].GetComponent<EnemyController>().canAttack = true;
                                timer = 3;
                            }

                        }
                        else
                        if (AttackGroup1[Rand].GetComponent<EnemyController>().Class == EnemyController.Type.Mystic)
                        {
                            if (AttackGroup1[Rand].GetComponent<EnemyController>().distance <= 3.5f)
                            {
                                if (AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Block && AttackGroup1[Rand].GetComponent<EnemyController>().state != EnemyController.State.Hit)
                                    AttackGroup1[Rand].GetComponent<EnemyController>().canAttack = true;

                                timer = 3;
                            }
                        }



                    }
                }
      
            }

        }

    }

    void GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        Transform bestTarget1 = null;
        Transform bestTarget2 = null;
        Transform bestTarget3 = null;

        float closestDistanceSqr = Mathf.Infinity;
        float closestDistanceSqr1 = Mathf.Infinity;
        float closestDistanceSqr2 = Mathf.Infinity;
        float closestDistanceSqr3 = Mathf.Infinity;

        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            
            float dSqrToTarget = Vector3.Distance(player.transform.position, potentialTarget.transform.position);
            if (potentialTarget != E1 && potentialTarget != E2 && potentialTarget != E3 && potentialTarget != E4 && potentialTarget.GetComponent<EnemyController>().state != EnemyController.State.Dead)
            {
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                    E1 = bestTarget;
                    //if (!AttackGroup1.Contains(potentialTarget))
                    //AttackGroup1.Add(potentialTarget);
                }
                else
                if (dSqrToTarget < closestDistanceSqr1)
                {
                    closestDistanceSqr1 = dSqrToTarget;
                    bestTarget1 = potentialTarget;
                    E2 = bestTarget1;
                    //if (!AttackGroup1.Contains(potentialTarget))
                    //AttackGroup1.Add(potentialTarget);
                }
                else
                if (dSqrToTarget < closestDistanceSqr2)
                {
                    closestDistanceSqr2 = dSqrToTarget;
                    bestTarget2 = potentialTarget;
                    E3 = bestTarget2;
                    //if (!AttackGroup1.Contains(potentialTarget))
                    //AttackGroup1.Add(potentialTarget);
                }
                else
                if(dSqrToTarget < closestDistanceSqr3)
                {
                    closestDistanceSqr3 = dSqrToTarget;
                    bestTarget3 = potentialTarget;
                    E4 = bestTarget3;
                }
                
            }
        }
            

        
    }

}
