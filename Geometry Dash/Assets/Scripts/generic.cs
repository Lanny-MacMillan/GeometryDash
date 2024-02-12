using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generic 
{
    static public void LimitYVelocity(float _limit, Rigidbody2D _rb)
    {
        int gravityMultiplier = (int)(Mathf.Abs(_rb.velocity.y) / _rb.velocity.y);

        if (_rb.velocity.y * -gravityMultiplier > _limit)
            _rb.velocity = Vector2.up * -_limit * gravityMultiplier;
    }

    // need                                rb         movement script
    // _onGroundRequired used for cube when ground is needed for jump, UFO does not
    // _initialVelocity controls power of jump
    // _gravityScale speed of falling
    // _canHold
    // _flipOnClick for the ball and spider, they will flip gravity on click, ufo does not
    static public void CreateGameMode(Rigidbody2D _rb, Movement _host, bool _onGroundRequired, float _initialVelocity, float _gravityScale, bool _canHold = false, bool _flipOnClick = false, float rotationMod = 0, float _yVelocityLimit = Mathf.Infinity)
    {
        if (!Input.GetMouseButton(0) || _canHold & _host.OnGround())
            _host.clickProcessed = false;



        _rb.gravityScale = _gravityScale * _host.Gravity;
        LimitYVelocity(_yVelocityLimit, _rb);



        if (Input.GetMouseButton(0))
        {
            if (_host.OnGround() && !_host.clickProcessed || !_onGroundRequired)
            {
                _host.clickProcessed = true;
                _rb.velocity = Vector2.up * _initialVelocity * _host.Gravity;
                _host.Gravity *= _flipOnClick ? -1 : 1;
            }
        }

        if (_host.OnGround() || !_onGroundRequired)
        {
            _host.Sprite.rotation = Quaternion.Euler(0, 0, Mathf.Round(_host.Sprite.rotation.z / 90) * 90); // rounds off to the nearest 90 degrees
        }
        else
            _host.Sprite.Rotate(Vector3.back, rotationMod * Time.deltaTime * _host.Gravity);
    }
}
