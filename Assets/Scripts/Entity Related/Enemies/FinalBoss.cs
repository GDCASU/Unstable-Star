using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FinalBoss : CombatEntity
{
    public enum MoveState
	{
        IDLE,
        MOVETO,
        HORIZONTAL,
        HORIZONTALFANCY
	}

    [SerializeField] private const int MAX_HEALTH = 200;
    private const float MIN_X = -25f;
    private const float MAX_X = 25f;
    private const float MIN_Y = 15f;
    private const float MAX_Y = 25f;
    private const float CENTER_X = (MIN_X + MAX_X) / 2f;
    private const float CENTER_Y = (MIN_Y + MAX_Y) / 2f;
    private const float DEFAULT_X_MOVE_SPEED = 25f;
    private const float DEFAULT_Y_FANCY_PERIOD = 1.5f;

    [SerializeField] private GameObject _anchorObject;
    [SerializeField] private ScriptableWeapon _bulletWeapon;
    [SerializeField] private ScriptableWeapon _fireworkWeapon;

    private int _currentPhase => 5 - Mathf.CeilToInt(4 * (float)health / MAX_HEALTH);
    private MoveState _moveState;
    private bool _movingRight;
    private bool calculatedFancyMovementVars = false;
    private float xMoveSpeed = DEFAULT_X_MOVE_SPEED;
    private float yFancyPeriod = DEFAULT_Y_FANCY_PERIOD;
    private float yTimer = 0f;
    private float tOffset = 0f;

    private void Start()
	{
        health = MAX_HEALTH;
        StartCoroutine(AttackSelection());
	}

    private IEnumerator AttackSelection()
	{
        // TODO: remove
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));

        Func<IEnumerator>[] attacks = new Func<IEnumerator>[] { SpreadAttack, WaveAttack, FireworkAttack };
        while(health > 0)
		{
            if(_currentPhase != 4)
			{
                yield return attacks[_currentPhase - 1].Invoke();
                yield return new WaitForSeconds(1f);
                continue;
			}
            yield return attacks[Random.Range(0, 3)].Invoke();
        }
	}

    private IEnumerator SpreadAttack()
	{
        Log("Doing spread attack");
        if(_currentPhase == 4) _moveState = MoveState.HORIZONTAL;
        for(int i = 0; i < (_currentPhase == 4 ? 2 : 4); i++)
		{
            float offset = Random.Range(0f, 10f);
            int gap = Random.Range(-6, 7);
            for(int j = -9; j <= 9; j++)
            {
                if(j != gap) FireWeapon(_bulletWeapon.GetWeaponObject(), _anchorObject.transform.position, j * 10f + offset);
            }
            SoundManager.instance.PlaySound(_bulletWeapon.GetWeaponObject().sound);
            yield return new WaitForSeconds(_currentPhase == 4 ? 0.5f : 0.75f);
        }
        if(_currentPhase == 4) _moveState = MoveState.HORIZONTALFANCY;
        yield break;
	}

    private IEnumerator WaveAttack()
	{
        Log("Doing wave attack");
        _movingRight = Random.Range(0, 2) == 1;
        xMoveSpeed  = 2f * DEFAULT_X_MOVE_SPEED;
        for(int i = 0; i < 3; i++)
		{
            _moveState = MoveState.HORIZONTAL;
            int repeat = Random.Range(20, 40);
            for(int j = 0; j < repeat; j++)
            {
                FireWeapon(_bulletWeapon.GetWeaponObject(), _anchorObject.transform.position, 0f);
                SoundManager.instance.PlaySound(_bulletWeapon.GetWeaponObject().sound);
                yield return new WaitForSeconds(0.05f);
            }
            _moveState = MoveState.HORIZONTALFANCY;
            yield return new WaitForSeconds(_currentPhase == 4 ? 0.15f : 0.25f);
        }
        xMoveSpeed = DEFAULT_X_MOVE_SPEED;
        yield break;
    }

    private IEnumerator FireworkAttack()
	{
        Log("Doing firework attack");
        if(_currentPhase != 4) _moveState = MoveState.IDLE;
        for(int i = 0; i < (_currentPhase == 4 ? 2 : 3); i++)
		{
            Vector3 target = Player.Instance.transform.position + new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            GameObject firework = FireWeapon(_fireworkWeapon.GetWeaponObject(), _anchorObject.transform.position, target);
            SoundManager.instance.PlaySound(_fireworkWeapon.GetWeaponObject().sound);
            StartCoroutine(FireworkExplosion(firework, target));
            yield return new WaitForSeconds(0.5f);
        }
        _moveState = MoveState.HORIZONTALFANCY;
        yield break;
    }

    private IEnumerator FireworkExplosion(GameObject firework, Vector3 target)
	{
        float timer = 0f;
        Vector3 lastPosition = firework.transform.position;
        yield return new WaitUntil(() =>
        {
            timer += Time.deltaTime;
            lastPosition = firework.transform.position;
            return firework == null || Vector3.Distance(firework.transform.position, target) < 3f;
        });
        if(timer > 5f) yield break;

        Log("Firework reached target or was destroyed");
        for(int i = 0; i < 16; i++)
		{
            FireWeapon(_bulletWeapon.GetWeaponObject(), lastPosition, i * 22.5f);
        }
        Destroy(firework);
        SoundManager.instance.PlaySound(_bulletWeapon.GetWeaponObject().sound);
    }

    private GameObject FireWeapon(Weapon weapon, Vector3 origin, float degreeOffset)
	{
        float angle = _anchorObject.transform.rotation.eulerAngles.z + degreeOffset;
        GameObject firedProjectile = Instantiate(weapon.prefab, origin, Quaternion.Euler(0f, 0f, angle), ProjectileContainer.Instance.transform);
        ProjectileObject projectileData = firedProjectile.GetComponent<ProjectileObject>();
        projectileData.SetData(_anchorObject.tag, PhysicsConfig.Get.ProjectilesEnemies, weapon.speed, weapon.damage);
        return firedProjectile;
    }

    private GameObject FireWeapon(Weapon weapon, Vector3 origin, Vector3 target)
	{
        float angle = Vector3.Angle(Vector3.down, target - _anchorObject.transform.position);
        if(target.x < _anchorObject.transform.position.x) angle *= -1f;
        return FireWeapon(weapon, origin, angle);
    }

    private IEnumerator MoveTo(Vector3 position, float moveTime)
	{
        MoveState prevState = _moveState;
        _moveState = MoveState.MOVETO;
        Vector3 startPos = transform.position;
        Vector3 endPos = position;
        float t = 0f;
        while(t < moveTime)
		{
            yield return null;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / moveTime);
        }
        _moveState = prevState;
	}

	private void Update()
	{
		switch(_moveState)
		{
            case MoveState.IDLE:
                calculatedFancyMovementVars = false;
                break;
            case MoveState.MOVETO:
                calculatedFancyMovementVars = false;
                break;
            case MoveState.HORIZONTAL:
                calculatedFancyMovementVars = false;
                transform.position += new Vector3((_movingRight ? xMoveSpeed : -xMoveSpeed) * Time.deltaTime, 0f);
                if(transform.position.x <= MIN_X)
                {
                    _movingRight = true;
                }
                if(transform.position.x >= MAX_X)
                {
                    _movingRight = false;
                }
                break;
            case MoveState.HORIZONTALFANCY:
                if(!calculatedFancyMovementVars)
				{
                    tOffset = Mathf.Asin((transform.position.y - CENTER_Y) / (MAX_Y - CENTER_Y));
                    yTimer = 0f;
                    calculatedFancyMovementVars = true;
                }
                yTimer += Time.deltaTime;
                transform.position = new Vector3(transform.position.x + (_movingRight ? xMoveSpeed : -xMoveSpeed) * Time.deltaTime,
                    (MAX_Y - CENTER_Y) * Mathf.Sin(2 * Mathf.PI / yFancyPeriod * yTimer + tOffset) + CENTER_Y);
                if(transform.position.x <= MIN_X)
                {
                    calculatedFancyMovementVars = false;
                    _movingRight = true;
                }
                if(transform.position.x >= MAX_X)
                {
                    calculatedFancyMovementVars = false;
                    _movingRight = false;
                }
                break;
        }
	}

	protected override void WhenPlayerDies()
	{
		throw new System.NotImplementedException();
	}

    protected override void TriggerDeath()
    {
        EventData.RaiseOnEnemyDeath(gameObject);

        StopAllCoroutines();
        _moveState = MoveState.IDLE;
        xMoveSpeed = DEFAULT_X_MOVE_SPEED;
        yFancyPeriod = DEFAULT_Y_FANCY_PERIOD;
    }

    private void Log(object o) => Debug.Log("[Final Boss] " + o);
}
