using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerMovement : MonoBehaviour
{
    private BasicMazeGenerator Maze;
    private int _column;
    private int _row;
    private Transform _player;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private AudioSource _audio;

    public AudioClip AngrySound;
    public AudioClip AttackSound;
    public AudioClip DamageSound;

    private const float speed = 1.5f;
    private Vector3 _target;
    private Direction _direction;
    private Quaternion _destRotation;
    private bool _attack;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        SearchForPlayer();
        Move();
    }

    private void Move()
    {
        Vector3 p = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime );
        _rigidbody.MovePosition(p);
        if (Vector3.Distance(transform.position, _target) < 0.001f)
        {
            GetNextPoint();
            _attack = false;
        }
    }

    private void Rotate()
    {
        // The step size is equal to speed times frame time.
        var step = speed * 100 * Time.deltaTime;

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _destRotation, step);
    }

    public void Init(BasicMazeGenerator maze, int column, int row, GameObject capsule)
    {
        Maze = maze;
        _column = column;
        _row = row;
        _target = new Vector3(column * MazeSpawner.CellHeight, 0, _row * MazeSpawner.CellWidth);
        _player = capsule.transform;
    }

    private void SearchForPlayer()
    {
        if (!_attack && Vector3.Distance(transform.position, _player.position) < 2f * MazeSpawner.CellWidth)
        {
            _attack = true;
            _column = (int) Math.Abs(_player.position.x / MazeSpawner.CellHeight);
            _row = (int) Math.Abs(_player.position.z / MazeSpawner.CellWidth);
            _animator.SetTrigger("fast");
            if (_audio != null && AngrySound != null)
            {
                _audio.PlayOneShot(AngrySound);
            }
            SetTarget();
            _target = new Vector3(_player.position.x, 0.1f, _player.position.z);
        }

        if (_attack && Vector3.Distance(transform.position, _player.position) <= 1.5f)
        {
            _animator.SetTrigger("attack");
            Attack();
        }
        if(_attack && Vector3.Distance(transform.position, _player.position) >= 2f * MazeSpawner.CellHeight)
        {
            _attack = false;
            _animator.SetTrigger("crawl");
            GetNextPoint();
        }
    }

    private void Attack()
    {
        if (_audio != null && AttackSound != null && !_audio.isPlaying)
        {
            _audio.PlayOneShot(AttackSound);
            Managers.Player.ChangeHealth(10);
        }
        
    }

    private bool GetNextPoint()
    {
        MazeCell cell = new MazeCell();
        try
        {
            cell = Maze.GetMazeCell(_row, _column);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.Log(_column +":"+_row);
        }

        return DateTime.Now.Millisecond % 2 == 0
            ? GetNext1(cell)
            : GetNext2(cell);
    }

    private bool GetNext1(MazeCell cell)
    {
        if (_direction == Direction.Start || _direction == Direction.Right)
        {
            if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }
            else if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }
            else if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }
            else if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }

            return SetTarget();
        }
        
        if (_direction == Direction.Left)
        {
            if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }
            else if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }
            else if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }
            else if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }

            return SetTarget();
        }

        if (_direction == Direction.Front)
        {
            if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }
            else if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }
            else if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }
            else if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }
            
            return SetTarget();
        }

        if (_direction == Direction.Back)
        {
            if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }
            else if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }
            else if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }
            else if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }

            return SetTarget();
        }

        return false;
    }

    private bool GetNext2(MazeCell cell)
    {
        if (_direction == Direction.Start || _direction == Direction.Right)
        {
            if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }
            else if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }
            else if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }
            else if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }

            return SetTarget();
        }
        
        if (_direction == Direction.Left)
        {
            if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }
            else if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }
            else if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }
            else if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }

            return SetTarget();
        }

        if (_direction == Direction.Front)
        {
            if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }
            else if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }
            else if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }
            else if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }
            
            return SetTarget();
        }

        if (_direction == Direction.Back)
        {
            if (!cell.WallBack)
            {
                _row--;
                _direction = Direction.Back;
            }
            else if (!cell.WallRight)
            {
                _column++;
                _direction = Direction.Right;
            }
            else if (!cell.WallLeft)
            {
                _column--;
                _direction = Direction.Left;
            }
            else if (!cell.WallFront)
            {
                _row++;
                _direction = Direction.Front;
            }

            return SetTarget();
        }

        return false;
    }

    private bool SetTarget()
    {
        var yRot = 0;
        if (_direction == Direction.Front)
        {
            yRot = 90;
        }
        if (_direction == Direction.Back)
        {
            yRot = -90;
        }
        if (_direction == Direction.Right)
        {
            yRot = 180;
        }
        if (_direction == Direction.Left)
        {
            yRot = 0;
        }
        _destRotation = Quaternion.Euler(0, yRot, 0);
        _target = new Vector3(_column * MazeSpawner.CellWidth, 0.1f, _row * MazeSpawner.CellHeight);
        return true;
    }

    private void Hit()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        _audio.PlayOneShot(DamageSound);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
    }
}
