using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class SpellsController : MonoBehaviour
{
    private Spells _spell;
    private LockOnController _lockedOnCheck;
    private Animator _daggerAnimator;
    private WeaponController _weaponController;
    private PlayerAttributes _attributes;
    private Rigidbody _playerRigidbody;
    private CinemachineVirtualCamera _camera;
    private SpellType _activeSpell;
    private float _spellCooldown;
    private bool _recharging;

    private void Start()
    {
        CheckNotNull();

        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _playerRigidbody = gameObject.GetComponent<Rigidbody>();
        _lockedOnCheck = FindObjectOfType<LockOnController>();
        _weaponController = FindObjectOfType<WeaponController>();
        _attributes = FindObjectOfType<PlayerAttributes>();
        _daggerAnimator = gameObject.GetComponentInChildren<Animator>();

        _activeSpell = SpellType.Exterminate;
    }

    private void Update()
    {
        CreateSpell(SelectSpell());
    }

    private void CheckNotNull()
    {
        var spellNotNull = Enum.TryParse(_activeSpell.ToString(), out _activeSpell);
        if (spellNotNull)
        {
            _activeSpell = Enum.GetValues(typeof(SpellType)).Cast<SpellType>().Min();

        }
    }

    private Spells SelectSpell()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _activeSpell++;
        }

        if (_activeSpell > Enum.GetValues(typeof(SpellType)).Cast<SpellType>().Max())
        {
            _activeSpell = Enum.GetValues(typeof(SpellType)).Cast<SpellType>().Min();
        }

        return _activeSpell switch
        {
            SpellType.Exterminate => _spell = new Exterminate(_weaponController, _attributes, _daggerAnimator, _playerRigidbody, _camera),
            _ => throw new ArgumentException("No Valid Spell Has Been Selected")
        };
    }

    private void CreateSpell(Spells spell)
    {
        if (!Input.GetKeyDown(KeyCode.E) || !_lockedOnCheck.lockedOn) return;
        if (_weaponController.canAttack)
        {
            spell.Execute(ref _spellCooldown);
        }
        if (_spellCooldown > 0 && !_recharging)
        {
            StartCoroutine(CoolDownHandler());
        }
    }

    private IEnumerator CoolDownHandler()
    {
        _recharging = true;
        while (_spellCooldown > 0)
        {
            yield return new WaitForSeconds(1);
            _spellCooldown--;
            print(_spellCooldown);
            if (!(_spellCooldown <= 0)) continue;
            _recharging = false;
            break;
        }
    }
}