using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class XxShoot : NetworkBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private AudioClip _spawnClip;
    [SerializeField] private float _projectileSpeed = 700;
    [SerializeField] private float _curCooldown = 0.5f;
    [SerializeField] private float _cooldownNormal = 0.5f;
    [SerializeField] private float _cooldownTired = 0.5f;
    [SerializeField] private Transform _spawner;

    private float _lastFired = float.MinValue;
    private bool _fired;

    private Rigidbody rb;

    [Header("Recoil")]
    [SerializeField] private float forceBack;
    [SerializeField] private bool shootBack;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        var Egg = GetComponentInChildren<PlayerEgg>();

        var player = GetComponent<XxMovement>();
        if (Egg == null)
        {
            _curCooldown = _cooldownTired;
            player.tiredLife = true;
        }
        else
        {
            if (Egg._state == PlayerEgg.state.POWEROUT)
            {
                _curCooldown = _cooldownTired;
                player.tiredLife = true;
            }
            else
            {
                _curCooldown = _cooldownNormal;
                player.tiredLife = false;
            }
        }

        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        //Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            //hitTransform = raycastHit.transform;
        }

        if (Input.GetMouseButton(0) && _lastFired + _curCooldown < Time.time)
        {

            _lastFired = Time.time;

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;

            //var dir = transform.forward;
            var dir = (mouseWorldPosition - transform.position).normalized;





            if (GetComponent<XxMovement>().hadEgg == true)
            {
                Egg.curPowerEgg -= 1;
            }


            RequestFireServerRpc(dir);
            ExecuteShoot(dir);
            StartCoroutine(ToggleLagIndicator());
        }
    }

    [ServerRpc]
    private void RequestFireServerRpc(Vector3 dir)
    {
        FireClientRpc(dir);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 dir)
    {
        if (!IsOwner) ExecuteShoot(dir);
    }

    private void ExecuteShoot(Vector3 dir)
    {
        /*
        var projectile = Instantiate(_projectile, _spawner.position, Quaternion.identity);
        projectile.Init(dir * _projectileSpeed);
        AudioSource.PlayClipAtPoint(_spawnClip, transform.position);
        */

        var projectile = Instantiate(_projectile, _spawner.position, Quaternion.LookRotation(dir, Vector3.up));
        projectile.Init(dir * _projectileSpeed);
        AudioSource.PlayClipAtPoint(_spawnClip, transform.position);

        if (shootBack)
        {
            rb.AddForce(transform.forward * forceBack * -1, ForceMode.Impulse);
        }
    }

    private IEnumerator ToggleLagIndicator()
    {

        _fired = true;
        yield return new WaitForSeconds(0.2f);
        _fired = false;

    }


}
