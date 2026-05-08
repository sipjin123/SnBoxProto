using Sandbox;

public partial class EnemyBase : Component, IActor
{
	protected override void OnStart()
	{
		
	}	
	protected override void OnUpdate()
	{

	}
	
	  public EnemyPoolManager Pool { get; set; }

    [Property]
    public GameObject Visuals { get; set; }

    [Property]
    public Collider Collider { get; set; }

    [Sync]
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
        }
    }

	[Property]
    private float _health;

    [Sync]
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

            if ( Visuals != null )
                Visuals.Enabled = value;

            if ( Collider != null )
                Collider.Enabled = value;
        }
    }

    private bool _isActive;

    [Sync]
    public Vector3 StartPosition
    {
        get => _startPosition;
        set
        {
            _startPosition = value;

            WorldPosition = value;
        }
    }

    private Vector3 _startPosition;

    [Sync]
    public Rotation StartRotation
    {
        get => _startRotation;
        set
        {
            _startRotation = value;

            WorldRotation = value;
        }
    }

    private Rotation _startRotation;

    public void ResetEnemy()
    {
        Health = 100f;

        SetActiveState( true );
		var Body = GameObject.GetComponent<Rigidbody>();
		if ( Body != null )
		{
			Body.Velocity = Vector3.Zero;
			Body.AngularVelocity = Vector3.Zero;

			Body.Enabled = false;
			Body.Enabled = true;

			Body.Reset();
		}
        GameObject.Network.Refresh();
    }

    public void TakeDamage( float damage )
    {
        if ( !Networking.IsHost )
            return;

        if ( !IsActive )
            return;

        Health -= damage;

        if ( Health <= 0f )
        {
            Die();
        }
    }

    private void Die()
    {
        Pool?.ReturnEnemy( this );
    }

    public void SetActiveState( bool active )
    {
        IsActive = active;
    }
}
