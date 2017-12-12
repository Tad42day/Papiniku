using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NLua;
using System.IO;

public enum Direcao
{
    direita, direita_baixo, baixo, esquerda_baixo, esquerda, esquerda_cima, cima, direita_cima, parado
}

public class Player : MonoBehaviour {

    public int cod;
    public Bullet myBullet;
    public float speed;
    public float range;
    public float rangewall;
    public LayerMask layerPlayer;
    public LayerMask layerBala;
    public LayerMask layerCranio;
    public LayerMask layerWall;
    [SerializeField]
    GameObject tiro;
    [SerializeField]
    GameObject skull;
    [SerializeField]
    Direcao currentDirecao;
    [HideInInspector]
    public int direcao;

    Lua lua;
    string path;
    Rigidbody2D rb;
    bool frenteLivre;
    public bool atirou = false;
    public bool isDead = false;
    Transform firepoint;
    Color cor;
    Manager manager;

    public Player target;

    void Start ()
    {
        manager = FindObjectOfType<Manager>();
        rb = GetComponent<Rigidbody2D>();
        firepoint = transform.Find("Firepoint");

        currentDirecao = Direcao.direita;
        direcao = (int)currentDirecao;

        CheckCor();

        lua = new Lua();
        lua.LoadCLRPackage();

        lua["jogador"] = this;

        StartCoroutine(luaUpdate());

        Call("Start");
    }

    void Update()
    {
        //if (cod == 1)
        //{
        //    int h = (int)Input.GetAxisRaw("Horizontal");
        //    int v = (int)Input.GetAxisRaw("Vertical");
        //    Vector2 dir = new Vector2(h, v);

        //    rb.velocity = dir * speed;

        //    if (dir.x != 0 || dir.y != 0)
        //    {
        //        Debug.Log("Change firepoint pos! " + dir);
        //        firepoint.localPosition = dir;
        //    }

        //    if (Input.GetKeyDown(KeyCode.Space) && !atirou)
        //        Shoot();
        //}
        //else if (cod == 2)
        //{
        //    int h = (int)Input.GetAxisRaw("Horizontal2");
        //    int v = (int)Input.GetAxisRaw("Vertical2");
        //    Vector2 dir = new Vector2(h, v);

        //    rb.velocity = dir * speed;

        //    if (dir.x != 0 || dir.y != 0)
        //    {
        //        Debug.Log("Change firepoint pos! " + dir);
        //        firepoint.localPosition = dir;
        //    }

        //    if (Input.GetKeyDown(KeyCode.K) && !atirou)
        //        Shoot();
        //}

        Call("Update");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = cor;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.GetComponentInParent<Bullet>())
        {
            Bullet b = col.transform.GetComponentInParent<Bullet>();

            if (b.state == BulletState.Safe && b.ownerCod == cod)
            {
                Destroy(col.transform.parent.gameObject);
                atirou = false;
            }
            else if (b.state == BulletState.Unsafe)
            {
                GameObject sk = Instantiate(skull, transform.position, Quaternion.identity);
                sk.GetComponent<SpriteRenderer>().color = cor;
                Die();
            }
        }
    }

    public void Teste()
    {
        Debug.Log("Funcao de teste");
    }

    #region Sensores
    public bool SensorDeParede()
    {
        Vector3 direction = firepoint.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rangewall, layerWall);

        if (hit)
            return true;
        else
            return false; 
    }
    public bool SensorDeInimigo()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range, layerPlayer);

        // Se existe pelo menos 1 inimigo no array
        //if(hit[0])
        //{
        if(hit[0].gameObject != gameObject)
        {
            target = hit[0].GetComponent<Player>();
            return true;
        }
        //}
        else
        {
            target = null;
        }

        return false;

        //if(if hit debug)
        //if (hit)
        //    return hit.collider.gameObject;
        //else
        //    return null;
    }
    public GameObject SensorDeCranios()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 8f, layerCranio);

        //foreach (Collider2D c in hit)
        //{
        //    if (c.gameObject != gameObject)
        //    {
        //        Debug.Log("Cranio detectado!");
        //        return c.gameObject;
        //    }
        //}

        if(hit[0])
        {
            if(hit[0].gameObject != gameObject)
            {
                return hit[0].gameObject;
            }
        }

        return null;

        //if(if hit debug)
        //if (hit)
        //    return hit.collider.gameObject;
        //else
        //    return null;
    }
    public GameObject SensorDeBala()
    {
        //RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2f,Vector2.zero);
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 5f, layerBala);
        //Debug.DrawLine(hit);

        //foreach (Collider2D c in hit)
        //{
        //    if (c.gameObject != gameObject)
        //    {
        //        Debug.Log("UÈÈÈÈ");
        //        return c.gameObject;
        //    }
        //}

        if (hit[0])
        {
            if (hit[0].gameObject != gameObject)
            {
                return hit[0].gameObject;
            }
        }

        return null;


        //if(if hit debug)
        //if (hit)
        //    return hit.collider.gameObject;
        //else
        //    return null;
    }
    #endregion

    #region Ações
    public void Move(int num)
    {
        Direcao dir = (Direcao) num;
        currentDirecao = (Direcao) num;
        direcao = num;

        if (dir == Direcao.cima)
        {
            rb.velocity = new Vector3(0, 1) * speed;
            firepoint.localPosition = new Vector2(0, 1);
        }
        else if (dir == Direcao.esquerda_baixo)
        {
            rb.velocity = new Vector3(-1, -1 ) * speed;
            firepoint.localPosition = new Vector2(-1, -1);

        }
        else if (dir == Direcao.direita_baixo)
        {
            rb.velocity = new Vector3(1, -1 ) * speed;
            firepoint.localPosition = new Vector2(1, -1);

        }
        else if (dir == Direcao.esquerda_cima)
        {
            rb.velocity = new Vector3(-1, 1 ) * speed;
            firepoint.localPosition = new Vector2(-1, 1);

        }
        else if (dir == Direcao.direita)
        {
            rb.velocity = new Vector3(1, 0 ) * speed;
            firepoint.localPosition = new Vector2(1, 0);

        }
        else if (dir == Direcao.esquerda)
        {
            rb.velocity = new Vector3(-1, 0 ) * speed;
            firepoint.localPosition = new Vector2(-1, 0);

        }
        else if (dir == Direcao.baixo)
        {
            rb.velocity = new Vector3(0, -1 ) * speed;
            firepoint.localPosition = new Vector2(0, -1);

        }
        else if (dir == Direcao.cima)
        {
            rb.velocity = new Vector3(0, 1 ) * speed;
            firepoint.localPosition = new Vector2(0, 1);

        } else if (dir == Direcao.parado)
        {
            rb.velocity = Vector3.zero;
        }

    }
    public void Shoot()
    {
        if (!atirou)
        {
            GameObject obj = Instantiate(tiro, firepoint.position, Quaternion.identity);
            obj.GetComponent<SpriteRenderer>().color = cor;
            obj.GetComponent<Bullet>().direcao = new Vector2(firepoint.localPosition.x, firepoint.localPosition.y);
            obj.GetComponent<Bullet>().ownerCod = cod;
            myBullet = obj.GetComponent<Bullet>();
            atirou = true;
        }
       
    }
    public void Die()
    {
        gameObject.SetActive(false);
        manager.Respawn(cod);
    }
    #endregion

    #region Métodos
    void CheckCor()
    {
        switch (cod)
        {
            case 1:
                cor = Color.green;
                break;
            case 2:
                cor = Color.red;
                break;
            case 3:
                cor = Color.yellow;
                break;
            case 4:
                cor = Color.blue;
                break;
        }

        GetComponent<SpriteRenderer>().color = cor;
    }
    public int GetDirecaoTo(Vector2 pos)
    {
        Vector2 positionV2 = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = pos - positionV2;
        direction.Normalize();

        Debug.DrawRay(positionV2, Vector2.right * 2f, cor);
        Debug.DrawRay(positionV2, direction * 2f, cor);

        float angulo = Vector2.SignedAngle(transform.right, direction);

        if(angulo >= -30 && angulo <= 30)
        {
            return (int)Direcao.direita;
        }
        else if(angulo >= 30 && angulo <= 60)
        {
            return (int)Direcao.direita_cima;
        }
        else if (angulo >= 60 && angulo <= 120)
        {
            return (int)Direcao.cima;
        }
        else if (angulo >= 120 && angulo <= 150)
        {
            return (int)Direcao.esquerda_cima;
        }
        else if (angulo >= 150 || angulo <= -150)
        {
            return (int)Direcao.esquerda;
        }
        else if (angulo >= -150 && angulo <= -120)
        {
            return (int)Direcao.esquerda_baixo;
        }
        else if (angulo >= -120 && angulo <= -60)
        {
            return (int)Direcao.baixo;
        }
        else if (angulo >= -60 && angulo <= -30)
        {
            return (int)Direcao.direita_baixo;
        }

        return -1;
    }
    public Vector2 GetPos()
    {
        return transform.position;
    }
    public Vector2 posicaoBala()
    {
        if (myBullet == null)
            return Vector2.zero;

        return myBullet.transform.position;
    }
    #endregion

    #region Codigo LUA
    IEnumerator luaUpdate()
    {
        while (true)
        {
#if UNITY_EDITOR
            path = File.ReadAllText("./Assets/LuaScripts/Player" + cod + ".lua");
#else
            path = File.ReadAllText("./LuaScripts/Player" + cod + ".lua");
#endif

            try
            {
                lua.DoString(path);
            }
            catch (NLua.Exceptions.LuaException e)
            {
                Debug.LogError(FormatException(e), gameObject);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    public System.Object[] Call(string function, params System.Object[] args)
    {
        System.Object[] result = new System.Object[0];
        if (lua == null) return result;
        LuaFunction lf = lua.GetFunction(function);
        if (lf == null) return result;
        try
        {
            // Note: calling a function that does not 
            // exist does not throw an exception.
            if (args != null)
            {
                result = lf.Call(args);
            }
            else
            {
                result = lf.Call();
            }
        }
        catch (NLua.Exceptions.LuaException e)
        {
            Debug.LogError(FormatException(e), gameObject);
            throw e;
        }
        return result;
    }
    public System.Object[] Call(string function)
    {
        return Call(function, null);
    }
    public static string FormatException(NLua.Exceptions.LuaException e)
    {
        string source = (string.IsNullOrEmpty(e.Source)) ? "<no source>" : e.Source.Substring(0, e.Source.Length - 2);
        return string.Format("{0}\nLua (at {2})", e.Message, string.Empty, source);
    }
#endregion
}
