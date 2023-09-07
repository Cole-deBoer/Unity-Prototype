using Cinemachine;
using UnityEngine;

internal class Exterminate : Spells
{
    public override float Damage { get; set; } = 25f;
    protected sealed override float Cooldown { get; set; } = 10;

    private readonly WeaponController _controller;
    private readonly PlayerAttributes _attributes;
    private readonly Animator _animator;
    private readonly Rigidbody _playerRigidbody;
    private readonly CinemachineVirtualCamera _camera;

    public Exterminate(WeaponController controller, PlayerAttributes attributes, Animator animator, Rigidbody playerRigidbody, CinemachineVirtualCamera camera)
    {
        _controller = controller;
        _attributes = attributes;
        _animator = animator;
        _playerRigidbody = playerRigidbody;
        _camera = camera;
    }

    internal override void Execute(ref float cooldown)
    {
        if (!(cooldown <= 0)) return;
        _controller.canAttack = false;
        _attributes.attack = (int)Damage;

        var distanceToTarget = _playerRigidbody.position - _camera.LookAt.position;
        _playerRigidbody.transform.position -= distanceToTarget + _playerRigidbody.transform.forward;

        _animator.SetBool(_animator.parameters[1].name, true);
        _controller.StartCoroutine(_controller.ResetAttackCooldown(_attributes.attackSpeed * 3, _animator.parameters[1]));
        cooldown = Cooldown;
    }
}